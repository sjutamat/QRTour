using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.ViewModel
{
    public class m_companyViewModel
    {
        public SearchComp s_comp { get; set; }
        public List<Comp_data> s_compData { get; set; }
      //public List<comp_item> company { get; set; }

        public long id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string flag_internal { get; set; }
        public string flag_active { get; set; }
        public long? admin_id { get; set; }
    }
    public class comp_item
    {
        public long id { get; set; }
        public string text { get; set; }
        public bool? flag_internal { get; set; } 
    }


    public class SearchComp
    {
        public string id { get; set; }
        public string title { get; set; }
        public string status { get; set; }
    }


    public class Comp_data {
        public long id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string flag_internal { get; set; }
        public string flag_active { get; set; }
        public long? admin_id { get; set; }
    }
}
