using System.IO;
using Android.Content;
using Autofac;
using EasyNow.App.Droid.Script.Module;
using Octokit;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EasyNow.App.Droid.Native;
using EasyNow.App.Droid.Runtime.Api;
using Java.IO;
using SharpCompress.Common;
using SharpCompress.Compressors.Xz;
using SharpCompress.Readers;
using File = Java.IO.File;
using FileMode = System.IO.FileMode;

namespace EasyNow.App.Droid.Frida
{
    public class FridaAgent
    {
        private readonly ILifetimeScope _scope;

        public FridaAgent(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public void Start()
        {
            var filePath = new File(this._scope.Resolve<Context>().FilesDir, "server").AbsolutePath;
            NativeShell.Execute($"su -c {filePath} &");
            filePath = new File(this._scope.Resolve<Context>().FilesDir, "npc/npc").AbsolutePath;
            NativeShell.Execute($"su -c 'cd {this._scope.Resolve<Context>().FilesDir.AbsolutePath}/npc &&{filePath}' &");
        }

        public async Task Download()
        {
            var filePath = new File(this._scope.Resolve<Context>().FilesDir, "server").AbsolutePath;
            if (System.IO.File.Exists(filePath))
            {
                return;
            }
            var gitHubClient = new GitHubClient(new ProductHeaderValue("EasyNow"));
            var repo=await gitHubClient.Repository.Release.GetLatest("frida","frida");
            var cpuAbi = _scope.Resolve<DeviceModule>().GetCpuAbi();
            var abi = cpuAbi switch

            {
                "arm64-v8a" => "arm64",
                "armeabi-v7a" => "arm",
                _=>cpuAbi
            };
            var asset = repo.Assets.FirstOrDefault(e => e.Name.StartsWith("frida-server")&& e.Name.EndsWith($"android-{abi}.xz"));
            if (asset != null)
            {
                using var webClient = new WebClient();
                var bytes=webClient.DownloadData(asset.BrowserDownloadUrl);
                using var ms = new MemoryStream(bytes);
                ms.Seek(0, SeekOrigin.Begin);
                using var xzStream = new XZStream(ms);
                using var fs = new FileStream(filePath, FileMode.CreateNew);
                await xzStream.CopyToAsync(fs);
                fs.Flush();
                IShell.Exec($"chmod a+x {filePath}", true);
            }

            filePath = new File(this._scope.Resolve<Context>().FilesDir, "npc").AbsolutePath;
            repo = await gitHubClient.Repository.Release.GetLatest("ehang-io", "nps");
            var name = "linux_"+(cpuAbi switch
            {
                "arm64-v8a" => "arm_v7",
                "armeabi-v7a" => "arm_v7",
                "x86"=>"386",
                "x86_64"=>"amd64",
                _=>cpuAbi
            })+"_client.tar.gz";
            asset = repo.Assets.FirstOrDefault(e => e.Name == name);
            if (asset != null)
            {
                using var webClient = new WebClient();
                var bytes=webClient.DownloadData(asset.BrowserDownloadUrl);
                using var ms = new MemoryStream(bytes);
                ms.Seek(0, SeekOrigin.Begin);
                using var reader = ReaderFactory.Open(ms);
                Directory.CreateDirectory(filePath);
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        reader.WriteEntryToDirectory(filePath,
                            new ExtractionOptions
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                    }
                }
                IShell.Exec($"chmod a+x {filePath}/npc", true);
            }

            var stream=this._scope.Resolve<Context>().Assets.Open("npc.conf");
            var npcConfFilePath = filePath + "/conf/npc.conf";
            System.IO.File.Delete(npcConfFilePath);
            using var npcConfFileStream = new FileStream(npcConfFilePath, FileMode.CreateNew);
            await stream.CopyToAsync(npcConfFileStream);
            npcConfFileStream.Flush();
        }
    }
}