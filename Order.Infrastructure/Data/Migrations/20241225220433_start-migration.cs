using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Order.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class startmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_email = table.Column<string>(type: "text", nullable: false),
                    customer_name = table.Column<string>(type: "text", nullable: false),
                    ShippingAddress_Name = table.Column<string>(type: "text", nullable: false),
                    shipping_address_street = table.Column<string>(type: "text", nullable: false),
                    shipping_address_city = table.Column<string>(type: "text", nullable: false),
                    shipping_address_state = table.Column<string>(type: "text", nullable: false),
                    shipping_address_country = table.Column<string>(type: "text", nullable: false),
                    shipping_address_zip_code = table.Column<string>(type: "text", nullable: false),
                    BillingAddress_Name = table.Column<string>(type: "text", nullable: false),
                    billing_address_street = table.Column<string>(type: "text", nullable: false),
                    billing_address_city = table.Column<string>(type: "text", nullable: false),
                    billing_address_state = table.Column<string>(type: "text", nullable: false),
                    billing_address_country = table.Column<string>(type: "text", nullable: false),
                    billing_address_zip_code = table.Column<string>(type: "text", nullable: false),
                    status_code = table.Column<int>(type: "integer", nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    OrderEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: false),
                    product_quantity = table.Column<long>(type: "bigint", nullable: false),
                    product_unit_price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => new { x.OrderEntityId, x.Id });
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderEntityId",
                        column: x => x.OrderEntityId,
                        principalTable: "Order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
