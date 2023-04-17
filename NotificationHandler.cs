using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationHandler : MonoBehaviour
{
    private const string ChannelID = "notification_channel";

    public void ScheduleNotification(DateTime datetime)
    {
        //notification details
        AndroidNotificationChannel notificationChannel = new AndroidNotificationChannel();
        {
            // All these gave errors
            //Id = "ChannelID";
            //Name = "notification_channel";
            //Description = "ok";
            //Importance = Importance.Default;
        };

        AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel);

        //how the notificiation looks
        AndroidNotification notification = new AndroidNotification();
        {
            // All these gave errors
            //Title = "energy Full";
            //Text = "Play again";
            //Smallicon = "default";
            //Largeicon = "default";
            //Firetime = datetime;
        };
    }


}
