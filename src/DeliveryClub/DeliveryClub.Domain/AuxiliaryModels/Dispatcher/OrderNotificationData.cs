using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.AuxiliaryModels.Dispatcher
{
    public class OrderNotificationData
    {
        public string Message { get; set; }

        public DispatcherOrderModel DispatcherOrderModel { get; set; }
    }
}
