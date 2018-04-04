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
    }
}
