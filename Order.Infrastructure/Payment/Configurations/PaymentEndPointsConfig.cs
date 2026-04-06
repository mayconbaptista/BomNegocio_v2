using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Payment.Configurations
{
    internal class PaymentEndPointsConfig
    {
        public string OauthToken { get; init; }
        public string CreatePix { get; init; }
        public string GetPix { get; init; }
        public string WebHook { get; init; }
    }
}
