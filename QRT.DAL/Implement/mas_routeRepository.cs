using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.Domain.Interface.Repository;
using QRT.DB;
using System.Data.Entity;

namespace QRT.DAL.Implement
{
    public class mas_routeRepository : RepositoryBase<mas_route>, Imas_routeRepository
    {
        private readonly QRCodeTourEntities _dbContext;
        public mas_routeRepository(QRCodeTourEntities _Context) : base(_Context)
        {
            _dbContext = _Context;
        }


        public List<mas_route> GetRouteByCompanmy(long? companyId)
        {
            var data = _dbContext.mas_route.Where(c => c.company_id.Equals(companyId)).ToList();
            return data;
        }
    }
}
