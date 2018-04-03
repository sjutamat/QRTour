using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.ViewModel
{
    public class m_empViewModel
    {
        //Model Index
        public SearchDataEmp s_emp { get; set; }
        public List<EmpData> s_empData { get; set; }

        //dropdown
        public List<comp_item> company { get; set; } //department

        public long id { get; set; }
        public string title { get; set; } // Mr. Ms. Miss hide
        public string name { get; set; }
        public string fname { get; set; }
        public string sname { get; set; }
        public string password { get; set; }
        public string code { get; set; }
        public string status { get; set; }
        public long comp { get; set; }
        public string comp_name { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? updated_date { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
    }



    public class SearchDataEmp
    {
        public string code { get; set; }
        public string name { get; set; } //name, surname or both
        public string comp { get; set; }
    }

    public class EmpData
    {
        public long id { get; set; }
        public string title { get; set; } // Mr. Ms. Miss, hide
        public string name { get; set; }
        public string fname { get; set; }
        public string sname { get; set; }
        public string password { get; set; }
        public string code { get; set; }
        public string status { get; set; }
        public long comp { get; set; }
        public string comp_name { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? updated_date { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
    }

    public class EmpLogin
    {
        public int location_id { get; set; }
        public string username { get; set; }
    }
}
