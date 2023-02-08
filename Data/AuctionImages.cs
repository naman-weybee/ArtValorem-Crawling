using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtValorem_Crawling.Data
{
    public class AuctionImages
    {
        [Key]
        public int Id { get; set; }
        public Auctions Auction { get; set; }
        public string AuctionId { get; set; }
        public string? ImageUrl { get; set; }
    }
}
