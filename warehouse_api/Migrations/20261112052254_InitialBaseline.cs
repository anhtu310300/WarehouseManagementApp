using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace warehouse_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialBaseline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "warehouse_db_bookingTable",
                columns: table => new
                {
                    bookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customerId = table.Column<int>(type: "int", nullable: false),
                    warehouseId = table.Column<int>(type: "int", nullable: false),
                    spaceId = table.Column<int>(type: "int", nullable: true),
                    bookingFromDate = table.Column<DateTime>(type: "date", nullable: false),
                    bookingTillDate = table.Column<DateTime>(type: "date", nullable: false),
                    bookingDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    sqftAreaNeeded = table.Column<int>(type: "int", nullable: false),
                    totalAmount = table.Column<double>(type: "float", nullable: false),
                    bookingDoneBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouse_db_bookingTable", x => x.bookingId);
                });

            migrationBuilder.CreateTable(
                name: "warehouse_db_customerMaster",
                columns: table => new
                {
                    customerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    custName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gstNo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouse_db_customerMaster", x => x.customerId);
                });

            migrationBuilder.CreateTable(
                name: "warehouse_db_empTable",
                columns: table => new
                {
                    empId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    empName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    warehouseAssignedId = table.Column<int>(type: "int", nullable: false),
                    mobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    passwordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouse_db_empTable", x => x.empId);
                });

            migrationBuilder.CreateTable(
                name: "warehouse_db_paymentMaster",
                columns: table => new
                {
                    paymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bookingId = table.Column<int>(type: "int", nullable: false),
                    paymendDate = table.Column<DateTime>(type: "date", nullable: false),
                    paymentAmount = table.Column<double>(type: "float", nullable: false),
                    payementMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paymentNaration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    receivedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouse_db_paymentMaster", x => x.paymentId);
                });

            migrationBuilder.CreateTable(
                name: "warehouse_db_spaceMaster",
                columns: table => new
                {
                    SpaceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    warehouseId = table.Column<int>(type: "int", nullable: false),
                    spaceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    areaSqft = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouse_db_spaceMaster", x => x.SpaceId);
                });

            migrationBuilder.CreateTable(
                name: "warehouse_db_warehousMaster",
                columns: table => new
                {
                    warehouseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    totalArea = table.Column<int>(type: "int", nullable: false),
                    isColdStorageAvailable = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouse_db_warehousMaster", x => x.warehouseId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "warehouse_db_bookingTable");

            migrationBuilder.DropTable(
                name: "warehouse_db_customerMaster");

            migrationBuilder.DropTable(
                name: "warehouse_db_empTable");

            migrationBuilder.DropTable(
                name: "warehouse_db_paymentMaster");

            migrationBuilder.DropTable(
                name: "warehouse_db_spaceMaster");

            migrationBuilder.DropTable(
                name: "warehouse_db_warehousMaster");
        }
    }
}
