using Microsoft.Azure.ServiceBus.Management;
using System;

namespace MPSTI.PlenoSoft.Core.Azure.ServiceBus.Contracts
{
    public static class ServiceBusTemplate
    {
        public static readonly QueueDescription QueueTemplate = new("QueueName")
        {
            DefaultMessageTimeToLive = TimeSpan.FromDays(14),
            LockDuration = TimeSpan.FromMinutes(1),
            MaxDeliveryCount = 10,
            MaxSizeInMB = 1024,

            EnableDeadLetteringOnMessageExpiration = true,
            EnableBatchedOperations = false,
            EnablePartitioning = true,
        };

        public static readonly TopicDescription TopicTemplate = new("TopicName")
        {
            AutoDeleteOnIdle = TimeSpan.FromDays(30),
            DefaultMessageTimeToLive = TimeSpan.FromDays(14),
            DuplicateDetectionHistoryTimeWindow = TimeSpan.FromDays(1),
            EnableBatchedOperations = false,
            EnablePartitioning = true,
            MaxSizeInMB = 1024,
            RequiresDuplicateDetection = false,
            Status = EntityStatus.Active,
            SupportOrdering = false,
            UserMetadata = "",
        };

        public static QueueDescription GetQueueDescription(string queueName)
        {
            return new QueueDescription(queueName)
            {
                DefaultMessageTimeToLive = QueueTemplate.DefaultMessageTimeToLive,
                LockDuration = QueueTemplate.LockDuration,
                MaxDeliveryCount = QueueTemplate.MaxDeliveryCount,
                MaxSizeInMB = QueueTemplate.MaxSizeInMB,

                EnableDeadLetteringOnMessageExpiration = QueueTemplate.EnableDeadLetteringOnMessageExpiration,
                EnableBatchedOperations = QueueTemplate.EnableBatchedOperations,
                EnablePartitioning = QueueTemplate.EnablePartitioning
            };
        }

        public static TopicDescription GetTopicDescription(string topicName)
        {
            return new TopicDescription(topicName)
            {
                AutoDeleteOnIdle = TopicTemplate.AutoDeleteOnIdle,
                DefaultMessageTimeToLive = TopicTemplate.DefaultMessageTimeToLive,
                DuplicateDetectionHistoryTimeWindow = TopicTemplate.DuplicateDetectionHistoryTimeWindow,
                EnableBatchedOperations = TopicTemplate.EnableBatchedOperations,
                EnablePartitioning = TopicTemplate.EnablePartitioning,
                MaxSizeInMB = TopicTemplate.MaxSizeInMB,
                RequiresDuplicateDetection = TopicTemplate.RequiresDuplicateDetection,
                Status = TopicTemplate.Status,
                SupportOrdering = TopicTemplate.SupportOrdering,
                UserMetadata = TopicTemplate.UserMetadata
            };
        }
    }
}