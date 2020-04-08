using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.Models
{
    public static class Role
    {
        public static string User => "User";

        public static string SuperUser => "SuperUser";

        public static string Admin => "Admin";

        public static string Dispatcher => "Dispatcher";
    }
}
