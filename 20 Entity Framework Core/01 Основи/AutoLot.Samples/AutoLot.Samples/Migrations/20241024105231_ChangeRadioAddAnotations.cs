using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoLot.Samples.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRadioAddAnotations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Radios_Inventory_CarId",
                table: "Radios");

            migrationBuilder.RenameColumn(
                name: "CarId",
                table: "Radios",
                newName: "InventoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Radios_CarId",
                table: "Radios",
                newName: "IX_Radios_InventoryId");

            migrationBuilder.AlterColumn<string>(
                name: "RadioId",
                table: "Radios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Radios_Inventory_InventoryId",
                table: "Radios",
                column: "InventoryId",
                principalSchema: "dbo",
                principalTable: "Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Radios_Inventory_InventoryId",
                table: "Radios");

            migrationBuilder.RenameColumn(
                name: "InventoryId",
                table: "Radios",
                newName: "CarId");

            migrationBuilder.RenameIndex(
                name: "IX_Radios_InventoryId",
                table: "Radios",
                newName: "IX_Radios_CarId");

            migrationBuilder.AlterColumn<string>(
                name: "RadioId",
                table: "Radios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_Radios_Inventory_CarId",
                table: "Radios",
                column: "CarId",
                principalSchema: "dbo",
                principalTable: "Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
