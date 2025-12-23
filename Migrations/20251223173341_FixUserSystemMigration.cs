using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    /// <inheritdoc />
    public partial class FixUserSystemMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS Admins;");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BlogPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS Users (
                    Id int NOT NULL AUTO_INCREMENT,
                    Username varchar(50) CHARACTER SET utf8mb4 NOT NULL,
                    Email varchar(200) CHARACTER SET utf8mb4 NOT NULL,
                    PasswordHash varchar(255) CHARACTER SET utf8mb4 NOT NULL,
                    CreatedDate datetime(6) NOT NULL,
                    CONSTRAINT PK_Users PRIMARY KEY (Id),
                    UNIQUE KEY IX_Users_Email (Email),
                    UNIQUE KEY IX_Users_Username (Username)
                ) CHARACTER SET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                SET @index_exists = (SELECT COUNT(*) FROM information_schema.STATISTICS 
                    WHERE table_schema = 'railway' AND table_name = 'BlogPosts' AND index_name = 'IX_BlogPosts_UserId');
                SET @sql = IF(@index_exists = 0, 
                    'CREATE INDEX IX_BlogPosts_UserId ON BlogPosts(UserId)', 
                    'SELECT 1');
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @fk_exists = (SELECT COUNT(*) FROM information_schema.KEY_COLUMN_USAGE 
                    WHERE table_schema = 'railway' AND table_name = 'BlogPosts' AND constraint_name = 'FK_BlogPosts_Users_UserId');
                SET @sql = IF(@fk_exists = 0, 
                    'ALTER TABLE BlogPosts ADD CONSTRAINT FK_BlogPosts_Users_UserId FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE', 
                    'SELECT 1');
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_Users_UserId",
                table: "BlogPosts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_UserId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BlogPosts");

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PasswordHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
