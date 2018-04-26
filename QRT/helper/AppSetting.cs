using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace QRT.helper
{
    public class AppSetting
    {
        /// <summary>
        /// service.qr.url
        /// </summary>
        public static string qr_extend
        {
            get
            {
                return ConfigurationManager.AppSettings["service.qr.url"];
            }
        }
    }
}