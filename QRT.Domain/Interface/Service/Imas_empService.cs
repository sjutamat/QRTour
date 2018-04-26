using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.Domain.ViewModel;

namespace QRT.Domain.Interface.Service
{
    public interface Imas_empService
    {
        m_empViewModel GetAllEmp(UserViewModel user);
        m_empViewModel FilterEmp(m_empViewModel model);
        m_empViewModel GetById(long id, UserViewModel user);
        void Save(m_empViewModel model);
        void UpdateStatus(long id);
        m_empViewModel GetEmp(UserViewModel user);
        string GetNewCode(long compid);
        EmpData CheckEmp(string emp_code, string pw);
    }
}
