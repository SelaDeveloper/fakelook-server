using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fakeLook_dal.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Date", "X_Position", "Y_Position", "Z_Position" },
                values: new object[] { new DateTime(2022, 3, 14, 20, 30, 15, 147, DateTimeKind.Local).AddTicks(1519), 11.599512735349771, 42.225508895991872, 11.766425544845195 });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Date", "X_Position", "Y_Position", "Z_Position" },
                values: new object[] { new DateTime(2022, 3, 14, 20, 30, 15, 147, DateTimeKind.Local).AddTicks(1559), 29.643111670195584, 48.259647636777231, 39.45809735943768 });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Date", "X_Position", "Y_Position", "Z_Position" },
                values: new object[] { new DateTime(2022, 3, 14, 20, 30, 15, 147, DateTimeKind.Local).AddTicks(1562), 3.2915891445939369, 4.1855296659698835, 35.760467450649038 });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Date", "X_Position", "Y_Position", "Z_Position" },
                values: new object[] { new DateTime(2022, 3, 14, 20, 30, 15, 147, DateTimeKind.Local).AddTicks(1564), 13.675386869953693, 42.723796559057519, 19.518868083070519 });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Date", "X_Position", "Y_Position", "Z_Position" },
                values: new object[] { new DateTime(2022, 3, 14, 20, 30, 15, 147, DateTimeKind.Local).AddTicks(1566), 41.916357020448139, 21.093129846180599, 48.947850112492134 });

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CommentId", "UserId" },
                values: new object[] { 4, 1 });

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CommentId", "UserId" },
                values: new object[] { 10, 5 });

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CommentId", "UserId" },
                values: new object[] { 21, 3 });

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CommentId",
                value: 17);

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CommentId", "UserId" },
                values: new object[] { 2, 2 });

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 6,
                column: "UserId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 7,
                column: "CommentId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CommentId", "UserId" },
                values: new object[] { 11, 3 });

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CommentId", "UserId" },
                values: new object[] { 22, 5 });

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 10,
                column: "CommentId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FirstName", "Password", "UserName" },
                values: new object[] { "user 1", "-1395380112", "user 1" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FirstName", "Password", "UserName" },
                values: new object[] { "user 2", "-1395380112", "user 2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FirstName", "Password", "UserName" },
                values: new object[] { "user 3", "-1395380112", "user 3" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FirstName", "Password", "UserName" },
                values: new object[] { "user 4", "-1395380112", "user 4" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "FirstName", "Password", "UserName" },
                values: new object[] { "user 5", "-1395380112", "user 5" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UserId",
                table: "Users",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "Name");

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Date", "X_Position", "Y_Position", "Z_Position" },
                values: new object[] { new DateTime(2022, 3, 14, 20, 16, 28, 928, DateTimeKind.Local).AddTicks(1053), 32.268628027507127, 28.641315457770734, 0.94836982795771796 });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Date", "X_Position", "Y_Position", "Z_Position" },
                values: new object[] { new DateTime(2022, 3, 14, 20, 16, 28, 928, DateTimeKind.Local).AddTicks(1097), 14.885542286740517, 38.557027691604247, 35.436982595759154 });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Date", "X_Position", "Y_Position", "Z_Position" },
                values: new object[] { new DateTime(2022, 3, 14, 20, 16, 28, 928, DateTimeKind.Local).AddTicks(1100), 14.662577713767339, 1.2293186355973595, 22.260052726412333 });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Date", "X_Position", "Y_Position", "Z_Position" },
                values: new object[] { new DateTime(2022, 3, 14, 20, 16, 28, 928, DateTimeKind.Local).AddTicks(1102), 18.097746163731109, 17.102204989326275, 5.2814618748839361 });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Date", "X_Position", "Y_Position", "Z_Position" },
                values: new object[] { new DateTime(2022, 3, 14, 20, 16, 28, 928, DateTimeKind.Local).AddTicks(1104), 18.537984776964837, 12.354114317447507, 13.288258894215071 });

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CommentId", "UserId" },
                values: new object[] { 23, 2 });

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CommentId", "UserId" },
                values: new object[] { 14, 1 });

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CommentId", "UserId" },
                values: new object[] { 6, 5 });

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 4,
                column: "CommentId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CommentId", "UserId" },
                values: new object[] { 5, 1 });

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 6,
                column: "UserId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 7,
                column: "CommentId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CommentId", "UserId" },
                values: new object[] { 19, 4 });

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CommentId", "UserId" },
                values: new object[] { 10, 1 });

            migrationBuilder.UpdateData(
                table: "UserTaggedComments",
                keyColumn: "Id",
                keyValue: 10,
                column: "CommentId",
                value: 21);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "660951936");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "660951936");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "660951936");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "Password",
                value: "660951936");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "Password",
                value: "660951936");
        }
    }
}
