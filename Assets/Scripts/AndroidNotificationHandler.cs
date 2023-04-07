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
    private const string ChanellId = "notification_chanell";

    // Schedule notification for energy recharge
    public void ScheduleNotification(DateTime dateTime)
    {
        // Define notification channel
        AndroidNotificationChannel notificationChannel = new AndroidNotificationChannel
        {
            Id = ChanellId,
            Name = "Notification Chanell",
            Description = "Some random description",
            Importance = Importance.Default 
        };

        // Register notification channel
        AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel);

        // Create notification
        AndroidNotification notification = new AndroidNotification
        {
            Title = "Energy Recharged!",
            Text = "Your energy has been recharged! Come back and play!",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = dateTime
        };

        // Send notification
        AndroidNotificationCenter.SendNotification(notification, ChanellId);
    }
#endif
}
