using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class AndroidNotificationHandler : MonoBehaviour
{
#if UNITY_ANDROID
    private const string ChannelId = "notification_channel";

    // Use this method to schedule a notification
    public void ScheduleNotification(DateTime dateTime)
    {
        // Create a new notification channel with the specified ID, name, and description
        AndroidNotificationChannel notificationChannel = new AndroidNotificationChannel
        {
            Id = ChannelId,
            Name = "Notification Channel",
            Description = "Some random description",
            Importance = Importance.Default
        };

        // Register the notification channel
        AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel);

        // Create a new notification with the specified title, message, and icon
        AndroidNotification notification = new AndroidNotification
        {
            Title = "Energy Recharged",
            Text = "Your energy has recharged, Come back to play again",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = dateTime
        };

        // Schedule the notification
        AndroidNotificationCenter.SendNotification(notification, ChannelId);
    }
#endif
}
