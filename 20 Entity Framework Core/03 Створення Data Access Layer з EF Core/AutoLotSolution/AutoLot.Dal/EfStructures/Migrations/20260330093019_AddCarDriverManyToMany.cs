using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoLot.Dal.EfStructures.Migrations
{
    /// <inheritdoc />
    public partial class AddCarDriverManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryToDrivers_Drivers_DriverId",
                schema: "dbo",
                table: "InventoryToDrivers");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryToDrivers_Inventory_InventoryId",
                schema: "dbo",
                table: "InventoryToDrivers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryToDrivers",
                schema: "dbo",
                table: "InventoryToDrivers")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "InventoryToDriversAudit")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "dbo")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropIndex(
                name: "IX_InventoryToDrivers_InventoryId",
                schema: "dbo",
                table: "InventoryToDrivers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryToDrivers",
                schema: "dbo",
                table: "InventoryToDrivers",
                columns: new[] { "InventoryId", "DriverId" });

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryDriver_Drivers_DriverId",
                schema: "dbo",
                table: "InventoryToDrivers",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryDriver_Inventory_InventoryId",
                schema: "dbo",
                table: "InventoryToDrivers",
                column: "InventoryId",
                principalSchema: "dbo",
                principalTable: "Inventory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryDriver_Drivers_DriverId",
                schema: "dbo",
                table: "InventoryToDrivers");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryDriver_Inventory_InventoryId",
                schema: "dbo",
                table: "InventoryToDrivers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryToDrivers",
                schema: "dbo",
                table: "InventoryToDrivers")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "InventoryToDriversAudit")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "dbo")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryToDrivers",
                schema: "dbo",
                table: "InventoryToDrivers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryToDrivers_InventoryId",
                schema: "dbo",
                table: "InventoryToDrivers",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryToDrivers_Drivers_DriverId",
                schema: "dbo",
                table: "InventoryToDrivers",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryToDrivers_Inventory_InventoryId",
                schema: "dbo",
                table: "InventoryToDrivers",
                column: "InventoryId",
                principalSchema: "dbo",
                principalTable: "Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
