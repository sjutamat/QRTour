using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.Domain.ViewModel;

namespace QRT.Domain.Interface.Service
{
    public interface Imas_locquestionService
    {
        List<m_locquestionViewModel> GetAllLocQuestion();
        m_locquestionViewModel GetByLocationId(long id);
        void Save(m_locquestionViewModel model);
        void UpdateStatus(long id);
        m_locquestionViewModel InitailModel();
    }
}
