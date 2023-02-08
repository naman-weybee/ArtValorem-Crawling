using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Phillips_Crawling_Task.Service;

namespace ArtValorem_Crawling
{
    class Program
    {
        private const string Url = "http://www.artvalorem.fr/recherche?language=fr&query=&isvisible=&myGroup=1&advancedRecherche=on&actuDatefilter=&venteType=all&ordre=score&categorie=9";

        static void Main(string[] args)
        {
            GetWatchDetails();
            Console.ReadLine();
        }

        public static void GetFullyLoadedWebPage(WebDriver driver)
        {
            long scrollHeight = 0;
            IJavaScriptExecutor js = driver;
            do
            {
                var newScrollHeight = (long)js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight); return document.body.scrollHeight;");
                if (newScrollHeight != scrollHeight)
                {
                    scrollHeight = newScrollHeight;
                    Thread.Sleep(3000);
                }
                else
                {
                    Thread.Sleep(4000);
                    break;
                }
            } while (true);
        }

        public static string GetFullyLoadedWebPageContent(WebDriver driver)
        {
            long scrollHeight = 0;
            IJavaScriptExecutor js = driver;
            do
            {
                var newScrollHeight = (long)js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight); return document.body.scrollHeight;");
                if (newScrollHeight != scrollHeight)
                {
                    scrollHeight = newScrollHeight;
                    Thread.Sleep(3000);
                }
                else
                {
                    Thread.Sleep(4000);
                    break;
                }
            } while (true);
            return driver.PageSource;
        }

        private static void GetWatchDetails()
        {
            ChromeOptions opt = new();
            opt.AddArgument("--log-level=3");
            opt.AddArguments("--disable-gpu");
            opt.AddArguments("--start-maximized");
            opt.AddArguments("--no-sandbox");
            opt.AddArguments("--disable-notifications");

            ChromeDriver driver = new(ChromeDriverService.CreateDefaultService(), opt, TimeSpan.FromMinutes(3));
            driver.Navigate().GoToUrl(Url);
            Thread.Sleep(3000);

            try
            {
                driver.FindElement(By.XPath(XpathStrings.AcceptCoockiesXpath)).Click();
                GetFullyLoadedWebPage(driver);
            }
            catch (Exception) { Thread.Sleep(3000); }

            try
            {
                int count = 2;
                do
                {
                    NextPageControl(driver, count);
                    count++;
                } while (true);
            }
            catch (Exception) { Thread.Sleep(3000); }

            var pageSource = GetFullyLoadedWebPageContent(driver);
            var Details = new HtmlDocument();
            Details.LoadHtml(pageSource);
        }

        public static void NextPageControl(ChromeDriver driver, int count)
        {
            driver.FindElement(By.XPath($"(//a[.='{count}'])[2]")).Click();
            GetFullyLoadedWebPage(driver);
        }
    }
}