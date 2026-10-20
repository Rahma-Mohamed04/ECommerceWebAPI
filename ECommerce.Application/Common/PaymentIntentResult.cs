using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common
{
    public class PaymentIntentResult
    {
        public PaymentIntentResult(string paymentIntentId, string clientSecret)
        {
            PaymentIntentId = paymentIntentId;
            ClientSecret = clientSecret;
        }

        public string PaymentIntentId { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;
    }
}
