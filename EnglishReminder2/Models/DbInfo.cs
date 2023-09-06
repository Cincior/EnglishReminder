using System;
using System.Collections.Generic;
using System.Text;

namespace EnglishReminder2.Models
{
    public class DbInfo
    {
        public static string srvrdbname = "EnglishReminder";
        public static string srvrname = "192.168.2.106";
        public static string srvrusername = "EnglishReminderAdmin";
        public static string srvrpassword = "12345";
        public static string sqlconn = $"Data Source={srvrname};Initial Catalog ={srvrdbname}; User ID ={srvrusername}; Password ={srvrpassword}";
    }       
}
