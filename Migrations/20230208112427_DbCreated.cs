using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtValorem_Crawling.Migrations
{
    public partial class DbCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Auctions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LotNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimationPriceStart = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimationPriceEnd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimationPriceCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultPriceCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaleOfDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaleOfMonth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaleOfYear = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Auctions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Auction_Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuctionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Auction_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_Auction_Images_tbl_Auctions_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "tbl_Auctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Auction_Images_AuctionId",
                table: "tbl_Auction_Images",
                column: "AuctionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Auction_Images");

            migrationBuilder.DropTable(
                name: "tbl_Auctions");
        }
    }
}
