using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicalFileWatcherDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var pluginDir = Path.Combine(AppContext.BaseDirectory, @"Plugins");
            IFileProvider fileProvider = new PhysicalFileProvider(pluginDir);
            OldFiles.AddRange(fileProvider.GetDirectoryContents(""));
            ChangeToken.OnChange(() => fileProvider.Watch("*.dll"), () => 
            {
                var fileInfos = fileProvider.GetDirectoryContents("");
                // 删除的文件
                var delFiles = OldFiles.Where(a => !fileInfos.Any(b => b.PhysicalPath == a.PhysicalPath));
                // 添加的文件
                var addFiles = fileInfos.Where(a => !OldFiles.Any(b => b.PhysicalPath == a.PhysicalPath));
                // 修改的文件 判断：新文件中有并且修改时间大于旧文件
                var modFiles = OldFiles.Where(a => fileInfos.FirstOrDefault(b => b.PhysicalPath == a.PhysicalPath)?.LastModified > a.LastModified);

                var allFiles = delFiles.ToDictionary(a => a.Name, status => 3).ToList()
                .Union(addFiles.ToDictionary(a => a.Name, status => 3).ToList())
                .Union(modFiles.ToDictionary(a => a.Name, status => 3).ToList());
                Console.WriteLine($"{allFiles.Select(kv => $"{kv.Key}")}");

                // 最后文件列表刷新
                OldFiles.Clear();
                OldFiles.AddRange(fileInfos);
            });
            while (true)
            {
                Task.Delay(5 * 1000).Wait();
            }
        }

        public static List<IFileInfo> OldFiles { get; } = new List<IFileInfo>();


        public static void PrintDllsName(string pluginDir)
        {
            PrintDllNames(names: Directory.GetFiles(pluginDir, "*.dll").Select(Path.GetFileNameWithoutExtension));
        }

        public static void PrintDllNames(IEnumerable<string> names)
        {
            Console.WriteLine($"- {DateTime.Now.ToString("HH:mm:ss.FFFFFF")} Plugin Dlls {names.Count()}\r\n{string.Join("\r\n", names)}\r\n");
        }

        //ChangeToken.OnChange(() => fileProvider.Watch("Data.txt"), () => LoadFileAsync(fileProvider));
        public static async void LoadFileAsync(IFileProvider fileProvider)
        {
            Stream stream = fileProvider.GetFileInfo("Data.txt").CreateReadStream();
            {
                byte[] buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                Console.WriteLine(Encoding.UTF8.GetString(buffer));
            }
        }
    }
}
