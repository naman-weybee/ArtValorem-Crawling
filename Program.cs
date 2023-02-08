using ArtValorem_Crawling.Data;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
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
        private static readonly ArtValorem_DbContext _context = new();

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

        private static async void GetWatchDetails()
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
                                var auctionTitleString = string.Empty;
                                var auctionTitle = string.Empty;
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
                                var saleOfMonthString = string.Empty;
                                var saleOfDate = string.Empty;
                                var saleOfMonth = string.Empty;
                                var saleOfYear = string.Empty;
                                var auctionLink = string.Empty;
                                var auctionId = string.Empty;

                                lotNumber = auction.SelectSingleNode(XpathStrings.LotCountXpath)?.InnerText.Replace("\n", "").Replace("\r", "").Replace("&nbsp", "").Trim() ?? string.Empty;
                                estimationPriceString = auction.SelectSingleNode(XpathStrings.EstimationPriceXpath)?.InnerText.Replace("\n", "").Replace("\r", "").Replace("&nbsp", "").Trim() ?? string.Empty;
                                resultPriceString = auction.SelectSingleNode(XpathStrings.ResultPriceXpath)?.InnerText.Replace("\n", "").Replace("\r", "").Replace("&nbsp", "").Trim() ?? string.Empty;
                                saleOfDateString = auction.SelectSingleNode(XpathStrings.SaleOfDateXpath)?.InnerText.Replace("\n", "").Replace("\r", "").Replace("&nbsp", "").Trim() ?? string.Empty;
                                auctionLink = BaseUrl + auction.SelectSingleNode(XpathStrings.AuctionLinkXpath)?.GetAttributes("href").First().Value ?? string.Empty;

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
                                    saleOfDate = RegexString.SaleOfDateRegex.Match(saleOfDateString).Groups[1].Value ?? string.Empty;
                                    saleOfMonthString = RegexString.SaleOfDateRegex.Match(saleOfDateString).Groups[3].Value ?? string.Empty;
                                    saleOfYear = RegexString.SaleOfDateRegex.Match(saleOfDateString).Groups[5].Value ?? string.Empty;

                                    if (!string.IsNullOrEmpty(saleOfMonthString))
                                        saleOfMonth = Enum.GetName(typeof(NumberToMonth), Convert.ToInt32(saleOfMonthString)) ?? string.Empty;
                                }

                                if (!string.IsNullOrEmpty(auctionLink))
                                    auctionId = RegexString.AuctionIdRegex.Match(auctionLink).Groups[1].Value ?? string.Empty;

                                driver.Navigate().GoToUrl(auctionLink);
                                var lotDetailsPage = GetFullyLoadedWebPageContent(driver);
                                var lotDetails = new HtmlDocument();
                                lotDetails.LoadHtml(lotDetailsPage);

                                auctionDescription = lotDetails.DocumentNode.SelectNodes(XpathStrings.AuctionDescriptionXpath)?.First().InnerText.Trim().Replace("\n", ", ").Replace("\r", "").Replace("&nbsp", "").Trim() ?? string.Empty;
                                auctionTitleString = lotDetails.DocumentNode.SelectNodes(XpathStrings.AuctionTitleXpath)?.First().InnerText.Trim().Replace("\n", "").Replace("\r", "").Replace("&nbsp", "").Trim() ?? string.Empty;

                                if (!string.IsNullOrEmpty(auctionTitleString))
                                    auctionTitle = RegexString.AuctionTitleRegex.Match(auctionTitleString).Groups[1].Value.Replace("...", "").Replace("\n", "").Replace("\r", "").Replace("&nbsp", "") ?? string.Empty;

                                driver.FindElement(By.XPath(XpathStrings.CatalogueInfoXpath)).Click();
                                var catalogueDetailsPage = GetFullyLoadedWebPageContent(driver);
                                var catalogueDetails = new HtmlDocument();
                                catalogueDetails.LoadHtml(catalogueDetailsPage);

                                Console.WriteLine($"----------Auction with Id = {auctionId}----------");
                                Console.WriteLine($"Auction Title: {auctionTitle}");
                                Console.WriteLine($"Auction Description: {auctionDescription}");
                                Console.WriteLine($"Auction Link: {auctionLink}");
                                Console.WriteLine($"Lot Number: {lotNumber}");
                                Console.WriteLine($"Estimation Price Start: {estimationPriceStart}");
                                Console.WriteLine($"Estimation Price End: {estimationPriceEnd}");
                                Console.WriteLine($"Estimation Price Currency: {estimationPriceCurrency}");
                                Console.WriteLine($"Result Price: {resultPrice}");
                                Console.WriteLine($"Result Price Currency: {resultPriceCurrency}");
                                Console.WriteLine($"Sale Of Date: {saleOfDate}");
                                Console.WriteLine($"Sale Of Month: {saleOfMonth}");
                                Console.WriteLine($"Sale Of Year: {saleOfYear}");
                                Console.WriteLine();

                                var auctionRecord = await _context.tbl_Auctions.Where(x => x.Id == auctionId).FirstOrDefaultAsync();
                                if (auctionRecord != null)
                                {
                                    auctionRecord.Id = auctionId;
                                    auctionRecord.Id = auctionId;
                                    auctionRecord.Title = auctionTitle;
                                    auctionRecord.Description = auctionDescription;
                                    auctionRecord.Link = auctionLink;
                                    auctionRecord.LotNumber = lotNumber;
                                    auctionRecord.EstimationPriceStart = estimationPriceStart;
                                    auctionRecord.EstimationPriceEnd = estimationPriceEnd;
                                    auctionRecord.EstimationPriceCurrency = estimationPriceCurrency;
                                    auctionRecord.ResultPrice = resultPrice;
                                    auctionRecord.ResultPriceCurrency = resultPriceCurrency;
                                    auctionRecord.SaleOfDate = saleOfDate;
                                    auctionRecord.SaleOfMonth = saleOfMonth;
                                    auctionRecord.SaleOfYear = saleOfYear;
                                    _context.tbl_Auctions.Update(auctionRecord);
                                }
                                else
                                {
                                    Auctions newAuction = new()
                                    {
                                        Id = auctionId,
                                        Title = auctionTitle,
                                        Description = auctionDescription,
                                        Link = auctionLink,
                                        LotNumber = lotNumber,
                                        EstimationPriceStart = estimationPriceStart,
                                        EstimationPriceEnd = estimationPriceEnd,
                                        EstimationPriceCurrency = estimationPriceCurrency,
                                        ResultPrice = resultPrice,
                                        ResultPriceCurrency = resultPriceCurrency,
                                        SaleOfDate = saleOfDate,
                                        SaleOfMonth = saleOfMonth,
                                        SaleOfYear = saleOfYear
                                    };
                                    await _context.tbl_Auctions.AddAsync(newAuction);
                                }

                                var imageContainer = driver.FindElements(By.XPath(XpathStrings.AuctionImageContainerXpath));
                                if (imageContainer != null)
                                {
                                    Console.WriteLine($"----------Lot Image with Auction Id = {auctionId}----------");
                                    foreach (var image in imageContainer)
                                    {
                                        var img = string.Empty;
                                        var imageUrl = string.Empty;

                                        img = image.GetCssValue("background");
                                        if (!string.IsNullOrEmpty(img))
                                            imageUrl = RegexString.LotImageRegex.Match(img).Groups[2].Value ?? string.Empty;
                                        Console.WriteLine(imageUrl);

                                        var auctionImageRecord = await _context.tbl_Auction_Images.Where(x => x.AuctionId == auctionId).FirstOrDefaultAsync();
                                        if (auctionImageRecord != null)
                                        {
                                            auctionImageRecord.Id = auctionImageRecord.Id;
                                            auctionImageRecord.AuctionId = auctionId;
                                            auctionImageRecord.ImageUrl = imageUrl;
                                            _context.tbl_Auction_Images.Update(auctionImageRecord);
                                        }
                                        else
                                        {
                                            AuctionImages images = new()
                                            {
                                                AuctionId = auctionId,
                                                ImageUrl = imageUrl
                                            };
                                            await _context.tbl_Auction_Images.AddAsync(images);
                                        }
                                    }
                                    Console.WriteLine();
                                }
                                else
                                {
                                    Console.WriteLine($"No Images for Lot with Auction Id = {auctionId}----------");
                                    Console.WriteLine();
                                }
                                driver.Navigate().Back();
                                GetFullyLoadedWebPage(driver);
                                await _context.SaveChangesAsync();
                                Console.WriteLine();
                                Console.WriteLine("=====================================================================================================================");
                                Console.WriteLine($"Data Saved Successfully in Auctions and AuctionImages Tables with AuctionId = {auctionId}");
                                Console.WriteLine("=====================================================================================================================");
                                Console.WriteLine();
                            }
                        }
                        NextPageControl(driver, count);
                        count++;
                    } while (true);
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("Data Saved Successfully in WatchAuctions and WatchDetails Tables...!");
                    Console.WriteLine();
                }
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