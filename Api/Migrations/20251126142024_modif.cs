using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class modif : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Animaux",
                schema: "public",
                newName: "t_e_animal_ani",
                newSchema: "public");

            migrationBuilder.RenameColumn(
                name: "Taille",
                schema: "public",
                table: "t_e_animal_ani",
                newName: "ani_taille");

            migrationBuilder.RenameColumn(
                name: "Species",
                schema: "public",
                table: "t_e_animal_ani",
                newName: "ani_species");

            migrationBuilder.RenameColumn(
                name: "Poids",
                schema: "public",
                table: "t_e_animal_ani",
                newName: "ani_poids");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "public",
                table: "t_e_animal_ani",
                newName: "ani_name");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "public",
                table: "t_e_animal_ani",
                newName: "ani_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "t_e_animal_ani",
                schema: "public",
                newName: "Animaux",
                newSchema: "public");

            migrationBuilder.RenameColumn(
                name: "ani_taille",
                schema: "public",
                table: "Animaux",
                newName: "Taille");

            migrationBuilder.RenameColumn(
                name: "ani_species",
                schema: "public",
                table: "Animaux",
                newName: "Species");

            migrationBuilder.RenameColumn(
                name: "ani_poids",
                schema: "public",
                table: "Animaux",
                newName: "Poids");

            migrationBuilder.RenameColumn(
                name: "ani_name",
                schema: "public",
                table: "Animaux",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ani_id",
                schema: "public",
                table: "Animaux",
                newName: "Id");
        }
    }
}
