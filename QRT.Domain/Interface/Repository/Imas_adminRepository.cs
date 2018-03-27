using QRT.DB;
using QRT.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.Interface.Repository
{
    public interface Imas_adminRepository:IRepositoryBase<mas_admin>
    {
        UserViewModel Login(string username, string password);
    }
}
