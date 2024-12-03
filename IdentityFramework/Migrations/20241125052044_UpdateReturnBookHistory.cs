using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityFramework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReturnBookHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BorrowingRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BorrowingRecords");
        }
    }
}
