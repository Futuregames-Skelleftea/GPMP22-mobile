using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class AndroidNoteHandler : MonoBehaviour
{
#if UNITY_ANDROID
    //channelID are a constant string
    private const string ChannelId = "note_channel";

    //Schedules a notification to be shown at a specified time
    public void ScheduleNote(DateTime dateTime)
    {
        //Create a new notification channel
        AndroidNotificationChannel noteChannel = new AndroidNotificationChannel
        {
            Id = ChannelId,
            Name = "Notification Channel",
            Description = "Some random stuff",
            Importance = Importance.Default
        };

        //Register notification channel to the Android notification system
        AndroidNotificationCenter.RegisterNotificationChannel(noteChannel);

        //Create a new notification
        AndroidNotification notification = new AndroidNotification
        {
            Title = "Energy Recharged!",
            Text = "Your energy has recharged!",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = dateTime
        };
        //Sending notification using the channel ID
        AndroidNotificationCenter.SendNotification(notification, ChannelId);
    }
#endif
}
