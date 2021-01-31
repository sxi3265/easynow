using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using EasyNow.ApiClient.WxPusher;
using EasyNow.Utility.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using Microsoft.Extensions.DependencyInjection;
using OpenCvSharp;
using PuppeteerSharp.Input;

namespace EasyNow.Job.Smzdm
{
    public class SmzdmCheckInJob:IJob
    {
        private readonly ILogger _logger;

        private readonly IConfigurationRoot _configuration;
        
        private readonly IServiceProvider _serviceProvider;

        public SmzdmCheckInJob(ILogger<SmzdmCheckInJob> logger, IConfigurationRoot configuration, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public async Task ExecuteAsync()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var args = new List<string>
            {
                "--no-sandbox",
                "--disable-setuid-sandbox",
                "--start-maximized"
            };
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                args.Add("--use-mock-keychain");
            }else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                args.Add("--disable-dev-shm-usage");
            }

            var launchOptions = new LaunchOptions
            {
                Headless = false,
                //Headless = _configuration["Headless"].To<bool>(),
                Args = args.ToArray(),
                IgnoredDefaultArgs = new []{"--enable-automation"},
                DefaultViewport = new ViewPortOptions
                {
                    Width = 1920,
                    Height = 1080
                }
            };
            var browser = await Puppeteer.LaunchAsync(launchOptions);
            var page = await browser.NewPageAsync();
            //await page.SetUserAgentAsync(_configuration["UserAgent"]);
            //await page.SetCookieAsync(_configuration["Cookies"].FromJson<CookieParam[]>());
            await page.GoToAsync("https://login.1234567.com.cn/",new NavigationOptions
            {
                Timeout = 0,
                WaitUntil = new []
                {
                    WaitUntilNavigation.Load,
                    WaitUntilNavigation.Networkidle2
                }
            });
            var element = await page.WaitForSelectorAsync("#tbname");
            await element.TypeAsync("123123", new TypeOptions
            {
                Delay = 100
            });
            element = await page.WaitForSelectorAsync("#tbpwd");
            await element.TypeAsync("123123", new TypeOptions
            {
                Delay = 100
            });
            element = await page.WaitForSelectorAsync("#btn_login");
            await element.ClickAsync();
            await Task.Delay(2000);
            element = await page.WaitForSelectorAsync("#divCaptcha");
            await element.ClickAsync();
            await Task.Delay(2000);
            element = await page.WaitForSelectorAsync(".em_fullbg");
            var fullbgBoundingBox = await element.BoundingBoxAsync();
            await element.ScreenshotAsync("full.png");
            await page.EvaluateExpressionAsync("$('.em_fullbg').attr('style','display:none !important;')");
            element = await page.WaitForSelectorAsync(".em_slice");
            var sliceBoundingBox = await element.BoundingBoxAsync();
            await page.EvaluateExpressionAsync("$('.em_slice').attr('style','display:none !important;')");
            element = await page.WaitForSelectorAsync(".em_cut_bg");
            await element.ScreenshotAsync("cut.png");
            //em_slice

            var full = Cv2.ImRead("full.png", ImreadModes.AnyColor);
            var cut = Cv2.ImRead("cut.png", ImreadModes.AnyColor);
            var result = new Mat();
            Cv2.Divide(full,cut,result,50);
            result.SaveImage("result.png");
            var img1 = result.CvtColor(ColorConversionCodes.BGR2GRAY);
            img1.SaveImage("img1.png");
            var img2=img1.Threshold(60, 200, ThresholdTypes.Binary);
            img2.SaveImage("img2.png");
            img2.FindContours(out var contours,out var hierarchyIndices,RetrievalModes.External,ContourApproximationModes.ApproxNone);
            cut.DrawContours(contours,-1,0,2);
            cut.SaveImage("result1.png");
            var posY = sliceBoundingBox.Y - fullbgBoundingBox.Y;
            element = await page.WaitForSelectorAsync(".em_slider_knob");
            var boundingBox = await element.BoundingBoxAsync();
            foreach (var points in contours)
            {
                var rect = Cv2.BoundingRect(points);
                if (rect.Top >= posY - 15 && rect.Top <= posY + 15)
                {
                    await page.FocusAsync(".em_slider_knob");
                    await page.Mouse.MoveAsync(boundingBox.X+boundingBox.Width/2, boundingBox.Y+boundingBox.Height/2);
                    await page.Mouse.DownAsync();
                    await page.Mouse.MoveAsync(boundingBox.X+boundingBox.Width/2 + rect.X-10, boundingBox.Y+boundingBox.Height/2, new MoveOptions
                    {
                        Steps = 1
                    });
                    await page.Mouse.UpAsync();
                    break;
                    //full.Rectangle(rect,0,2);
                }
                
            }

