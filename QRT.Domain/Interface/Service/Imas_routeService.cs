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
        m_routeViewModel GetRoute(UserViewModel user);
        m_routeViewModel GetAllRoute(UserViewModel user);
        void Save(m_routeViewModel model, UserViewModel user);
        m_routeViewModel GetById(long id, UserViewModel user);
        m_routeViewModel FilterRoute(m_routeViewModel model, UserViewModel user);
        void UpdateStatus(long id, UserViewModel user);
        List<route_item> GetRouteItem(UserViewModel user);
    }
}
