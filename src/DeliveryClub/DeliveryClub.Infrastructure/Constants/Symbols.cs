using DeliveryClub.Domain.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Infrastructure.Constants
{
    public static class Symbols
    {
        public static Dictionary<Specialization, string> Specializations = new Dictionary<Specialization, string>
        {
            { Specialization.Sushi, "&#127843;" },
            { Specialization.Pizza, "&#127829;"},
            { Specialization.FastFood, "&#127839;"},
            { Specialization.Fish, "&#128031;"},
            { Specialization.Meat, "&#129385;"},
            { Specialization.Chicken, "&#127831;"},
            { Specialization.Salad, "&#129367;"}
        };

        public static Dictionary<PaymentMethod, string> PaymentMethods = new Dictionary<PaymentMethod, string>
        {
            { PaymentMethod.Card, "&#128179;"},
            { PaymentMethod.Cash, "&#128176;"},
            { PaymentMethod.Online, "&#128176;"}
        };

        public static string Truck => "&#128666;";

        public static string MoneyBag => "&#128176;";

        public static string Timer => "&#9202;";
    }
}
