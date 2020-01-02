using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluginCore
{
    public class DefaultPluginFactory : IPluginFactory
    {
        /// <summary>
        /// 已加载程序集
        /// </summary>
        public List<Assembly> LoadedAssembly { get; } = new List<Assembly>();

        protected List<PluginInfo> PluginList { get; } = new List<PluginInfo>();

        public async Task<bool> Init(PluginCoreContext context)
        {
            foreach (PluginInfo pi in PluginList)
            {
                try
                {
                    var r = await pi.Instance.Init(context);
                    pi.SetInitFail(r.Code != "0");
                    //pi.SetMessage(r.Message);
                    //Logger.Debug("plugin {0} init {1} {2}", pi.Name, r.Code, r.Message ?? "");
                }
                catch (Exception e)
                {
                    //Logger.Error("plugin {0} init error \r\n{1}", pi.Name, e.ToString());
                    //pi.SetInitFail(true);
                    //pi.SetMessage(e.Message);
                }
            }
            return true;
        }

        public void Load(string pluginPath)
        {
            if (!Directory.Exists(pluginPath))
            {
                return;
            }
            DirectoryLoader dl = new DirectoryLoader();
            var al = dl.LoadFromDirectory(pluginPath);
            al.ForEach(a =>
            {
                LoadedAssembly.Add(a);
                var tl = dl.GetTypes(a, typeof(IPlugin), (t) =>
                {
                    if (t.GetTypeInfo().IsAbstract) return false;
                    if (t.GetTypeInfo().IsPublic) return true;
                    return false;
                });

                tl.ForEach(t =>
                {
                    try
                    {
                        IPlugin p = Activator.CreateInstance(t) as IPlugin;
                        PluginInfo pi = new PluginInfo()
                        {
                            //Description = p.Description,
                            //Order = p.Order,
                            //ID = p.PluginID,
                            Type = t,
                            Instance = p,
                            //Name = p.PluginName
                        };
                        //if (typeof(IPluginConfig<PluginCoreConfig>).GetTypeInfo().IsAssignableFrom(p.GetType()))
                        //{
                        //    pi.HasConfig = true;
                        //}
                        PluginList.Add(pi);
                    }
                    catch (Exception ex)
                    {
                        //Logger.Error("can not load plugin type: {0}\r\n{1}", t.FullName, e.ToString());
                    }
                });
            });
        }

        public async Task<bool> Start(PluginCoreContext context)
        {
            foreach (PluginInfo pi in PluginList)
            {
                if (pi.InitFail)
                    continue;
                try
                {
                    var r = await pi.Instance.Start(context);
                    pi.SetStartFail(r.Code != "0");
                    pi.SetMessage(r.Message);
                    //Logger.Debug("plugin {0} start {1} {2}", pi.Name, r.Code, r.Message ?? "");
                    if (r.IsSuccess())
                    {
                        //pi.SetRunning(true);
                    }
                }
                catch (Exception e)
                {
                    //Logger.Error("plugin {0} start error \r\n{1}", pi.Name, e.ToString());
                    pi.SetStartFail(true);
                    pi.SetMessage(e.Message);
                }
            }
            return true;
        }

        public async Task<bool> Stop(PluginCoreContext context)
        {
            foreach (PluginInfo pi in PluginList)
            {
                //初始化失败 不调用停止，但如果初始化成功，后面Start失败，依然会调用Stop
                if (pi.InitFail)
                    continue;
                try
                {
                    var r = await pi.Instance.Stop(context);
                    pi.SetMessage(r.Message);
                    //Logger.Debug("plugin {0} stop {1} {2}", pi.Name, r.Code, r.Message ?? "");
                    if (r.IsSuccess())
                    {
                        pi.SetRunning(false);
                    }
                }
                catch (Exception e)
                {
                    //Logger.Error("plugin {0} stop error \r\n{1}", pi.Name, e.ToString());
                    pi.SetMessage(e.Message);
                }
            }
            return true;
        }

        protected class PluginInfo //: PluginItem
        {
            public Type Type { get; set; }
            public IPlugin Instance { get; set; }
            public bool InitFail { get; set; }
            public bool StartFail { get; set; }
            public string Message { get; set; }
            public bool IsRunning { get; set; }
            public void SetInitFail(bool fail)
            {
                this.InitFail = fail;
            }
            public void SetStartFail(bool fail)
            {
                this.StartFail = fail;
            }
            public void SetRunning(bool run)
            {
                this.IsRunning = run;
            }
            public void SetMessage(string msg)
            {
                this.Message = msg;
            }
        }
    }
}
