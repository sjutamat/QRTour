using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.DB;
using QRT.Domain.Interface.Repository;

namespace QRT.DAL.Implement
{
    public class mas_viewerRepository : RepositoryBase<mas_user>, Imas_viewerRepository
    {
        private readonly QRCodeTourEntities _dbContext;
        public mas_viewerRepository(QRCodeTourEntities _Context) : base(_Context)
        {
            _dbContext = _Context;
        }
    }
}
