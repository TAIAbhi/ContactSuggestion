using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace ContactSuggestion.Models
{
    public class RequestNotificationViewModel
    {
        public IList<Category> Categories { get; set; }
        [Display(Name = "Location")]
        public string LocationName { get; set; }
        public int  ? LocationId { get; set; }
        public Category MainCategory { get; set; }
        public int ? CatId { get; set; }
        public int ?  UID  { get; set; }
        public int DeviceUID  { get; set; }
        [Display(Name = "Description")]
        public string Text  { get; set; }
        [Display(Name = "Notification Type")]
        public string NotificationType  { get; set; }
        [Display(Name = "Notification Title")]
        public string NotificationTitle  { get; set; }    
        public string Sent  { get; set; }
        public DateTime ? ScheduleTIme  { get; set; }
        public DateTime ? ConfirmTime  { get; set; }
        public DateTime ? FeedbackResponse  { get; set; }
        public DateTime ? FBTime  { get; set; }
        public int ? SubCategoryId  { get; set; }
        public int ? MicrocategoryId  { get; set; }
        [Display(Name = "Sub Category")]
        public string Microcategory { get; set; }
        [Display(Name = "Notification Photo")]
        public string NotificationPhoto  { get; set; }
        public DateTime ? TimeSent  { get; set; }
        public int  ContactId { get; set; }
        public string Contact { get; set; }
        public int City { get; set; }

        public  string Notify { get; set; }
        public string RedirectTo { get; set; }
        public string AddOrView { get; set; }
        public string ProvdReqdsuggData { get; set; }
    }

    public class PushNotification
    {
        public string[] registration_ids { get; set; }
        public Notifications notification { get; set; }
        public PushData data { get; set; }

    }
    public class Notifications
    {
        public string title { get; set; }
        public string body { get; set; }
        public string sound { get; set; }
        public string vibrate { get; set; }

        public string priority { get; set; }

    }
    public class PushData
    {
        public string title { get; set; }
        public string body { get; set; }
        public string date { get; set; }
        public int ? catId { get; set; }
        public int ? subCatId { get; set; }
        public int ? microId { get; set; }
        public int  ? locationId { get; set; }
        public string contactId { get; set; }
        public string bussinessName { get; set; }
        public string suggestionId { get; set; }
     


    }
}