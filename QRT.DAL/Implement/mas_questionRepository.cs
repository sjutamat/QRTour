using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.Domain.Interface.Repository;
using QRT.DB;
using QRT.Domain.ViewModel;

namespace QRT.DAL.Implement
{
    public class mas_questionRepository : RepositoryBase<mas_question>,Imas_questionRepository
    {
        private readonly QRCodeTourEntities _dbContext;
        public mas_questionRepository(QRCodeTourEntities _Context) : base(_Context)
        {
            _dbContext = _Context;
        }

        public List<QuestionList> QuestionByLocation(string location)
        {
            var data = from q in _dbContext.mas_question
                                      join lq in _dbContext.mas_locquestion on q.question_id equals lq.question_id
                                      join l in _dbContext.mas_location on lq.location_id equals l.location_id
                                      where (l.qrcode1 == location && l.qrcode1_status == "A") || (l.qrcode2 == location && l.qrcode2_status == "A")
                                      select new QuestionList
                                      {
                                          id = q.question_id, // or pc.ProdId
                                          route_id = q.route_id,
                                          title = q.question_title,
                                          location_id = lq.location_id,
                                          location_name = l.location_title
                                      };

            var a = data.ToList();
            return data.ToList();

        }

    }
}
