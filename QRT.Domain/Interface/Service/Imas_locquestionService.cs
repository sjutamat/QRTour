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
        List<m_locquestionViewModel> GetAllLocQuestion(UserViewModel user);
        m_locquestionViewModel GetByLocationId(long id, UserViewModel user);
        void Save(m_locquestionViewModel model, UserViewModel user);
        void UpdateStatus(long id, UserViewModel user);
        m_locquestionViewModel InitailModel(UserViewModel user);
    }
}
