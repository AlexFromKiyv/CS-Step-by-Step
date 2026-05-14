using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoLot.Dal.EfStructures.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCreditRisk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "CreditRisks");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "CreditRisks",
                type: "nvarchar(max)",
                nullable: false,
                computedColumnSql: "[LastName] + ', ' + [FirstName]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "CreditRisks");

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "CreditRisks",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }
    }
}