            full.SaveImage("result2.png");

            //var full = Cv2.ImRead("full.png", ImreadModes.AnyColor);
            //var cut = Cv2.ImRead("cut.png", ImreadModes.AnyColor);
            //var result = new Mat();
            //Cv2.Divide(full,cut,result,50);
            //result.SaveImage("result.png");
            //var result = CalculateSSIM(full, cut);
            //var src = new Mat("cut.png");
            //var img1 = src.CvtColor(ColorConversionCodes.BGR2GRAY);
            //img1.SaveImage("img1.png");
            //var img2=img1.Threshold(60, 200, ThresholdTypes.Binary);
            //img2.SaveImage("img2.png");
            Console.ReadLine();


            //// 未登录状态，说明cookie过期
            //if ((await page.QuerySelectorAsync("a.name-link"))== null)
            //{
            //    var result=await _serviceProvider.GetService<IWxPusher>().SendMessage(new MessageReq
            //    {
            //        AppToken = _configuration["AppToken"],
            //        ContentType = ContentType.Text,
            //        Content = @"什么值得买Cookie已过期,请更新Cookie",
            //        Uids = new []{_configuration["Uid"]}
            //    });
            //    _logger.LogInformation($"什么值得买Cookie已过期,已发送微信消息,结果:{result.ToJson()}");
            //    return;
            //}
            //var element =await page.WaitForSelectorAsync("a.J_punch");
            //var text=await (await element.GetPropertyAsync("textContent")).JsonValueAsync<string>();
            //// var b=await page.EvaluateFunctionAsync<string>("e=>e.textContent",element);
            //if (text == "签到领奖")
            //{
            //    await element.ClickAsync();
            //    await Task.Delay(5000);
            //    element =await page.WaitForSelectorAsync("a.J_punch");
            //    text=await (await element.GetPropertyAsync("textContent")).JsonValueAsync<string>();
            //}

            //_logger.LogInformation(text);
        }

        public Scalar CalculateSSIM(Mat i1, Mat i2)
        {
            const double C1 = 6.5025, C2 = 58.5225;
            /***************************** INITS **********************************/
            MatType d = MatType.CV_32F;

            Mat I1 = new Mat(), I2 = new Mat();
            i1.ConvertTo(I1, d);           // cannot calculate on one byte large values
            i2.ConvertTo(I2, d);

            Mat I2_2 = I2.Mul(I2);        // I2^2
            Mat I1_2 = I1.Mul(I1);        // I1^2
            Mat I1_I2 = I1.Mul(I2);        // I1 * I2

            /***********************PRELIMINARY COMPUTING ******************************/

            Mat mu1 = new Mat(), mu2 = new Mat();   //
            Cv2.GaussianBlur(I1, mu1, new OpenCvSharp.Size(11, 11), 1.5);
            Cv2.GaussianBlur(I2, mu2, new OpenCvSharp.Size(11, 11), 1.5);

            Mat mu1_2 = mu1.Mul(mu1);
            Mat mu2_2 = mu2.Mul(mu2);
            Mat mu1_mu2 = mu1.Mul(mu2);

            Mat sigma1_2 = new Mat(), sigma2_2 = new Mat(), sigma12 = new Mat();

            Cv2.GaussianBlur(I1_2, sigma1_2, new OpenCvSharp.Size(11, 11), 1.5);
            sigma1_2 -= mu1_2;

            Cv2.GaussianBlur(I2_2, sigma2_2, new OpenCvSharp.Size(11, 11), 1.5);
            sigma2_2 -= mu2_2;

            Cv2.GaussianBlur(I1_I2, sigma12, new OpenCvSharp.Size(11, 11), 1.5);
            sigma12 -= mu1_mu2;

            ///////////////////////////////// FORMULA ////////////////////////////////
            Mat t1, t2, t3;

            t1 = 2 * mu1_mu2 + C1;
            t2 = 2 * sigma12 + C2;
            t3 = t1.Mul(t2);              // t3 = ((2*mu1_mu2 + C1).*(2*sigma12 + C2))

            t1 = mu1_2 + mu2_2 + C1;
            t2 = sigma1_2 + sigma2_2 + C2;
            t1 = t1.Mul(t2);               // t1 =((mu1_2 + mu2_2 + C1).*(sigma1_2 + sigma2_2 + C2))

            Mat ssim_map = new Mat();
            Cv2.Divide(t3, t1, ssim_map);
            ssim_map.SaveImage("ssim.png");
            // ssim_map =  t3./t1;
            Scalar mssim = Cv2.Mean(ssim_map);// mssim = average of ssim map
            return mssim;
        }
    }
}