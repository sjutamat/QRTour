using QRT.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.Interface.Service
{
    public interface Imas_routeService
    {
        m_routeViewModel GetRoute();
        m_routeViewModel GetAllRoute();
        void Save(m_routeViewModel model);
        m_routeViewModel GetById(long id);
        m_routeViewModel FilterRoute(m_routeViewModel model);
        void UpdateStatus(long id);
        List<route_item> GetRouteItem();
    }
}
