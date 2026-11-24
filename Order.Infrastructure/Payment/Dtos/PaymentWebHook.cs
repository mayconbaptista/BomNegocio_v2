using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Payment.Dtos
{
    internal record PaymentWebHook
    {
        public string WebhookUrl { get; init; } = "https://noncondensible-atonally-ryker.ngrok-free.dev/api/Webhook/notify";
    }
}
