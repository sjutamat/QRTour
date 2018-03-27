using QRT.DB;
using QRT.Domain.Interface.Repository;
using QRT.Domain.ViewModel;
using System.Linq;

namespace QRT.DAL.Implement
{
    public class mas_adminRepository:RepositoryBase<mas_admin>,Imas_adminRepository
    {
        private readonly QRCodeTourEntities _dbContext;
        public mas_adminRepository(QRCodeTourEntities _Context) : base(_Context)
        {
            _dbContext = _Context;
        }

        public UserViewModel Login(string username, string password)
        {
            var query = Filter(f => f.admin_user.Equals(username) && f.admin_password.Equals(password)).ToList()
                .Select(s => new UserViewModel()
                {
                    username = s.admin_user,
                    id = s.admin_id
                }).FirstOrDefault();
            return query;
        }
    }
}
