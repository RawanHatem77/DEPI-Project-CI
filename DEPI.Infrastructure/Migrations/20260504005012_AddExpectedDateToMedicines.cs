using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DEPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExpectedDateToMedicines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // إحنا وقفنا السطر ده عشان العمود موجود فعلاً في MonsterASP
            /*
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedDate",
                table: "Medicines",
                type: "datetime2",
                nullable: true);
            */

            // ووقفنا تحديث البيانات عشان ميعملش Error لو العمود مش متشاف لسه
            /*
            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 1,
                column: "ExpectedDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: 2,
                column: "ExpectedDate",
                value: null);
            
            // ... وهكذا لباقي الـ 11 دواء
            */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // برضه هنوقف المسح عشان لو حبينا نرجع ميمسحش العمود اللي إنتي ضفتيه يدوي
            /*
            migrationBuilder.DropColumn(
                name: "ExpectedDate",
                table: "Medicines");
            */
        }
    }
}