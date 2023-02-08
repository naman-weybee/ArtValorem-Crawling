using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtValorem_Crawling.Data
{
    public class ArtValorem_DbContext : DbContext
    {
        public ArtValorem_DbContext()
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=DESKTOP-9J2CV47; database=ArtValorem; integrated security=SSPI");
        }

        public DbSet<Auctions> tbl_Auctions { get; set; }
        public DbSet<AuctionImages> tbl_Auction_Images { get; set; }
    }
}
