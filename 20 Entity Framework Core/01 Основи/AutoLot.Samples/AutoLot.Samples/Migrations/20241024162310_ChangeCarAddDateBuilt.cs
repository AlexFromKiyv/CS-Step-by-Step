using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoLot.Samples.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCarAddDateBuilt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateBuilt",
                schema: "dbo",
                table: "Inventory",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "getdate()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateBuilt",
                schema: "dbo",
                table: "Inventory");
        }
    }
}
