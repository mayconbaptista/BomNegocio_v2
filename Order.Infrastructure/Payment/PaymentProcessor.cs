using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Order.Application.Common.Interfaces;
using Order.Domain.Entities;
using Order.Domain.Enums;
using Order.Infrastructure.Payment.Configurations;
using Order.Infrastructure.Payment.Dtos;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Order.Infrastructure.Payment
{
    internal class PaymentProcessor (HttpClient paymentHttpClient, IOptions<PaymentConfig> paymentCredentials, ILogger<PaymentProcessor> logger) 
        : IPaymentProcessor
    {
        private readonly HttpClient _paymentHttpClient = paymentHttpClient;
        private readonly PaymentConfig _paymentConfig = paymentCredentials.Value;

        public async Task<PaymentEntity> ProcessPixPayment(string document, string name, decimal price)
        {
            var paymentPixDto = new PaymentPixDto
            {
                Calendario = new CalendarioDto
                {
                    Expiracao = (uint)TimeSpan.FromMinutes(10).TotalSeconds
                },
                Devedor = new DevedorDto
                {
                    Cpf = document,
                    Nome = name
                },
                Valor = new ValorDto
                {
                    Original = price.ToString(format: "F2")
                },
                Chave = Guid.NewGuid(),
                SolicitacaoPagador = $"Compra feita pelo {name} com documento {document} no valor de R${price:F2}"
            };

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var requestJson = JsonSerializer.Serialize(paymentPixDto, options);

            using StringContent stringContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await this._paymentHttpClient.PostAsync(this._paymentConfig.EndPoints.CreatePix, stringContent);

            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();

            var paymentResponse = JsonSerializer.Deserialize<PaymentPixDto>(responseContent);

            return PaymentEntity.Pix(
                paymentResponse!.TxId!,
                paymentResponse.Chave,
                paymentResponse.Calendario.Criacao ?? DateTime.Now,
                TimeSpan.FromMinutes(paymentResponse.Calendario.Expiracao),
                paymentResponse.PixCopiaECola!,
                paymentResponse.Location!
            );
        }

        public Task CadastreInWebHook(string key)
        {
            throw new NotImplementedException();
        }
    }
}
