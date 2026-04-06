using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class efimigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    CostumerIdentifier = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    billing_address_city = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    billing_address_country = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    billing_address_name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    billing_address_state = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    billing_address_street = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    billing_address_zip_code = table.Column<string>(type: "character(8)", fixedLength: true, maxLength: 8, nullable: false),
                    delivery_estimatedDeliveryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    delivery_price = table.Column<decimal>(type: "numeric", nullable: false),
                    delivery_type = table.Column<string>(type: "text", nullable: false),
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
                name: "Order_item",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<long>(type: "bigint", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_item", x => new { x.order_id, x.product_id });
                    table.ForeignKey(
                        name: "FK_Order_OrderItem",
                        column: x => x.order_id,
                        principalTable: "Order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    px_id = table.Column<string>(type: "text", nullable: false),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    method = table.Column<string>(type: "text", nullable: false),
                    expire_in = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    pix_copy_and_paste = table.Column<string>(type: "text", nullable: false),
                    location = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_payment",
                        column: x => x.order_id,
                        principalTable: "Order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_order_id",
                table: "Payment",
                column: "order_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order_item");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
