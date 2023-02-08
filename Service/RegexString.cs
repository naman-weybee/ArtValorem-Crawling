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
        public static readonly Regex TimeDurationRegex = new(@"(\d+)(\s?\,?\-?)(\w+)(\s?\,?\-?)(\d{4})(\s?\,?\-?)(\d+\:\d+\s\w+)(\s+?\,?\-?)([^\n\r])(\w+)", RegexOptions.IgnoreCase);
        public static readonly Regex SaleNumberRegex = new(@"(\d+)", RegexOptions.IgnoreCase);
        public static readonly Regex AuctionDateRegex = new(@"(\w+)(\s?\,?\-?)(\d+)(\s?\,?\-?\s?)(\w+)(\s?\,?\-?)(\d+)(\s?\,?\-?\s?)(\d{4})", RegexOptions.IgnoreCase);
        public static readonly Regex DimensionRegex = new(@"(\d+.\d+)(\s?)(\w+)(\s?\w?\s?)(\d+.\d+)(\s?)(\w+)", RegexOptions.IgnoreCase);
        public static readonly Regex AuctionIdRegex = new(@"Auction=([^\n\r]*)", RegexOptions.IgnoreCase);
        public static readonly Regex ReferenceNoRegex = new(@"Reference: (.*)<br>", RegexOptions.IgnoreCase);
        public static readonly Regex CaseMaterialRegex = new(@"Case Material: (.*)<br>", RegexOptions.IgnoreCase);
        public static readonly Regex ConditionRegex = new(@"Condition: (.*)", RegexOptions.IgnoreCase);
        public static readonly Regex CircaRegex = new(@"Circa: (.*)<br>", RegexOptions.IgnoreCase);
        public static readonly Regex WinningBidRegex = new(@"^(\w+)?(\s?)(\w+)?(\s?\:?\s?)(\D*)(\s)(\d+)(\D*)$", RegexOptions.IgnoreCase);
        public static readonly Regex EstimateRegex = new(@"^(\w+)?(\:?\s?)(\D+)(\s)(\d+)(\s?\-?\s?)(\d+)", RegexOptions.IgnoreCase);
        public static readonly Regex CaseDiameterRegex = new(@"Case Diameter: (.*)<br>", RegexOptions.IgnoreCase);
        public static readonly Regex CaseDimensionRegex = new(@"(\d+.?\d+)?(\s?)(\w+)?(\s?\w?\s?)(\d+.?\d+)?(\s?)(\w+)?", RegexOptions.IgnoreCase);
    }
}
