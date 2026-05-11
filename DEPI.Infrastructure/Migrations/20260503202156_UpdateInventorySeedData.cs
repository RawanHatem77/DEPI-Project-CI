using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DEPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInventorySeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MedicineInventories",
                columns: new[] { "Id", "ExpectedArrival", "HospitalId", "MedicineId", "Quantity" },
                values: new object[,]
                {
                    { 11, null, 2, 4, 80 },
                    { 12, null, 2, 5, 40 },
                    { 13, null, 3, 10, 50 },
                    { 14, null, 4, 6, 90 },
                    { 15, null, 4, 7, 120 },
                    { 16, null, 6, 3, 150 },
                    { 17, null, 6, 5, 60 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MedicineInventories",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "MedicineInventories",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "MedicineInventories",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "MedicineInventories",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "MedicineInventories",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "MedicineInventories",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "MedicineInventories",
                keyColumn: "Id",
                keyValue: 17);
        }
    }
}
