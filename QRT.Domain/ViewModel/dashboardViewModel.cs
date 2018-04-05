using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.ViewModel
{
    public class dashboardViewModel
    {
        public List<answerHeader> reportList { get; set; }
    }


    public class answerHeader
    {
        public long route_id { get; set; }
        public string name { get; set; }
        public DateTime? start_time { get; set; } //เวลาของ Location ที่ 1
        public DateTime? end_time { get; set; } // เวลาของ Location สุดท้าย
        public long emp_id { get; set; }
        public string emp_name { get; set; }
        public string comp_name { get; set; }
        public List<answerDetailList> answerList { get; set; }
    }


    public class answerDetailList
    {
        public long? location_id { get; set; }
        public string location_name { get; set; }
        public DateTime? answer_cdate { get; set; }
        public string answer_flag { get; set; }
        public string answer_comment { get; set; }
        public long? answer_emp_id { get; set; }
        public string answer_emp_name { get; set; }
        public string answer_emp_company { get; set; }
    }
}
