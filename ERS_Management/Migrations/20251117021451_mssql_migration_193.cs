using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERS_Management.Migrations
{
    /// <inheritdoc />
    public partial class mssql_migration_193 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FaultId",
                table: "EntryLog",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaultId",
                table: "EntryLog");
        }
    }
}
