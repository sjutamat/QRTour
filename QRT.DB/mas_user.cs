//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QRT.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class mas_user
    {
        public int user_id { get; set; }
        public string user_login { get; set; }
        public string user_password { get; set; }
        public string user_fname { get; set; }
        public string user_surname { get; set; }
        public int authen_view { get; set; }
        public int authen_edit { get; set; }
        public string user_active { get; set; }
        public Nullable<System.DateTime> user_cdate { get; set; }
        public Nullable<int> admin_id { get; set; }
    }
}
