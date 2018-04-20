using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.ViewModel
{
    public class dashboardViewModel
    {
        public SearchDataDashboard s_dashboard { get; set; }
        public List<answerHeader> reportList { get; set; }
        public List<route_item> route { get; set; }
    }


    public class answerHeader
    {
        public long route_id { get; set; }
        public string name { get; set; }
        public DateTime? start_time { get; set; } //เวลาของ Location ที่ 1
        public DateTime? end_time { get; set; } // เวลาของ Location สุดท้าย
        public string start_time_string { get; set; }
        public string end_time_string { get; set; }
        public long emp_id { get; set; }
        public string emp_name { get; set; }
        public string comp_name { get; set; }
        public List<employeeData> listEmpData { get; set; }
    }

    public class employeeData
    {
        public long id { get; set; }
        public string name { get; set; }
        public DateTime? start_time { get; set; } //เวลาของ Location ที่ 1
        public DateTime? end_time { get; set; } // เวลาของ Location สุดท้าย
        public string start_time_string { get; set; }
        public string end_time_string { get; set; }
        public List<answerDetailList> answerSummary { get; set; }
    }

    public class answerDetailList
    {
        public long? location_id { get; set; }
        public string location_name { get; set; }
        public DateTime? answer_cdate { get; set; }
        public string answer_cdate_string { get; set; }
        public string answer_flag { get; set; }
        public string answer_comment { get; set; }
        public long? answer_emp_id { get; set; }
        public string answer_emp_name { get; set; }
        public string answer_emp_company { get; set; }
    }

    public class SearchDataDashboard
    {
        public string date_start { get; set; }
        public string date_end { get; set; }
        public string route { get; set; }
    }
}
