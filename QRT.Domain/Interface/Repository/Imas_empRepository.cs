using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.DB;
using QRT.Domain.ViewModel;

namespace QRT.Domain.Interface.Repository
{
    public interface Imas_empRepository:IRepositoryBase<mas_emp>
    {
        List<EmpData> GetEmployeeByAdminId(long admin_id);
        IQueryable<EmpData> GetEmployeeById(long id, long admin_id);
    }

  
}
