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
        m_questionViewModel GetAllQuesion();
        m_questionViewModel FilterQuestion(m_questionViewModel model);
        m_questionViewModel GetById(long id);
        void Save(m_questionViewModel model);
        void UpdateStatus(long id);
        List<quest_item> GetQuestItemByRouteID(long route_id,long location_id);
        m_questionViewModel GetQuestion();
    }
}
