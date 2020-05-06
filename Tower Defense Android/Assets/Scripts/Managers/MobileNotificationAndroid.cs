using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

public class MobileNotificationAndroid : MonoBehaviour
{
    void Start()
    {
        CreateChannel();
        SendReturnNotification();
    }

    private void CreateChannel()
    {
        AndroidNotificationChannel channel = new AndroidNotificationChannel()
        {
            Id = "call back",
            Name = "Call back channel",
            Description = "channel to call players back",
            Importance = Importance.Default
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    private void SendReturnNotification()
    {
        AndroidNotification notification = new AndroidNotification()
        {
            Title = "Please, Come in!",
            Text = "Waves are not over yet",
            LargeIcon = "return_airplane",
            FireTime = System.DateTime.Now.AddHours(2)
        };

        var identifier = AndroidNotificationCenter.SendNotification(notification, "call back");

        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Delivered)
        {
            SendSecondReturnNotification();
        }
    }

    private void SendSecondReturnNotification()
    {
        AndroidNotification secondNotification = new AndroidNotification()
        {
            Title = "Where are you?",
            Text = "Don't disappoint me and rush to play",
            LargeIcon = "return_rocket",
            FireTime = System.DateTime.Now.AddHours(3)
        };

        AndroidNotificationCenter.SendNotification(secondNotification, "call back");
    }

    private void OnApplicationQuit()
    {
        SendReturnNotification();
    }
}
