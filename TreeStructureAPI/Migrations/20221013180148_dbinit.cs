using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TreeStructureAPI.Migrations
{
    public partial class dbinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "nodes",
                columns: table => new
                {
                    nodeid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    parentid = table.Column<int>(type: "integer", nullable: true),
                    depth = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nodes", x => x.nodeid);
                    table.ForeignKey(
                        name: "FK_nodes_nodes_parentid",
                        column: x => x.parentid,
                        principalTable: "nodes",
                        principalColumn: "nodeid",
                        onDelete: ReferentialAction.Cascade
                        );
                });

            migrationBuilder.InsertData(
                table: "nodes",
                columns: new[] { "nodeid", "depth", "name", "parentid" },
                values: new object[] { 1, 0, "Root", null });

            migrationBuilder.InsertData(
                table: "nodes",
                columns: new[] { "nodeid", "depth", "name", "parentid" },
                values: new object[,]
                {
                    { 2, 1, "A1", 1 },
                    { 3, 1, "A2", 1 }
                });

            migrationBuilder.InsertData(
                table: "nodes",
                columns: new[] { "nodeid", "depth", "name", "parentid" },
                values: new object[,]
                {
                    { 4, 2, "B1", 2 },
                    { 5, 2, "B2", 2 }
                });

            migrationBuilder.InsertData(
                table: "nodes",
                columns: new[] { "nodeid", "depth", "name", "parentid" },
                values: new object[,]
                {
                    { 6, 3, "C1", 4 },
                    { 7, 3, "C2", 4 },
                    { 8, 3, "C3", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_nodes_parentid",
                table: "nodes",
                column: "parentid");

            var recursiveFunction =
            @"CREATE FUNCTION getnodes(targetid integer, max integer)
                RETURNS TABLE(nodeid integer, name character varying(30), parentid integer, depth integer)
                LANGUAGE SQL
                AS $$
                WITH RECURSIVE nodesrec(nodeid, name, parentid, depth) AS(
                   SELECT nodeid, name, parentid, 0 as depth
                   FROM nodes
                   WHERE nodes.nodeid = targetid
                UNION
                    SELECT e.nodeid, e.name, e.parentid, o.depth + 1
                    FROM nodes e
                    JOIN nodesrec o ON o.nodeid = e.parentid
                    WHERE o.depth<max
                )
                SELECT * FROM nodesrec
                $$;";
            migrationBuilder.Sql(recursiveFunction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "nodes");
        }
    }
}
