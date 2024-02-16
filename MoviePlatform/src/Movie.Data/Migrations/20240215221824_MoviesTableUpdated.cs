using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movie.Data.Migrations
{
    public partial class MoviesTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MovieUrl",
                table: "Movies",
                newName: "Subtitle");

            migrationBuilder.AddColumn<string>(
                name: "Movie1080pUrl",
                table: "Movies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Movie480pUrl",
                table: "Movies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Movie1080pUrl",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Movie480pUrl",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "Subtitle",
                table: "Movies",
                newName: "MovieUrl");
        }
    }
}
