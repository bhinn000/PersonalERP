using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalERP.Migrations
{
    /// <inheritdoc />
    public partial class AddedDateTimeinArtPiece : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ArtPieces",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ArtPieces",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ArtPieces",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ArtPieces",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "ArtPieces",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "ArtPieces",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ArtPieces");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ArtPieces");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ArtPieces");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ArtPieces");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "ArtPieces");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "ArtPieces");
        }
    }
}
