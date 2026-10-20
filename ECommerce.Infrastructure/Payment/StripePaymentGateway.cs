using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Payment
{
    public class StripePaymentGateway : IPaymentGateway
    {
        private readonly PaymentGatewaySettings payment;
        private readonly PaymentIntentService _paymentIntentService = new();

        public StripePaymentGateway(IOptions<PaymentGatewaySettings> options)
        {
            payment = options.Value;
            /*
             السطر القادم يربط السيكرت كي
            المكان الصحيح لوضعه هو في بداية الدالة وقبل استدعاء أي كود يخص 
            Stripe  
            وتحديداً قبل السطر الذي يُرسل الطلب
             _paymentIntentService.CreateAsync.
             */
            StripeConfiguration.ApiKey = payment.SecretKey;
        }
        public async Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken ct = default)
        {
            var options = new PaymentIntentCreateOptions()
            {
                Amount = (long)amount,
                Currency = currency.ToLower(),
                AllowedPaymentMethodTypes = ["card"]
            };

            var intent = await _paymentIntentService.CreateAsync(options, cancellationToken: ct);
            return new PaymentIntentResult(intent.Id, intent.ClientSecret);
        }

        public async Task<PaymentIntentResult> UpdatePaymentIntentAsync(decimal amount, string paymentIntentId, CancellationToken ct = default)
        {
            var options = new PaymentIntentUpdateOptions()
            {
                Amount = (long)amount
            };

            var intent = await _paymentIntentService.UpdateAsync(paymentIntentId, options, cancellationToken: ct);
            return new PaymentIntentResult(intent.Id, intent.ClientSecret);
        }
    }
}
