using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.ViewModel
{
    public class m_locquestionViewModel
    {
        public long id { get; set; }
        public long? location_id { get; set; }
        public string location_name { get; set; }
        public long? question_id { get; set; }
        public string question_name { get; set; }
        public string[] question_arr { get; set; }
        public string status { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? updated_date { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }

        public List<location_item> location { get; set; }
        public List<question_item> question { get; set; }
    }

    public class location_item
    {
        public long id { get; set; }
        public string text { get; set; }
    }

    public class question_item
    {
        public long id { get; set; }
        public string text { get; set; }
    }
}
