using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System;


namespace ServiceFirstApplication.Repository
{
    public class MessageStatusType
    {
        public static string Open { get { return "A"; } }
        public static string Closed { get { return "C"; } }
        public static string Replyed { get { return "R"; } }

    }
    public class SlotColor
    {
        public static string White { get { return "white"; } }
        public static string Pink { get { return "pink"; } }
        public static string Green { get { return "green"; } }
    }

    public class SessionStatus
    {
        public static string Logout { get { return "L"; } }
        public static string Active { get { return "A"; } }
        public static string InActive { get { return "A"; } }
        public static string Expire { get { return "E"; } }
    }

    public class NotificationModule
    {
        public static string ContactUs { get { return "ContactUs"; } }
        public static string Feedback { get { return "UserFeedback"; } }
        public static string ReportAnError { get { return "ReportAnError"; } }
        public static string MedicineReportAnError { get { return "MedicineReportAnError"; } }
        public static string LabReportLabaoratory { get { return "LabReportLabaoratory"; } }
        public static string LabReportPatient { get { return "LabReportPatient"; } }
        public static string PurchaseOrder { get { return "PurchaseOrder"; } }
        public static string BookAppointment { get { return "BookAppointment"; } }
        public static string BloodDonation { get { return "BloodDonation"; } }
        public static string DoctorPrescriptionForm { get { return "DoctorPrescriptionForm"; } }
        public static string SendForQuotation { get { return "SendForQuotation"; } }
        public static string OrderDetail { get { return "OrderDetail"; } }
        public static string ShareMedicalhistory { get { return "ShareMedicalhistory"; } }
        public static string MedicalVault { get { return "MedicalVault"; } }
        public static string PatientRecords { get { return "PatientRecords"; } }
        public static string ManageDoctorPrescriptionForm { get { return "ManageDoctorPrescriptionForm"; } }
        public static string SendMessage { get { return "SendMessage"; } }
        public static string MessageDetail { get { return "MessageDetail"; } }
        public static string SOSPushNotificationModule { get { return "SOSPushNotificationModule"; } }
        public static string MedicineReminderPushNotification { get { return "MedicineReminderPushNotification"; } }
        public static string ManageSettings { get { return "ManageSettings"; } }

        public static string FlatDiscount { get { return "FlatDiscount"; } }
        
        public static string RelativeSOS { get { return "RelativeSOS1"; } }
        public static string VersionUpdates { get { return "New App Version"; } }

    }

    public class UserTypeChar
    {
        public static string SuperAdminChar { get { return "S"; } }//Super Admin
        public static string AdminChar { get { return "A"; } }//Admin
        public static string FrontUser { get { return "U"; } }//Front user

    }

    public class UserType
    {
        public static string SuperAdmin { get { return UserTypeChar.SuperAdminChar + UserTypeChar.AdminChar; } }
        public static string Admin { get { return UserTypeChar.AdminChar + UserTypeChar.FrontUser; } }

        public static string GetFullUserType(string type)
        {
            if (type == SuperAdmin)
                return "Super Admin";
            else if (type == Admin)
                return "Administrator";
            return "";
        }

        public static string GetFolder(string type)
        {
            if (type == SuperAdmin)
                return "admin";
            else if (type == Admin)
                return "admin";

            return "";
        }

        public string Tag { get; set; }
        public string Name { get; set; }

        public static IEnumerable<UserType> GetSystemUsers()
        {
            List<UserType> uList = new List<UserType>();
            uList.Add(new UserType() { Name = GetFullUserType(Admin), Tag = Admin });
            return uList.AsEnumerable();
        }
    }
    public class OrderStatusChar
    {
        public static string Cancel { get { return "C"; } }
        public static string Pending { get { return "P"; } }
        public static string Shipped { get { return "S"; } }
        public static string Delivered { get { return "D"; } }
    }

