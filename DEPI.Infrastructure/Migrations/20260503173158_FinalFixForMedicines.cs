using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DEPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinalFixForMedicines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HospitalId",
                table: "Medicines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Medicines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HospitalId", "Price", "Quantity" },
                values: new object[] { 1, 150.0m, 100 });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HospitalId", "Price", "Quantity" },
                values: new object[] { 5, 450.0m, 50 });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "HospitalId", "Price", "Quantity" },
                values: new object[] { 1, 60.0m, 200 });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "HospitalId", "Price", "Quantity" },
                values: new object[] { 2, 180.0m, 80 });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "HospitalId", "Price", "Quantity" },
                values: new object[] { 1, 210.0m, 90 });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "HospitalId", "Price", "Quantity" },
                values: new object[] { 7, 90.0m, 110 });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "HospitalId", "Price", "Quantity" },
                values: new object[] { 1, 45.0m, 150 });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "HospitalId", "Price", "Quantity" },
                values: new object[] { 7, 120.0m, 70 });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "HospitalId", "Price", "Quantity" },
                values: new object[] { 3, 30.0m, 130 });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "HospitalId", "Price", "Quantity" },
                values: new object[] { 3, 40.0m, 100 });

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "HospitalId", "Price", "Quantity" },
                values: new object[] { 3, 300.0m, 40 });

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_HospitalId",
                table: "Medicines",
                column: "HospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_Hospitals_HospitalId",
                table: "Medicines",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_Hospitals_HospitalId",
                table: "Medicines");

            migrationBuilder.DropIndex(
                name: "IX_Medicines_HospitalId",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "HospitalId",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Medicines");

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 1,
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 2,
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 3,
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 4,
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 5,
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 6,
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 7,
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 8,
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 9,
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 10,
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 11,
                column: "Quantity",
                value: 0);
        }
    }
}
