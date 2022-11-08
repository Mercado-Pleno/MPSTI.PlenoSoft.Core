using Microsoft.Azure.ServiceBus;
using System;

namespace MPSTI.PlenoSoft.Core.Azure.ServiceBus.Extensions
{
    public static class MessageExtensions
    {
        public static string DeliveryCount = "DeliveryCount";

        public static Message CreateClone(this Message message, int count, TimeSpan timeSpan)
        {
            var newMessage = message.Clone();
            newMessage.UserProperties[DeliveryCount] = count;
            newMessage.ScheduledEnqueueTimeUtc = DateTime.UtcNow.Add(timeSpan);
            return newMessage;
        }

        public static bool PodeReprocessar(this Message message, out int count)
        {
            count = message.UserProperties.TryGetValue(DeliveryCount, out var value)
                ? Convert.ToInt32(value)
                : 0;

            return count < 5;
        }
    }
}