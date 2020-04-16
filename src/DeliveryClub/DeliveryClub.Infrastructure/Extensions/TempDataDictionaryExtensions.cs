using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Infrastructure.Extensions
{
    public static class TempDataDictionaryExtensions
    {
        private const string TEMP_DATA_ACTIVE_TAB_KEY = "ACTIVE_TAB";

        public static string GetActiveTab(this ITempDataDictionary tempData)
        {
            return tempData[TEMP_DATA_ACTIVE_TAB_KEY] as string;
        }
    }
}
