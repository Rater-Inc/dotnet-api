using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Rater.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nickname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "spaces",
                columns: table => new
                {
                    space_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    creator_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_locked = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    link = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("spaces_pkey", x => x.space_id);
                    table.ForeignKey(
                        name: "spaces_creator_id_fkey",
                        column: x => x.creator_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "metrics",
                columns: table => new
                {
                    metric_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    space_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("metrics_pkey", x => x.metric_id);
                    table.ForeignKey(
                        name: "metrics_space_id_fkey",
                        column: x => x.space_id,
                        principalTable: "spaces",
                        principalColumn: "space_id");
                });

            migrationBuilder.CreateTable(
                name: "participants",
                columns: table => new
                {
                    participant_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    space_id = table.Column<int>(type: "integer", nullable: false),
                    participant_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("participants_pkey", x => x.participant_id);
                    table.ForeignKey(
                        name: "participants_space_id_fkey",
                        column: x => x.space_id,
                        principalTable: "spaces",
                        principalColumn: "space_id");
                });

            migrationBuilder.CreateTable(
                name: "ratings",
                columns: table => new
                {
                    rating_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rater_id = table.Column<int>(type: "integer", nullable: false),
                    ratee_id = table.Column<int>(type: "integer", nullable: false),
                    space_id = table.Column<int>(type: "integer", nullable: false),
                    metric_id = table.Column<int>(type: "integer", nullable: false),
                    score = table.Column<int>(type: "integer", nullable: false),
                    rated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ratings_pkey", x => x.rating_id);
                    table.ForeignKey(
                        name: "ratings_metric_id_fkey",
                        column: x => x.metric_id,
                        principalTable: "metrics",
                        principalColumn: "metric_id");
                    table.ForeignKey(
                        name: "ratings_ratee_id_fkey",
                        column: x => x.ratee_id,
                        principalTable: "participants",
                        principalColumn: "participant_id");
                    table.ForeignKey(
                        name: "ratings_rater_id_fkey",
                        column: x => x.rater_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "ratings_space_id_fkey",
                        column: x => x.space_id,
                        principalTable: "spaces",
                        principalColumn: "space_id");
                });

            migrationBuilder.CreateIndex(
                name: "metrics_space_id_name_key",
                table: "metrics",
                columns: new[] { "space_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "participants_space_id_participant_name_key",
                table: "participants",
                columns: new[] { "space_id", "participant_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ratings_metric_id",
                table: "ratings",
                column: "metric_id");

            migrationBuilder.CreateIndex(
                name: "IX_ratings_ratee_id",
                table: "ratings",
                column: "ratee_id");

            migrationBuilder.CreateIndex(
                name: "IX_ratings_space_id",
                table: "ratings",
                column: "space_id");

            migrationBuilder.CreateIndex(
                name: "ratings_rater_id_ratee_id_space_id_metric_id_key",
                table: "ratings",
                columns: new[] { "rater_id", "ratee_id", "space_id", "metric_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_spaces_creator_id",
                table: "spaces",
                column: "creator_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ratings");

            migrationBuilder.DropTable(
                name: "metrics");

            migrationBuilder.DropTable(
                name: "participants");

            migrationBuilder.DropTable(
                name: "spaces");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
