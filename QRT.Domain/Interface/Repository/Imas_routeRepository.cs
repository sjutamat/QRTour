using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.DB;

namespace QRT.Domain.Interface.Repository
{
    public interface Imas_routeRepository : IRepositoryBase<mas_route>
    {
        List<mas_route> GetRouteByCompanmy(long? companyId);
    }
}
