using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Only include this namespace if the platform is Android
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class AndroidNotificationHandler : MonoBehaviour
{
    // Only include this constant if the platform is Android
#if UNITY_ANDROID

    private const string ChannelId = "notification_channel";

    // Schedule a notification for a specified date and time
    public void SceduleNotification(DateTime dateTime)
    {
        // Create a new notification channel with the given ID, name, description, and default importance level
        AndroidNotificationChannel notificationChannel = new AndroidNotificationChannel
        {
            Id = ChannelId,
            Name = "Notification Channel",
            Description = "Some random description",
            Importance = Importance.Default
        };

        // Register the notification channel with the Android notification center
        AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel);

        // Create a new notification with the given title, text, icons, and fire time
        AndroidNotification notification = new AndroidNotification
        {
            Title = "Energy Recharged!",
            Text = "Hello Petter Grifin here. Your energy has recharged",
            SmallIcon = "defult",
            LargeIcon = "default",
            FireTime = dateTime
        };

        // Send the notification using Android notification center
        AndroidNotificationCenter.SendNotification(notification, ChannelId);

    }
#endif
}
