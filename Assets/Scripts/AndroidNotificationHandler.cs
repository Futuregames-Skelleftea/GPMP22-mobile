using System.Collections;
using System.Collections.Generic;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
using System;
using UnityEngine;

// Sends notifications whenever energy has been recharged
public class AndroidNotificationHandler : MonoBehaviour
{
#if UNITY_ANDROID
    private const string ChannelId = "notifcation_channel";
    public void ScheduleNotification(DateTime dateTime)
    {
        AndroidNotificationChannel notifcationChannel = new AndroidNotificationChannel
        {
            Id = ChannelId,
            Name = "Notifcation Channel",
            Description = "Some random description",
            Importance = Importance.Default
        };

        AndroidNotificationCenter.RegisterNotificationChannel(notifcationChannel);

        AndroidNotification notification = new AndroidNotification
        {
            Title = "Energy Recharged!",
            Text = "Your energy has recharged, come back to play again!",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = dateTime
        };

        AndroidNotificationCenter.SendNotification(notification, ChannelId);
    }
#endif
}
