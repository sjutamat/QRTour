using QRT.DB;
using QRT.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.Interface.Service
{
    public interface Imas_locationService
    {
        m_locationViewModel GetAllLocation(UserViewModel user);
        m_locationViewModel FilterLocation(m_locationViewModel model, UserViewModel user);
        m_locationViewModel GetRoute(UserViewModel user);
        string CodeGenerater();
        void Save(m_locationViewModel model, UserViewModel user);
        void SaveQuestion(m_locquestionViewModel model, UserViewModel user);
        m_locationViewModel GetById(long id, UserViewModel user);
        m_locationViewModel GetLocationById(long id);
        void UpdateStatus(long id, UserViewModel user);
        List<quest_item> GetQuestion(long route_id,long location_id, UserViewModel user);
        //byte[] GenQRCode();
        bool ChkSequentNumber(string locationId);
        string ChkOverSequentNumber(string locationId, EmpData emp);
        mas_location GetLocationByCode(string code);
        bool ChkQRCodeActive(string locationId);
    }
}
