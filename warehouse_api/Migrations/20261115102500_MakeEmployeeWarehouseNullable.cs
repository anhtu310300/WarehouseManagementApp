using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace warehouse_api.Migrations
{
    /// <inheritdoc />
    public partial class MakeEmployeeWarehouseNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "warehouseAssignedId",
                table: "warehouse_db_empTable",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "warehouseAssignedId",
                table: "warehouse_db_empTable",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