    public class OrderStatus
    {
        public static string Cancel { get { return OrderStatusChar.Cancel; } }
        public static string Pending { get { return OrderStatusChar.Pending; } }
        public static string Shipped { get { return OrderStatusChar.Shipped; ; } }
        public static string Delivered { get { return OrderStatusChar.Delivered; ; } }
        public static string GetFullOrderStatus(string type)
        {
            if (type == Cancel)
                return "Cancel";
            else if (type == Pending)
                return "Pending";
            else if (type == Shipped)
                return "Shipped";
            else if (type == Delivered)
                return "Delivered";
            return " ";
        }
    }
    public class NotificationStatusChar
    {
        public static string Read { get { return "R"; } }
        public static string UnRead { get { return "U"; } }
    }
    public class NotificationStatusType
    {
        public static string Read { get { return NotificationStatusChar.Read; } }
        public static string UnRead { get { return NotificationStatusChar.UnRead; } }
        public static string GetFullNofiticationStatus(string type)
        {
            if (type == Read)
                return "Read";
            else if (type == UnRead)
                return "UnRead";
            return "";
        }
    }

    public class TimeSpanResource
    {
        public static string From { get { return "From"; } }
        public static string To { get { return "To"; } }
        public static string Previous { get { return "Previous"; } }
        public static string Next { get { return "Next"; } }
    }

    public class PageCalled
    {
        public static string AppointmentBooking { get { return "BookingApp"; } }
        public static string AppointmentCallendar { get { return "AppointmentCallendar"; } }
    }






    public class TimeSpanDay
    {
        public string DayName { get; set; }
        public string Date { get; set; }
        public bool IsBookingAvail { get; set; }

        public static DateTime GetWeekFirstDate(DateTime d)
        {
            // lastMonday is always the Monday before nextSunday.
            // When today is a Sunday, lastMonday will be tomorrow.     

            if (d.DayOfWeek == DayOfWeek.Sunday)
                d = d.AddDays(-1);

            int offset = d.DayOfWeek - DayOfWeek.Monday;
            DateTime lastMonday = d.AddDays(-offset);

            return lastMonday;
        }

        public static DateTime GetWeekLastDate(DateTime d)
        {
            // lastMonday is always the Monday before nextSunday.
            // When today is a Sunday, lastMonday will be tomorrow.     

            if (d.DayOfWeek == DayOfWeek.Sunday)
                d = d.AddDays(-1);

            int offset = d.DayOfWeek - DayOfWeek.Monday;
            DateTime lastMonday = d.AddDays(-offset);

            DateTime nextSunday = lastMonday.AddDays(6);

            return nextSunday;
        }
    }

    public static class DateTimeExtensions
    {
        public enum Days
        {
            SUNDAY = 0,
            MONDAY = 1,
            TUESDAY = 2,
            WEDNESDAY = 3,
            THURSDAY = 4,
            FRIDAY = 5,
            SATURDAY = 6
        }

        public static DateTime StartOfWeek(this DateTime dt, Days startOfWeek)
        {
            int diff = (int)dt.DayOfWeek - (int)startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        public static IEnumerable<DateTime> Range(this DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, (endDate - startDate).Days + 1).Select(d => startDate.AddDays(d));
        }
    }

    public class RegistrationType
    {
        public static string Doctor { get { return "Doctor"; } }
        public static string Patient { get { return "Patient"; } }
        public static string Lab { get { return "Lab"; } }
        public static string Nurse { get { return "Nurse"; } }

        public static string GetFullRegistrationType(string type)
        {
            if (type == Doctor)
                return "Doctor";
            else if (type == Patient)
                return "Patient";
            else if (type == Lab)
                return "Lab";
            else if (type == Nurse)
                return "Nurse";
            return "";
        }
    }

    public class ModuleValues
    {
        public static string AllRights { get { return "allrights"; } }
        public static string ManageAdminUsers { get { return "adminuser"; } }
        public static string ManageUserRole { get { return "role"; } }
        public static string ManageDoctors { get { return "doctors"; } }
        public static string ManagePatients { get { return "patients"; } }
        public static string ManageLaboratories { get { return "laboratories"; } }
        public static string ManageNurses { get { return "nurses"; } }
        public static string ManageMedicines { get { return "medicines"; } }
        public static string ManageOrders { get { return "orders"; } }
        public static string ManagePrescriptionsQuotations { get { return "prescriptionsquotations"; } }

        public static string ManageCities { get { return "cities"; } }
        public static string ManageLocations { get { return "locations"; } }
        public static string ManageZipCodes { get { return "zipcodes"; } }


        public static string ManageFeedbacks { get { return "feedbacks"; } }
        public static string ManageErrorReports { get { return "errorreports"; } }
        public static string ManageMedicineErrorReport { get { return "medicineerrorreports"; } }
        public static string ManageSettings { get { return "settings"; } }
        public static string ManageContactUs { get { return "contactus"; } }

