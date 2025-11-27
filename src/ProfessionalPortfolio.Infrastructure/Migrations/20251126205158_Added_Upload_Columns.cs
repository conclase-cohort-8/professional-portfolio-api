using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfessionalPortfolio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_Upload_Columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicturePublicId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResumePublicId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResumeUrl",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfilePicturePublicId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResumePublicId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResumeUrl",
                table: "Users");
        }
    }
}
