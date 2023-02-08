using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phillips_Crawling_Task.Service
{
    public class XpathStrings
    {

        public static readonly string AcceptCoockiesXpath = "(//strong[.='Tout accepter'])[1]";
        public static readonly string NextPageXpath = "(//a[.='Suivant'])[2]";
        public static readonly string AuctionListXpath = "//div[@class='col-md-12 lot_recherche odd']|//div[@class='col-md-12 lot_recherche even']";
        public static readonly string AuctionImageUrlXpath = ".//div[@class='col-md-3'][1]/a[1]/img";
        public static readonly string LotCountXpath = ".//div[@class='col-md-1 lotnum']";
        public static readonly string AuctionTitleXpath = ".//div[@class='product-title']/h3";
        public static readonly string AuctionDescriptionXpath = ".//div[@class='col-md-5']";
        public static readonly string AuctionLinkXpath = ".//div[@class='col-md-3'][2]/a";
        public static readonly string EstimationPriceXpath = ".//div[@class='col-md-3'][2]/div[1]/div";
        public static readonly string ResultPriceXpath = ".//div[@class='col-md-3'][2]/div[1]/div[2]";
        public static readonly string SaleOfDateXpath = ".//div[@class='col-md-3'][2]/div[2]";
    }
}