        public static string ManagePromoCodes { get { return "promocodes"; } }
    }

    public class RegularExp
    {
        public const string UserName = @"([a-zA-Z]+)";
        public const string Password = @"^[^<>]*$";
        public const string Email = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        public const string Url = @"^http\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$";
        public const string AlphaNumeric = @"([a-zA-Z0-9@ ]+)";
        public const string Numeric = @"([0-9]+)";
        public const string Decimal = @"[\d]{1,15}([.][\d]{1,2})?";
        public const string Alphabets = @"([a-zA-Z ]+)";
        public const string PhoneNumber = @"^[(+),(.),0-9 (,),-]+$";// if phone req
        public const string ZipCode = @"^\d{5}$";
        public const string PhoneNotReq = @"^([(+),(.),0-9 (,),-]+){10,20}$";// if phone not req
        public const string GTZeroNumeric = @"(^[1-9][0-9]*$)";
        public const string Date = @"^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d{2}$";
    }
    public class MessageType
    {
        public static string MainMessage { get { return "M"; } }
        public static string ReplyMessage { get { return "R"; } }

        public static string GetFullMessageType(string type)
        {
            if (type == MainMessage)
                return "MainMessage";
            else if (type == ReplyMessage)
                return "ReplyMessage";
            return "";
        }

        public static IEnumerable<object> GetMessageTypeList()
        {
            List<object> uList = new List<object>();
            uList.Add(new { Name = GetFullMessageType(MainMessage), Tag = MainMessage });
            uList.Add(new { Name = GetFullMessageType(ReplyMessage), Tag = ReplyMessage });

            return uList.AsEnumerable();
        }
    }
    public class StatusType
    {
        public static string Active { get { return "A"; } }
        public static string InActive { get { return "I"; } }
        public static string Delete { get { return "D"; } }
        public static string ErrorOccured { get { return "E"; } }

        public static string GetFullStatusType(string type)
        {
            if (type == Active)
                return "Active";
            else if (type == InActive)
                return "InActive";
            return "";
        }

        public static IEnumerable<object> GetStatusTypeList()
        {
            List<object> uList = new List<object>();
            uList.Add(new { Name = GetFullStatusType(Active), Tag = Active });
            uList.Add(new { Name = GetFullStatusType(InActive), Tag = InActive });

            return uList.AsEnumerable();
        }
    }

    public class SlotDuration
    {
        public string Text { get; set; }
        public int Value { get; set; }
        public static IEnumerable<SlotDuration> GetSlotDurationList(string Usertype)
        {
            List<SlotDuration> uList = new List<SlotDuration>();


            if (Usertype == RegistrationType.Nurse)
            {
                for (int i = 1; i <= 12; )
                {
                    uList.Add(new SlotDuration { Text = i.ToString() + " Hours", Value = i * 60 });
                    i++;
                }
            }
            else
            {
                for (int i = 10; i <= 60; )
                {
                    uList.Add(new SlotDuration { Text = i.ToString() + " Minutes", Value = i });
                    i = i + 10;
                }

            }

            return uList.AsEnumerable();
        }
    }

