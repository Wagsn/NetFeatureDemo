# 支持即时修改的项目演示

只需要在项目配置文件(.csproj)中加入配置项
```csproj
<ItemGroup>
    <DotNetCliToolReference Include="Microsoft.DotNet.Watcher.Tools" Version="2.0.0" />
</ItemGroup>
```
在项目文件夹下运行`dotnet watch run`命令启动项目即可
```out
watch : Started
Hello World!
watch : Exited
watch : Waiting for a file to change before restarting dotnet...
watch : Started
Hello World! after watch change
watch : Exited
watch : Waiting for a file to change before restarting dotnet...
```