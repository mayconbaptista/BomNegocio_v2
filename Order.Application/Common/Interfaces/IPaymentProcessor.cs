
using Order.Domain.Entities;
using Order.Domain.Enums;
using Order.Domain.ValueObjects;

namespace Order.Application.Common.Interfaces
{
    public interface IPaymentProcessor
    {
        public Task<PaymentEntity> ProcessPixPayment(string nome, string documento, decimal valor);
    }
}
