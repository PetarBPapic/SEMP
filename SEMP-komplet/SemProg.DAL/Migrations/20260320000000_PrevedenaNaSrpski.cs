using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SemProg.DAL.Migrations
{
    public partial class PrevedenaNaSrpski : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Episodes_Users_CreatedBy", table: "Episodes");
            migrationBuilder.DropForeignKey(name: "FK_Ratings_Episodes_EpisodeId", table: "Ratings");
            migrationBuilder.DropForeignKey(name: "FK_Ratings_Users_UserId", table: "Ratings");
            migrationBuilder.DropIndex(name: "IX_Episodes_CreatedBy", table: "Episodes");
            migrationBuilder.DropIndex(name: "IX_Ratings_EpisodeId", table: "Ratings");
            migrationBuilder.DropIndex(name: "IX_Ratings_UserId", table: "Ratings");
            migrationBuilder.DropIndex(name: "IX_Users_Username", table: "Users");

            migrationBuilder.RenameColumn(name: "Username", table: "Users", newName: "KorisnickoIme");
            migrationBuilder.RenameColumn(name: "Password", table: "Users", newName: "Lozinka");
            migrationBuilder.RenameColumn(name: "Role", table: "Users", newName: "Uloga");
            migrationBuilder.RenameColumn(name: "Title", table: "Episodes", newName: "Naslov");
            migrationBuilder.RenameColumn(name: "Description", table: "Episodes", newName: "Opis");
            migrationBuilder.RenameColumn(name: "ReleaseDate", table: "Episodes", newName: "DatumPremijere");
            migrationBuilder.RenameColumn(name: "CreatedBy", table: "Episodes", newName: "KreiraoId");
            migrationBuilder.RenameColumn(name: "Score", table: "Ratings", newName: "Vrednost");
            migrationBuilder.RenameColumn(name: "Comment", table: "Ratings", newName: "Komentar");
            migrationBuilder.RenameColumn(name: "RatedAt", table: "Ratings", newName: "OcenjeneNa");
            migrationBuilder.RenameColumn(name: "EpisodeId", table: "Ratings", newName: "EpizodaId");
            migrationBuilder.RenameColumn(name: "UserId", table: "Ratings", newName: "KorisnikId");

            migrationBuilder.RenameTable(name: "Users", newName: "Korisnici");
            migrationBuilder.RenameTable(name: "Episodes", newName: "Epizode");
            migrationBuilder.RenameTable(name: "Ratings", newName: "Ocene");

            migrationBuilder.CreateIndex(name: "IX_Korisnici_KorisnickoIme", table: "Korisnici", column: "KorisnickoIme", unique: true);
            migrationBuilder.CreateIndex(name: "IX_Epizode_KreiraoId", table: "Epizode", column: "KreiraoId");
            migrationBuilder.CreateIndex(name: "IX_Ocene_EpizodaId", table: "Ocene", column: "EpizodaId");
            migrationBuilder.CreateIndex(name: "IX_Ocene_KorisnikId", table: "Ocene", column: "KorisnikId");

            migrationBuilder.AddForeignKey(name: "FK_Epizode_Korisnici_KreiraoId", table: "Epizode", column: "KreiraoId", principalTable: "Korisnici", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(name: "FK_Ocene_Epizode_EpizodaId", table: "Ocene", column: "EpizodaId", principalTable: "Epizode", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(name: "FK_Ocene_Korisnici_KorisnikId", table: "Ocene", column: "KorisnikId", principalTable: "Korisnici", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Epizode_Korisnici_KreiraoId", table: "Epizode");
            migrationBuilder.DropForeignKey(name: "FK_Ocene_Epizode_EpizodaId", table: "Ocene");
            migrationBuilder.DropForeignKey(name: "FK_Ocene_Korisnici_KorisnikId", table: "Ocene");
            migrationBuilder.DropIndex(name: "IX_Korisnici_KorisnickoIme", table: "Korisnici");
            migrationBuilder.DropIndex(name: "IX_Epizode_KreiraoId", table: "Epizode");
            migrationBuilder.DropIndex(name: "IX_Ocene_EpizodaId", table: "Ocene");
            migrationBuilder.DropIndex(name: "IX_Ocene_KorisnikId", table: "Ocene");

            migrationBuilder.RenameTable(name: "Korisnici", newName: "Users");
            migrationBuilder.RenameTable(name: "Epizode", newName: "Episodes");
            migrationBuilder.RenameTable(name: "Ocene", newName: "Ratings");

            migrationBuilder.RenameColumn(name: "KorisnickoIme", table: "Users", newName: "Username");
            migrationBuilder.RenameColumn(name: "Lozinka", table: "Users", newName: "Password");
            migrationBuilder.RenameColumn(name: "Uloga", table: "Users", newName: "Role");
            migrationBuilder.RenameColumn(name: "Naslov", table: "Episodes", newName: "Title");
            migrationBuilder.RenameColumn(name: "Opis", table: "Episodes", newName: "Description");
            migrationBuilder.RenameColumn(name: "DatumPremijere", table: "Episodes", newName: "ReleaseDate");
            migrationBuilder.RenameColumn(name: "KreiraoId", table: "Episodes", newName: "CreatedBy");
            migrationBuilder.RenameColumn(name: "Vrednost", table: "Ratings", newName: "Score");
            migrationBuilder.RenameColumn(name: "Komentar", table: "Ratings", newName: "Comment");
            migrationBuilder.RenameColumn(name: "OcenjeneNa", table: "Ratings", newName: "RatedAt");
            migrationBuilder.RenameColumn(name: "EpizodaId", table: "Ratings", newName: "EpisodeId");
            migrationBuilder.RenameColumn(name: "KorisnikId", table: "Ratings", newName: "UserId");

            migrationBuilder.CreateIndex(name: "IX_Users_Username", table: "Users", column: "Username", unique: true);
            migrationBuilder.CreateIndex(name: "IX_Episodes_CreatedBy", table: "Episodes", column: "CreatedBy");
            migrationBuilder.CreateIndex(name: "IX_Ratings_EpisodeId", table: "Ratings", column: "EpisodeId");
            migrationBuilder.CreateIndex(name: "IX_Ratings_UserId", table: "Ratings", column: "UserId");

            migrationBuilder.AddForeignKey(name: "FK_Episodes_Users_CreatedBy", table: "Episodes", column: "CreatedBy", principalTable: "Users", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(name: "FK_Ratings_Episodes_EpisodeId", table: "Ratings", column: "EpisodeId", principalTable: "Episodes", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(name: "FK_Ratings_Users_UserId", table: "Ratings", column: "UserId", principalTable: "Users", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
        }
    }
}
