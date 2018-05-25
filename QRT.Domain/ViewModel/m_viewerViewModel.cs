using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.ViewModel
{
    public class m_viewerViewModel
    {
        public SearchViewer s_viewer { get; set; }
        public List<ViewerData> s_viewerData {get;set;}

        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int authen_view { get; set; }
        public int authen_edit { get; set; }
        public string status { get; set; }
        public DateTime? created_date { get; set; }
        public int created_by { get; set; }
    }


    public class SearchViewer
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string status { get; set; }
    }


    public class ViewerData
    {
        public string id { get; set; }
        public string name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string status { get; set; }
    }
}
