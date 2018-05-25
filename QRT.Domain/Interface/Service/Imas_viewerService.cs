using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.Domain.ViewModel;

namespace QRT.Domain.Interface.Service
{
    public interface Imas_viewerService
    {
        m_viewerViewModel GetAllReportViewer(UserViewModel user);
        m_viewerViewModel GetById(int id, UserViewModel user);
        string ChkDuplicate(m_viewerViewModel model);
        void Save(m_viewerViewModel model, UserViewModel user);
    }
}
