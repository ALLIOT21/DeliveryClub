using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace DeliveryClub.Infrastructure.Extensions
{
    public static class TempDataDictionaryExtensions
    {
        private const string TEMP_DATA_NOTIFICATION_MESSAGE_KEY = "Notification";

        public static void AddNotificationMessage(this ITempDataDictionary tempData, Notification notification)
        {
            tempData[TEMP_DATA_NOTIFICATION_MESSAGE_KEY] = JsonConvert.SerializeObject(notification);
        }        

        public static Notification GetNotificationMessage(this ITempDataDictionary tempData)
        {
            if (tempData.ContainsKey(TEMP_DATA_NOTIFICATION_MESSAGE_KEY))
                return JsonConvert.DeserializeObject<Notification>(tempData[TEMP_DATA_NOTIFICATION_MESSAGE_KEY].ToString());
            else
                return null;
        }
    }
}
