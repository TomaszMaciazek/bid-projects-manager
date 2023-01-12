using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BidProjectsManager.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class FixedTypoInProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BidEstiamtedOperationEnd",
                table: "Projects",
                newName: "BidEstimatedOperationEnd");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BidEstimatedOperationEnd",
                table: "Projects",
                newName: "BidEstiamtedOperationEnd");
        }
    }
}
