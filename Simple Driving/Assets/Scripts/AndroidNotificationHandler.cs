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
    public void ScheduleNotification(DateTime dateTime) // Send notification if function is called.
    {
        AndroidNotificationChannel notificationChannel = new AndroidNotificationChannel // Channel settings.
        {
            Id = ChannelId,
            Name = "Notification Channel",
            Description = "Some random description",
            Importance = Importance.Default

        };

        AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel); // Register Channel.

        AndroidNotification notification = new AndroidNotification // Design of the notification to send.
        {
            Title = "Energy Recharged",
            Text = "Your energy has been recharged, come back to play again!",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = dateTime

        };

        AndroidNotificationCenter.SendNotification(notification, ChannelId); // Send notification.

    }
#endif
}
