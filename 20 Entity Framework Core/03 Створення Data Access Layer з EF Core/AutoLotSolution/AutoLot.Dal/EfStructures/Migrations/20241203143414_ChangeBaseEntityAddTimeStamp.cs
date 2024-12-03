using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoLot.Dal.EfStructures.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBaseEntityAddTimeStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                schema: "dbo",
                table: "Radios",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                schema: "dbo",
                table: "Orders",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                schema: "dbo",
                table: "Makes",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                schema: "dbo",
                table: "InventoryToDrivers",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                schema: "dbo",
                table: "Inventory",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "Drivers",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                schema: "dbo",
                table: "Customers",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                schema: "dbo",
                table: "CreditRisks",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeStamp",
                schema: "dbo",
                table: "Radios");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                schema: "dbo",
                table: "Makes");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                schema: "dbo",
                table: "InventoryToDrivers");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                schema: "dbo",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                schema: "dbo",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                schema: "dbo",
                table: "CreditRisks");
        }
    }
}
