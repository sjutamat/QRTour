using Op3ration.ExceptionHandler;
using QRT.DB;
using QRT.Domain.Interface.Repository;
using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Service.Implement
{
    public class trn_hotkeyService : Itrn_hotkeyService
    {
        #region Utility
        private readonly Itrn_hotkeyRepository _hotkey;
        private readonly Imas_locationRepository _location;
        public trn_hotkeyService(
           Itrn_hotkeyRepository itrnhotkeyrepository
            , Imas_locationRepository imaslocationrepository
            )
        {
            _hotkey = itrnhotkeyrepository;
            _location = imaslocationrepository;
        }
        #endregion

        public void SaveHotKey(hotkey model, UserViewModel admin)
        {
            // start.AddMinutes(20) > DateTime.UtcNow;
            try
            {
                var start = DateTime.Now;
                trn_hotkey hk = new trn_hotkey();
                hk.hotkey_code = model.keycode.ToString();
                hk.hotkey_cdate = start;
                hk.hotkey_expiredate = start.AddMinutes(5);
                hk.hotkey_active = "A";
                hk.admin_id = admin.id;
                _hotkey.Create(hk);
            }
            catch (Exception ex)
            {
                if (ex.IsValidateHandler())
                    throw ex.ToValidateHandler();
                throw new ValidateHandler(MessageLevel.Error, "Error:'" + ex.Message + "'");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">4 digit</param>
        /// <param name="locationId">GUID from QRCode</param>
        /// <returns>True = HotKey is Valid, False = HotKey is Invalid</returns>
        public bool chkHotKey(string key, string locationId)
        {
            var loc = _location.Filter(c => c.qrcode1.Equals(locationId) && c.qrcode1_status.Equals("A") || c.qrcode2.Equals(locationId) && c.qrcode2_status.Equals("A")).SingleOrDefault(); 
            var data = _hotkey.Filter(c => c.hotkey_active == "A" && c.hotkey_code == key).SingleOrDefault();
            if (data != null && loc !=null)
            {
                if (loc.adminid_create == data.admin_id || data.hotkey_expiredate > DateTime.Now)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public void UpdateHotKey(string hotkeycode)
        {
            if (!string.IsNullOrEmpty(hotkeycode) && hotkeycode != "0")
            {
                var oldData = _hotkey.Filter(c => c.hotkey_code == hotkeycode).SingleOrDefault();
                try
                {
                    oldData.hotkey_active = "D";
                    oldData.hotkey_usedate = DateTime.Now;
                    _hotkey.Update(oldData);
                    //_comp.Delete(oldData);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }


        public string GetPINByID(long id)
        {
            return "";
        }
    }
}
