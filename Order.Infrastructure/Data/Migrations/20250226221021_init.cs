using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    status_code = table.Column<int>(type: "integer", nullable: false),
                    billing_address_city = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    billing_address_country = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    billing_address_name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    billing_address_state = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    billing_address_street = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    billing_address_zip_code = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    shipping_address_city = table.Column<string>(type: "text", nullable: false),
                    shipping_address_country = table.Column<string>(type: "text", nullable: false),
                    shipping_address_name = table.Column<string>(type: "text", nullable: false),
                    shipping_address_state = table.Column<string>(type: "text", nullable: false),
                    shipping_address_street = table.Column<string>(type: "text", nullable: false),
                    shipping_address_zip_code = table.Column<string>(type: "text", nullable: false),
                    create_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_item",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<long>(type: "bigint", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_item", x => new { x.order_id, x.product_id });
                    table.ForeignKey(
                        name: "FK_Order_OrderItem",
                        column: x => x.order_id,
                        principalTable: "Order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_item");

            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
