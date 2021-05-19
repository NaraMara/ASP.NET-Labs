using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend5.Migrations
{
    public partial class fixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "WardStuffs",
                newName: "WardStuffId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Diagnoses",
                newName: "DiagnosisId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Analyses",
                newName: "AnalysisId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WardStuffId",
                table: "WardStuffs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "DiagnosisId",
                table: "Diagnoses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AnalysisId",
                table: "Analyses",
                newName: "Id");
        }
    }
}
