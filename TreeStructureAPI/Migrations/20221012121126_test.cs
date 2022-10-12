using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TreeStructureAPI.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    NodeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    Depth = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.NodeId);
                    table.ForeignKey(
                        name: "FK_Nodes_Nodes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Nodes",
                        principalColumn: "NodeId");
                });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "NodeId", "Depth", "Name", "ParentId" },
                values: new object[] { 1, 0, "Root", null });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "NodeId", "Depth", "Name", "ParentId" },
                values: new object[,]
                {
                    { 2, 1, "A1", 1 },
                    { 3, 1, "A2", 1 }
                });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "NodeId", "Depth", "Name", "ParentId" },
                values: new object[,]
                {
                    { 4, 2, "B1", 2 },
                    { 5, 2, "B2", 2 }
                });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "NodeId", "Depth", "Name", "ParentId" },
                values: new object[,]
                {
                    { 6, 3, "C1", 4 },
                    { 7, 3, "C2", 4 },
                    { 8, 3, "C3", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_ParentId",
                table: "Nodes",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nodes");
        }
    }
}
