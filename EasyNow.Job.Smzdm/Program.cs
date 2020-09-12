using System;
using System.Threading.Tasks;
using PuppeteerSharp;
using PuppeteerSharp.Input;

namespace EasyNow.Job.Smzdm
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false
            });
            var page = await browser.NewPageAsync();
            await page.SetViewportAsync(new ViewPortOptions
            {
                Width = 1920,
                Height = 1080
            });
            //https://zhiyou.smzdm.com/user/login/
            await page.GoToAsync("https://www.smzdm.com/");
            var element=await page.WaitForSelectorAsync("a.J_login_trigger");
            await element.ClickAsync();
            element = await page.WaitForSelectorAsync("iframe#J_login_iframe");
            var frame = await element.ContentFrameAsync();
            
            element = await frame.WaitForSelectorAsync("div.qrcode-change.J_qrcode_change");
            var box = await element.BoundingBoxAsync();
            await Task.Delay(5000);
            await page.Mouse.ClickAsync(box.X + box.Width / 3 * 2, box.Y + box.Height / 3);
            element = await frame.WaitForSelectorAsync("input#username");
            await frame.TypeAsync("input#username", "xxxxxx", new TypeOptions
            {
                Delay = 100
            });
            Console.WriteLine("Hello World!");
        }
    }
}