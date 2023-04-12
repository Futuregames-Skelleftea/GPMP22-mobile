using Unity.Notifications.Android;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class AndroidNotificationHandler
{
    private const string ChannelId = "notification_channel";


    public void ScheduleNotification(DateTime dateTime)
    {
        AndroidNotificationChannel notificationChannel = new()
        {
            Id = ChannelId,
            Name = "Notification Channel",
            Description = "A channel for notifications",
            Importance = Importance.Default
        };

        AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel);

        AndroidNotification notification = new()
        {
            Title = "Plays Replenished!",
            Text = "times up, lets do this",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = dateTime
        };

        AndroidNotificationCenter.SendNotification(notification,ChannelId);
    }


}
