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

        public AdminViewModel GetById(UserViewModel user)
        {
            var query = _admin.Find(c => c.admin_id == user.id);
            AdminViewModel data = new AdminViewModel();
            if (query !=null)
            {
                data.id = query.admin_id;
                data.user = query.admin_user;
                data.password = query.admin_password;
                data.created_date = query.admin_cdate;
                data.status = query.admin_active;
                //data.company_id = query.company_id;
            }
            return data;
        }


        public string GetAdminName(int id)
        {
            var adminData = _admin.Filter(c => c.admin_id == id).Select(s=>s.admin_user).SingleOrDefault();
            return adminData;
        }
    }
}
