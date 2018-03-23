using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.ViewModel
{
    public class m_locationViewModel
    {
        public SearchDataLocation s_location { get; set; }
        public List<LocationData> s_locationData { get; set; }

        public List<route_item> route { get; set; }
        public List<quest_item> quest { get; set; }

        public long id { get; set; }
        public long? route_id { get; set; }
        public string route_name { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int? seq_number { get; set; }
        public string status { get; set; }
        public string qrcode1 { get; set; }
        public string code1_status { get; set; }
        public string qrcode2 { get; set; }
        public string code2_status { get; set; }
        public DateTime? created_date { get; set; }
        public int? created_by { get; set; }
        public DateTime? updated_date { get; set; }
        public int? updated_by { get; set; }
    }

    public class SearchDataLocation
    {
        public long id { get; set; }
        public string title { get; set; }
        public string route { get; set; }
    }

    public class LocationData
    {
        public long id { get; set; }
        public long? route_id { get; set; }
        public string route_name { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int? seq_number { get; set; }
        public string status { get; set; }
        public DateTime? created_date { get; set; }
        public int? created_by { get; set; }
        public DateTime? updated_date { get; set; }
        public int? updated_by { get; set; }
    }
}
