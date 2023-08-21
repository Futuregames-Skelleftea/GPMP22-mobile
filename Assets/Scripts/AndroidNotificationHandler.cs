using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
using UnityEngine;

public class AndroidNotificationHandler : MonoBehaviour
{
#if UNITY_ANDROID
    // Notification channel ID
    private const string ChannelId = "notification_channel";

    // Schedule a notification at a specific date and time
    public void ScheduleNotification(DateTime dateTime)
    {
        // Create a new AndroidNotificationChannel
        AndroidNotificationChannel notificationChannel = new AndroidNotificationChannel
        {
            Id = ChannelId,
            Name = "Notification Channel",
            Description = "Some random description",
            Importance = Importance.Default
        };

        // Register the notification channel
        AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel);

        // Create a new AndroidNotification
        AndroidNotification notification = new AndroidNotification
        {
            Title = "Energy Recharged!",
            Text = "Your energy has recharged, come back to play again!",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = dateTime
        };

        // Send the notification with the specified channel ID
        AndroidNotificationCenter.SendNotification(notification, ChannelId);
    }
#endif
}
