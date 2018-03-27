using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.Domain.Interface.Repository;
using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;
using System.Web;

namespace QRT.Service.Implement
{
    public class mas_adminService : Imas_adminService
    {
        #region Utility
        private readonly Imas_adminRepository _admin;
    
        public mas_adminService(Imas_adminRepository imasadminrepository)
        {
            _admin = imasadminrepository;
        }
        #endregion


        

        public UserViewModel Login(Login vm)
        {
            var adminData = _admin.Login(vm.username,vm.password) ;
            return adminData;
        }
    }
}