    public class PromoCodeTypes
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public static IEnumerable<PromoCodeTypes> GetPromotionCodeTypes()
        {
            List<PromoCodeTypes> uList = new List<PromoCodeTypes>();
            uList.Add(new PromoCodeTypes { Text = "Percentage", Value = "Percentage" });
            uList.Add(new PromoCodeTypes { Text = "Fixed Price", Value = "FixedPrice" });
            return uList.AsEnumerable();
        }
    }

    public class PromoCodeType
    {
        public static string Percentage { get { return "Percentage"; } }
        public static string FixedPrice { get { return "FixedPrice"; } }
    }

    //    Tablet
    //Capsule
    //bottle
    //strip
    //injection
    //tube
    //drops


    public class MedicineTypes
    {

        public string Text { get; set; }
        public string Value { get; set; }
        public bool CheckedStatus { get; set; }
        public static IEnumerable<MedicineTypes> GetMedicineTypes()
        {
            List<MedicineTypes> uList = new List<MedicineTypes>();
            uList.Add(new MedicineTypes { Text = "Tablet", Value = MedicineType.Tablet });
            uList.Add(new MedicineTypes { Text = "Capsule", Value = MedicineType.Capsule });
            uList.Add(new MedicineTypes { Text = "Bottle", Value = MedicineType.Botle });
            uList.Add(new MedicineTypes { Text = "Injection", Value = MedicineType.Injection });
            uList.Add(new MedicineTypes { Text = "Tube", Value = MedicineType.Tube });
            uList.Add(new MedicineTypes { Text = "Drops", Value = MedicineType.Drops });
            return uList.AsEnumerable();
        }
    }


    public class MedicineType
    {
        public static string Tablet { get { return "tablet"; } }
        public static string Capsule { get { return "capsule"; } }
        public static string Botle { get { return "bottle"; } }
        public static string Injection { get { return "injection"; } }
        public static string Tube { get { return "tube"; } }
        public static string Drops { get { return "drops"; } }
    }




    public class ApprovedStatusType
    {
        public static string Pending { get { return "P"; } }
        public static string Approved { get { return "A"; } }
        public static string Rejected { get { return "R"; } }
        public static string Cancel { get { return "C"; } }

        public static string GetFullApprovedStatusType(string type)
        {
            if (type == Pending)
                return "Pending";
            else if (type == Approved)
                return "Approved";
            else if (type == Rejected)
                return "Rejected";
            else if (type == Cancel)
                return "Cancelled";

            return "";
        }

        public static IEnumerable<object> GetApprovedStatusTypeList()
        {
            List<object> uList = new List<object>();
            uList.Add(new { Name = GetFullApprovedStatusType(Pending), Tag = Pending });
            uList.Add(new { Name = GetFullApprovedStatusType(Approved), Tag = Approved });
            uList.Add(new { Name = GetFullApprovedStatusType(Rejected), Tag = Rejected });
            return uList.AsEnumerable();
        }
    }
    public class SelectLists
    {
        public static IEnumerable<object> GetGenderTypeList()
        {
            List<object> uList = new List<object>();
            uList.Add(new { Name = "Select", Tag = "" });
            uList.Add(new { Name = "Male", Tag = "M" });
            uList.Add(new { Name = "Female", Tag = "F" });
            return uList.AsEnumerable();
        }

        public static IEnumerable<object> GetPhoneTypeList()
        {
            List<object> uList = new List<object>();
            uList.Add(new { Name = "Select", Tag = "" });
            uList.Add(new { Name = "Home", Tag = "Home" });
            uList.Add(new { Name = "Mobile", Tag = "Mobile" });
            uList.Add(new { Name = "Office", Tag = "Office" });
            return uList.AsEnumerable();
        }

        public static IEnumerable<object> GetMonthNames()
        {
            List<object> uList = new List<object>();
            uList.Add(new { Name = "Month", Tag = "" });
            string fmt = "00.##";
            for (int i = 1; i <= 12; i++)
            {
                //uList.Add(new { Name = DateTimeFormatInfo.CurrentInfo.GetMonthName(i), Tag = i });
                uList.Add(new { Name = i.ToString(fmt), Tag = i.ToString(fmt) });
            }
            return uList.AsEnumerable();
        }

        public static IEnumerable<object> GetYears(int fromYear = 0)
        {
            List<object> uList = new List<object>();
            uList.Add(new { Name = "Year", Tag = "" });

            int currYear = 0;
            if (fromYear > 0)
                currYear = fromYear;
            else
                currYear = Convert.ToInt32(CommonFunctions.GetYear());

            for (int i = currYear - 1; i <= currYear + 10; i++)
            {
                uList.Add(new { Name = i, Tag = i });
            }
            return uList.AsEnumerable();
        }

        public static IEnumerable<object> GetMaritalList()
        {
            List<object> uList = new List<object>();
            uList.Add(new { Name = "Select", Tag = "" });
            uList.Add(new { Name = "Single", Tag = "Single" });
            uList.Add(new { Name = "Married", Tag = "Married" });
            return uList.AsEnumerable();
        }

        public static IEnumerable<object> GetEducationType()
        {
            List<object> uList = new List<object>();
            uList.Add(new { Name = "Select", Tag = "" });
            uList.Add(new { Name = "High School Grade", Tag = "High School Grade" });
            uList.Add(new { Name = "College Grade", Tag = "College Grade" });
            uList.Add(new { Name = "Other", Tag = "Other" });
            return uList.AsEnumerable();
        }

    }
}