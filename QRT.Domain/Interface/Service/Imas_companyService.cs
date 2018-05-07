using QRT.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.Interface.Service
{
    public interface Imas_companyService
    {
        List<comp_item> GetCompany();
        comp_item GetById(long id);
        List<comp_item> GetCompanyByAdmin(long adminId);
        m_companyViewModel GetAllCompany(UserViewModel user);
        m_companyViewModel GetFilterCompany(m_companyViewModel model, UserViewModel user);
        void Save(m_companyViewModel model, UserViewModel user);
        m_companyViewModel GetCompanyById(long id, UserViewModel user);
        void UpdateStatus(long id, UserViewModel user);
    }
}
