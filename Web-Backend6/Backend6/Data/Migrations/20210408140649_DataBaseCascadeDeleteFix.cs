using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend6.Data.Migrations
{
    public partial class DataBaseCascadeDeleteFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_forumMessageAttachments_ForumMessages_MessageId",
                table: "forumMessageAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessages_AspNetUsers_CreatorId",
                table: "ForumMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessages_ForumTopics_TopicId",
                table: "ForumMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumTopics_AspNetUsers_CreatorId",
                table: "ForumTopics");

            migrationBuilder.AlterColumn<Guid>(
                name: "TopicId",
                table: "ForumMessages",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MessageId",
                table: "forumMessageAttachments",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_forumMessageAttachments_ForumMessages_MessageId",
                table: "forumMessageAttachments",
                column: "MessageId",
                principalTable: "ForumMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessages_AspNetUsers_CreatorId",
                table: "ForumMessages",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessages_ForumTopics_TopicId",
                table: "ForumMessages",
                column: "TopicId",
                principalTable: "ForumTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumTopics_AspNetUsers_CreatorId",
                table: "ForumTopics",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_forumMessageAttachments_ForumMessages_MessageId",
                table: "forumMessageAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessages_AspNetUsers_CreatorId",
                table: "ForumMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessages_ForumTopics_TopicId",
                table: "ForumMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumTopics_AspNetUsers_CreatorId",
                table: "ForumTopics");

            migrationBuilder.AlterColumn<Guid>(
                name: "TopicId",
                table: "ForumMessages",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "MessageId",
                table: "forumMessageAttachments",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_forumMessageAttachments_ForumMessages_MessageId",
                table: "forumMessageAttachments",
                column: "MessageId",
                principalTable: "ForumMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessages_AspNetUsers_CreatorId",
                table: "ForumMessages",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessages_ForumTopics_TopicId",
                table: "ForumMessages",
                column: "TopicId",
                principalTable: "ForumTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumTopics_AspNetUsers_CreatorId",
                table: "ForumTopics",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
