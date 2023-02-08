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
        private const string BaseUrl = "http://www.artvalorem.fr";

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
            try
            {
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
                        var pageSource = GetFullyLoadedWebPageContent(driver);
                        var Details = new HtmlDocument();
                        Details.LoadHtml(pageSource);

                        var AllAuctionList = Details.DocumentNode.SelectNodes(XpathStrings.AuctionListXpath);
                        if (AllAuctionList != null)
                        {
                            foreach (var auction in AllAuctionList)
                            {
                                var auctionImageUrl = string.Empty;
                                var auctionTitle = string.Empty;
                                var auctionDescriptionString = string.Empty;
                                var auctionDescription = string.Empty;
                                var lotNumber = string.Empty;
                                var estimationPriceString = string.Empty;
                                var estimationPriceStart = string.Empty;
                                var estimationPriceEnd = string.Empty;
                                var estimationPriceCurrency = string.Empty;
                                var resultPriceString = string.Empty;
                                var resultPrice = string.Empty;
                                var resultPriceCurrency = string.Empty;
                                var saleOfDateString = string.Empty;
                                var saleStartMonthString = string.Empty;
                                var saleStartDate = string.Empty;
                                var saleStartMonth = string.Empty;
                                var saleStartYear = string.Empty;
                                var auctionLink = string.Empty;
                                var auctionId = string.Empty;

                                auctionImageUrl = auction.SelectSingleNode(XpathStrings.AuctionImageUrlXpath)?.GetAttributes("src").First().Value.Replace("amp;", "") ?? string.Empty;
                                auctionTitle = auction.SelectSingleNode(XpathStrings.AuctionTitleXpath)?.InnerText.Replace("\n", "").Replace("\r", "").Replace("&nbsp", "").Trim() ?? string.Empty;
                                auctionDescriptionString = auction.SelectSingleNode(XpathStrings.AuctionDescriptionXpath)?.InnerHtml.Replace("\n", "").Replace("\r", "").Replace("&nbsp", "").Trim() ?? string.Empty;
                                lotNumber = auction.SelectSingleNode(XpathStrings.LotCountXpath)?.InnerText.Replace("\n", "").Replace("\r", "").Replace("&nbsp", "").Trim() ?? string.Empty;
                                estimationPriceString = auction.SelectSingleNode(XpathStrings.EstimationPriceXpath)?.InnerText.Replace("\n", "").Replace("\r", "").Replace("&nbsp", "").Trim() ?? string.Empty;
                                resultPriceString = auction.SelectSingleNode(XpathStrings.ResultPriceXpath)?.InnerText.Replace("\n", "").Replace("\r", "").Replace("&nbsp", "").Trim() ?? string.Empty;
                                saleOfDateString = auction.SelectSingleNode(XpathStrings.SaleOfDateXpath)?.InnerText.Replace("\n", "").Replace("\r", "").Replace("&nbsp", "").Trim() ?? string.Empty;
                                auctionLink = BaseUrl + auction.SelectSingleNode(XpathStrings.AuctionLinkXpath)?.GetAttributes("href").First().Value ?? string.Empty;

                                if (!string.IsNullOrEmpty(auctionDescriptionString))
                                    auctionDescription = RegexString.AuctionDescriptionRegex.Match(auctionDescriptionString).Groups[1].Value.Trim() ?? string.Empty;

                                if (!string.IsNullOrEmpty(estimationPriceString))
                                {
                                    estimationPriceStart = RegexString.EstimationPriceRegex.Match(estimationPriceString).Groups[1].Value ?? string.Empty;
                                    estimationPriceEnd = RegexString.EstimationPriceRegex.Match(estimationPriceString).Groups[3].Value ?? string.Empty;
                                    estimationPriceCurrency = RegexString.EstimationPriceRegex.Match(estimationPriceString).Groups[5].Value ?? string.Empty;
                                }

                                if (!string.IsNullOrEmpty(resultPriceString))
                                {
                                    resultPrice = RegexString.ResultPriceRegex.Match(resultPriceString).Groups[1].Value ?? string.Empty;
                                    resultPriceCurrency = RegexString.ResultPriceRegex.Match(resultPriceString).Groups[3].Value ?? string.Empty;
                                }

                                if (!string.IsNullOrEmpty(saleOfDateString))
                                {
                                    saleStartDate = RegexString.SaleOfDateRegex.Match(saleOfDateString).Groups[1].Value ?? string.Empty;
                                    saleStartMonthString = RegexString.SaleOfDateRegex.Match(saleOfDateString).Groups[3].Value ?? string.Empty;
                                    saleStartYear = RegexString.SaleOfDateRegex.Match(saleOfDateString).Groups[5].Value ?? string.Empty;

                                    if (!string.IsNullOrEmpty(saleStartMonthString))
                                        saleStartMonth = Enum.GetName(typeof(NumberToMonth), Convert.ToInt32(saleStartMonthString)) ?? string.Empty;
                                }

                                if (!string.IsNullOrEmpty(auctionLink))
                                    auctionId = RegexString.AuctionIdRegex.Match(auctionLink).Groups[1].Value ?? string.Empty;

                                Console.WriteLine($"----------Auction with Id = {auctionId}----------");
                                Console.WriteLine($"Auction Title: {auctionTitle}");
                                Console.WriteLine($"Auction Image Url: {auctionImageUrl}");
                                Console.WriteLine($"Auction Description: {auctionDescription}");
                                Console.WriteLine($"Auction Link: {auctionLink}");
                                Console.WriteLine($"Lot Number: {lotNumber}");
                                Console.WriteLine($"Estimation Price Start: {estimationPriceStart}");
                                Console.WriteLine($"Estimation Price End: {estimationPriceEnd}");
                                Console.WriteLine($"Estimation Price Currency: {estimationPriceCurrency}");
                                Console.WriteLine($"Result Price: {resultPrice}");
                                Console.WriteLine($"Result Price Currency: {resultPriceCurrency}");
                                Console.WriteLine($"Sale Start Date: {saleStartDate}");
                                Console.WriteLine($"Sale Start Month: {saleStartMonth}");
                                Console.WriteLine($"Sale Start Year: {saleStartYear}");
                                Console.WriteLine();
                            }
                        }
                        NextPageControl(driver, count);
                        count++;
                    } while (true);
                }
                catch (Exception) { Thread.Sleep(3000); }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void NextPageControl(ChromeDriver driver, int count)
        {
            driver.FindElement(By.XPath($"(//a[.='{count}'])[2]")).Click();
            GetFullyLoadedWebPage(driver);
        }
    }
}