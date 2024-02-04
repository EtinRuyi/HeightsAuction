using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeightsAuction.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemId1",
                table: "BiddingRooms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BiddingRooms_ItemId1",
                table: "BiddingRooms",
                column: "ItemId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BiddingRooms_Items_ItemId1",
                table: "BiddingRooms",
                column: "ItemId1",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BiddingRooms_Items_ItemId1",
                table: "BiddingRooms");

            migrationBuilder.DropIndex(
                name: "IX_BiddingRooms_ItemId1",
                table: "BiddingRooms");

            migrationBuilder.DropColumn(
                name: "ItemId1",
                table: "BiddingRooms");
        }
    }
}
