using BuildBlocks.Domain.ValueObjects;
using BuildInBlocks.Messaging.Dtos;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Order.Domain.Entities;
using Order.Domain.Events;
using Order.Domain.ValueObjects;
using System.Reflection;

namespace Order.Application.Dtos
{
    public static class MapsterConfig
    {
        public static IServiceCollection AddMapsterConfig (this IServiceCollection services)
        {

            TypeAdapterConfig<Address, AddressDto>
                .NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Street, src => src.Street)
                .Map(dest => dest.City, src => src.City)
                .Map(dest => dest.State, src => src.State)
                .Map(dest => dest.Country, src => src.Country)
                .Map(dest => dest.ZipCode, src => src.ZipCode);

            TypeAdapterConfig<AddressDto, Address>
                .NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Street, src => src.Street)
                .Map(dest => dest.City, src => src.City)
                .Map(dest => dest.State, src => src.State)
                .Map(dest => dest.Country, src => src.Country)
                .Map(dest => dest.ZipCode, src => src.ZipCode);

            TypeAdapterConfig<Customer, CustomerDto>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.Name, src => src.Name);


            TypeAdapterConfig<OrderEntity, OrderDto>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.ShippingAddress, src => src.ShippingAddress)
                .Map(dest => dest.BillingAddress, src => src.BillingAddress)
                .Map(dest => dest.TotalPrice, src => src.TotalPrice)
                .Map(dest => dest.Items, src => src.OrderItems);

            TypeAdapterConfig<OrderDto, OrderEntity>
                .NewConfig()
                .Ignore(dest => dest.Id)
                .Map(dest => dest.CustomerId, src => src.Customer.Id)
                .Map(dest => dest.ShippingAddress, src => src.ShippingAddress)
                .Map(dest => dest.BillingAddress, src => src.BillingAddress)
                .Map(dest => dest.TotalPrice, src => src.TotalPrice)
                .Map(dest => dest.OrderItems, src => src.Items);

            TypeAdapterConfig<OrderItemEntity, OrderItemDto>
                .NewConfig()
                .Map(dest => dest.ProductId, src => src.ProductId)
                .Map(dest => dest.UnitPrice, src => src.UnitPrice)
                .Map(dest => dest.Quantity, src => src.Quantity);

            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
