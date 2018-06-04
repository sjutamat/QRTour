using QRT.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.Interface.Service
{
    public interface Itrn_hotkeyService
    {
        void SaveHotKey(hotkey model, UserViewModel admin);
        bool chkHotKey(string key, string locationId);
        void UpdateHotKey(string hotkeycode);
        string GetPINByID(long id);
    }
}
