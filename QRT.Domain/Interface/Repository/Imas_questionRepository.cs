using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.DB;
using QRT.Domain.ViewModel;

namespace QRT.Domain.Interface.Repository
{
    public interface Imas_questionRepository:IRepositoryBase<mas_question>
    {
        List<QuestionList> QuestionByLocation(string location);
    }
}
