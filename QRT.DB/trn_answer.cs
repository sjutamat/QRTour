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
    
    public partial class trn_answer
    {
        public long answer_id { get; set; }
        public Nullable<long> transaction_id { get; set; }
        public Nullable<long> emp_id { get; set; }
        public Nullable<long> location_id { get; set; }
        public Nullable<long> question_id { get; set; }
        public string answer_txt { get; set; }
        public string session_id { get; set; }
        public Nullable<System.DateTime> answer_cdate { get; set; }
        public string answer_picture { get; set; }
    
        public virtual mas_emp mas_emp { get; set; }
        public virtual mas_location mas_location { get; set; }
        public virtual mas_question mas_question { get; set; }
        public virtual trn_transaction trn_transaction { get; set; }
    }
}
