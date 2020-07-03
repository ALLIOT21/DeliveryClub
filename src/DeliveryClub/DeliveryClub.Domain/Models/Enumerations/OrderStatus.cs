using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.Models.Enumerations
{
    public enum OrderStatus
    {
        Received,
        Declined,
        Accepted,
        Courier,
        Delivered
    }
}
