using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SemProg.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Username", "Password", "Role" },
                values: new object[,]
                {
                    { "admin", "admin", "admin" },
                    { "user",  "user",  "user"  },
                    { "pera",  "pera",  "user"  }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Users WHERE Username IN ('admin','user','pera')");
        }
    }
}