using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    ContentUrl = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    PostType = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    Upvotes = table.Column<int>(type: "int", nullable: false),
                    Downvotes = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AddedBy = table.Column<string>(type: "NVARCHAR(300)", maxLength: 300, nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(300)", maxLength: 300, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    UserEmail = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    UserBio = table.Column<string>(type: "NVARCHAR(1500)", maxLength: 1500, nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    UserType = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BlockedReason = table.Column<string>(type: "NVARCHAR(1500)", maxLength: 1500, nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "NVARCHAR(300)", maxLength: 300, nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    IsSiteUser = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AddedBy = table.Column<string>(type: "NVARCHAR(300)", maxLength: 300, nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(300)", maxLength: 300, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostComments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    Comment = table.Column<string>(type: "NVARCHAR(3500)", maxLength: 3500, nullable: false),
                    Upvotes = table.Column<int>(type: "int", nullable: false),
                    Downvotes = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AddedBy = table.Column<string>(type: "NVARCHAR(300)", maxLength: 300, nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(300)", maxLength: 300, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostComments_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostVotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    VoteType = table.Column<string>(type: "NVARCHAR(3500)", maxLength: 3500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostVotes_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_PostId",
                table: "PostComments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostVotes_PostId",
                table: "PostVotes",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostComments");

            migrationBuilder.DropTable(
                name: "PostVotes");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Post");
        }
    }
}
