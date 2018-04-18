using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.ViewModel
{
    public class m_routeViewModel
    {
        public SearchData s_route { get; set; }
        public List<RouteData> s_routeData { get; set; }
        public List<comp_item> company { get; set; }

        public long id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public long? comp_id { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? updated_date { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
    }

    public class SearchData
    {
        public string id { get; set; }
        public string title { get; set; }
        public string status { get; set; }
    }

    public class RouteData
    {
        public long id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public long comp_id { get; set; }
        //public string comp_name { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? updated_date { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
    }

    public class route_item
    {
        public long id { get; set; }
        public string text { get; set; }
    }
}