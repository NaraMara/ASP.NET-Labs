using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend5.Migrations
{
    public partial class PlacementsAddHospitalIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HospitalId",
                table: "Placements",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Placements_HospitalId",
                table: "Placements",
                column: "HospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Placements_Hospitals_HospitalId",
                table: "Placements",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Placements_Hospitals_HospitalId",
                table: "Placements");

            migrationBuilder.DropIndex(
                name: "IX_Placements_HospitalId",
                table: "Placements");

            migrationBuilder.DropColumn(
                name: "HospitalId",
                table: "Placements");
        }
    }
}
