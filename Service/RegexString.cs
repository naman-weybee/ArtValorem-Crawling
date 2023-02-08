using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Phillips_Crawling_Task.Service
{
    public class RegexString
    {
        public static readonly Regex AuctionDescriptionRegex = new(@"</div>(.*)<br><br>", RegexOptions.IgnoreCase);
        public static readonly Regex EstimationPriceRegex = new(@"(\d+)(\s?\-?\s?)(\d+)?(\s?\-?\s?)(\w+)?", RegexOptions.IgnoreCase);
        public static readonly Regex ResultPriceRegex = new(@"(\d+)(\s?\-?\s?)(\w+)?", RegexOptions.IgnoreCase);
        public static readonly Regex SaleOfDateRegex = new(@"(\d+)?(\s?\-?\s?)(\d+)?(\s?\-?\s?)(\d{4})", RegexOptions.IgnoreCase);
        public static readonly Regex AuctionIdRegex = new(@"(\d+)", RegexOptions.IgnoreCase);
    }

    public enum NumberToMonth
    {
        January = 1,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December,
    }
}
