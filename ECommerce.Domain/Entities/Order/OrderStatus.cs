using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Order
{
    public enum OrderStatus
    {
        Pending = 0,
        PaymentReceived,
        PaymentFailed
    }
}
