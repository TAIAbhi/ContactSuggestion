using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactSuggestion.Models
{
    public class ResendNotification
    {
        public int UID { get; set; }
        public int DeviceUID   { get; set; }
        public string Text { get; set; }
        public string NotificationType { get; set; }
        public string NotificationTitle   { get; set; }
        public string Location { get; set; }
        public bool Sent   { get; set; }
        public int ? Target { get; set; }
        public DateTime ? ScheduleTIme   { get; set; }
        public DateTime ? ConfirmTime { get; set; }
        public int ? SubCategory { get; set; }
        public int ?  Microcategory { get; set; }
        public DateTime ? TimeSent { get; set; }
        public bool ? IsDone { get; set; }
        public bool ? IsDismissed { get; set; }
        public int ? RequestID { get; set; }
        public string RedirectTo  { get; set; }
        public int ? SentNotificationBy { get; set; }
        public int ContactId { get; set; }

    }
}