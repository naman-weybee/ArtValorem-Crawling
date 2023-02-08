using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtValorem_Crawling.Data
{
    public class Auctions
    {
        [Key]
        public string Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public string? LotNumber { get; set; }
        public string? EstimationPriceStart { get; set; }
        public string? EstimationPriceEnd { get; set; }
        public string? EstimationPriceCurrency { get; set; }
        public string? ResultPrice { get; set; }
        public string? ResultPriceCurrency { get; set; }
        public string? SaleOfDate { get; set; }
        public string? SaleOfMonth { get; set; }
        public string? SaleOfYear { get; set; }
    }
}
