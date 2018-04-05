using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.ViewModel
{
    public class m_questionViewModel
    {
        public SearchQuestionData s_question { get; set; }
        public List<QuestionData> s_questionData { get; set; }
        
        public List<route_item> route { get; set; }
        

        public long id { get; set; }
        public long? route_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? updated_date { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
    }

    public class answer
    {
        public List<Answer> answerlist { get; set; }
        public string answer_text { get; set; }
    }

    public class quest_item
    {
        public long id { get; set; }
        public string text { get; set; }
        public string set_select { get; set; }
    }

    public class SearchQuestionData
    {
        public long id { get; set; }
        public string title { get; set; }
        public string route { get; set; }
    }

    public class QuestionData
    {
        public long id { get; set; }
        public long route_id { get; set; }
        public string title { get; set; }
        public string route_name { get; set; }
        public string status { get; set; }
        public DateTime? created_date { get; set; }
    }


    public class QuestionList
    {
        public long id { get; set; }
        public long? route_id { get; set; }
        public string title { get; set; }
        public long? location_id { get; set; }
        public string location_name { get; set; }
        public string answer_text { get; set; }
    }

    public class Answer
    {
        public string location_id { get; set; }
        public string question_id { get; set; }
        public string answer_flag { get; set; }
        public string answer_comment { get; set; }
    }

}
