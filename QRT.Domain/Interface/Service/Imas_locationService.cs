using QRT.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.Interface.Service
{
    public interface Imas_locationService
    {
        m_locationViewModel GetAllLocation();
        m_locationViewModel FilterLocation(m_locationViewModel model);
        m_locationViewModel GetRoute();
        string CodeGenerater();
        void Save(m_locationViewModel model);
        void SaveQuestion(m_locquestionViewModel model);
        m_locationViewModel GetById(long id);
        void UpdateStatus(long id);
        List<quest_item> GetQuestion(long route_id,long location_id);
    }
}
