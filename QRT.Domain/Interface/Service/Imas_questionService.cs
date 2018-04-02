using QRT.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.Interface.Service
{
    public interface Imas_questionService
    {
        m_questionViewModel GetAllQuesion(UserViewModel user);
        m_questionViewModel FilterQuestion(m_questionViewModel model, UserViewModel user);
        m_questionViewModel GetById(long id, UserViewModel user);
        void Save(m_questionViewModel model, UserViewModel user);
        void UpdateStatus(long id, UserViewModel user);
        List<quest_item> GetQuestItemByRouteID(long route_id,long location_id, UserViewModel user);
        m_questionViewModel GetQuestion(UserViewModel user);
        List<QuestionList> GetQuestionByLocation(int locationId);
    }
}
