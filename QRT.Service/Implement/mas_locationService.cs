using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.DB;
using QRT.Domain;
using QRT.Domain.Interface.Service;
using QRT.Domain.Interface.Repository;
using Op3ration.ExceptionHandler;
using QRT.Domain.ViewModel;
using QRCoder;
using System.Drawing;

namespace QRT.Service.Implement
{
    public class mas_locationService:Imas_locationService
    {
        #region utility
        private readonly Imas_locationRepository _location;
        private readonly Imas_routeRepository _route;
        private readonly Itrn_transactionRepository _trn;

        private readonly Imas_routeService _routeservice;
        private readonly Imas_questionService _questservice;
        private readonly Imas_locquestionService _locquestservice;
        
        //private ValidateHandler Validator;
        public mas_locationService(Imas_locationRepository imas_locationrepository,
            Imas_routeRepository imas_routerepository,
            Itrn_transactionRepository itrn_TransactionRepository,
            Imas_routeService imasrouteservice,
            Imas_questionService imasquestservice,
            Imas_locquestionService imaslocquestionservice)
        {
            _location = imas_locationrepository;
            _route = imas_routerepository;
            _trn = itrn_TransactionRepository;
            _routeservice = imasrouteservice;
            _questservice = imasquestservice;
            _locquestservice = imaslocquestionservice;
        }


        //private ValidateHandler ValidateModel(m_locationViewModel model)
        //{
        //    Validator = new ValidateHandler();
        //    return Validator;
        //}


        public string CodeGenerater()
        {
            var guidCode = Guid.NewGuid();
            return guidCode.ToString();
        }


        public m_locationViewModel GetRoute(UserViewModel user)
        {
            m_locationViewModel location = new m_locationViewModel();
            location.route = _routeservice.GetRouteItem(user);
            location.qrcode1 = CodeGenerater();
            location.qrcode2 = CodeGenerater();


            return location;
        }
        #endregion

        public m_locationViewModel GetAllLocation(UserViewModel user)
        {
            m_locationViewModel model = new m_locationViewModel();
            var data = _location.Filter(f => f.adminid_create == user.id && f.location_active != "D",inc=> inc.mas_route) .ToList();
            if (data != null)
            {
                var location = data.Select(x => new LocationData()
                {
                    id = x.location_id,
                    title = x.location_title,
                    route_id = x.route_id,
                    description = x.location_desc,
                    seq_number = x.seq_number,
                    status = x.location_active == "A" ? "Active" : "Deactive",
                    created_date = x.location_cdate,
                    route_name = x.mas_route.route_title,
                }).OrderByDescending(c => c.created_date).ToList();
                model.s_location = new SearchDataLocation();
                model.s_locationData = location;
                model.route = _routeservice.GetRouteItem(user);
                return model;
            }
            else
            {
                model.s_locationData = new List<LocationData>();
                return model;

            }
        }

        public m_locationViewModel FilterLocation(m_locationViewModel model, UserViewModel user)
        {
            var id = (model.s_location.id == null) ? 0 : Convert.ToInt32(model.s_location.id);
            var title = model.s_location.title;
            var route = Convert.ToInt32(model.s_location.route);

            var data = _location.Filter(c => c.location_active == "A" && c.adminid_create == user.id, inc => inc.mas_route).ToList();
            if (id!=0)
            {
                data = data.Where(c => c.location_id == id).ToList();
            }

            if (!String.IsNullOrEmpty(title))
            {
                data = data.Where(c => c.location_title.Contains(title)).ToList();
            }

            if (route != 0)
            {
                data = data.Where(c => c.route_id == route).ToList();
            }
                     

            if (data != null)
            {
                var location = data.Select(x => new LocationData()
                {
                    id = x.location_id,
                    title = x.location_title,
                    route_id = x.route_id,
                    description = x.location_desc,
                    seq_number = x.seq_number,
                    status = x.location_active == "A" ? "Active" : "Deactive",
                    created_date = x.location_cdate,
                    route_name = x.mas_route.route_title,
                }).OrderByDescending(c => c.created_date).ToList();

                model.s_locationData = location;
                model.route = _routeservice.GetRouteItem(user);
                return model;
            }
            else
            {
                model.s_locationData = new List<LocationData>();
                return model;

            }
        }

        public m_locationViewModel GetById(long id, UserViewModel user)
        {
            var data = _location.Filter(c => c.location_id == id).SingleOrDefault();
            m_locationViewModel location = new m_locationViewModel();
            location.id = data.location_id;
            location.route_id = data.route_id;
            location.title = data.location_title;
            location.description = data.location_desc;
            location.status = data.location_active == "A" ? "On" : "Off";
            location.seq_number = data.seq_number;
            location.qrcode1 = data.qrcode1;
            location.qrcode2 = data.qrcode2;
            location.code1_status = data.qrcode1_status == "A" ? "On" : "Off";
            location.code2_status = data.qrcode2_status == "A" ? "On" : "Off";

            location.created_date = data.location_cdate;
            location.created_by = data.adminid_create;
            location.route = _routeservice.GetRouteItem(user);

            return location;
        }

        public m_locationViewModel GetLocationById(long id)
        {
            var data = _location.Filter(c => c.location_id == id).SingleOrDefault();
            m_locationViewModel location = new m_locationViewModel();
            location.id = data.location_id;
            location.route_id = data.route_id;
            location.title = data.location_title;
            location.description = data.location_desc;
            location.status = data.location_active == "A" ? "On" : "Off";
            location.seq_number = data.seq_number;
            location.qrcode1 = data.qrcode1;
            location.qrcode2 = data.qrcode2;
            location.code1_status = data.qrcode1_status == "A" ? "On" : "Off";
            location.code2_status = data.qrcode2_status == "A" ? "On" : "Off";

            location.created_date = data.location_cdate;
            location.created_by = data.adminid_create;
           // location.route = _routeservice.GetRouteItem(user);

            return location;
        }

        public void Save(m_locationViewModel model, UserViewModel user)
        {
            if (model.id == 0)
            {
                try
                {
                    mas_location location = new mas_location();
                    location.route_id = model.route_id;
                    location.location_title = model.title;
                    location.location_desc = model.description;
                    location.location_active = model.status == "On" ? "A" : "D";
                    location.seq_number = model.seq_number;
                    location.qrcode1 = model.qrcode1;
                    location.qrcode2 = model.qrcode2;
                    location.qrcode1_status = model.code1_status == "On" ? "A" : "D";
                    location.qrcode2_status = model.code2_status == "On" ? "A" : "D";

                    location.location_cdate = DateTime.Now;
                    location.adminid_create = user.id;
                    location.location_udate = DateTime.Now;
                    location.adminid_update = user.id;
                    _location.Create(location);
                }
                catch (Exception ex)
                {
                    if (ex.IsValidateHandler())
                        throw ex.ToValidateHandler();
                    throw new ValidateHandler(MessageLevel.Error, "Error:'" + ex.Message + "'");
                }
                
            }
            else
            {
               
                var oldData = _location.Filter(c => c.location_id == model.id).SingleOrDefault();
                if (oldData != null)
                {
                    try
                    {
                        oldData.location_title = model.title;
                        oldData.location_desc = model.description;
                        oldData.location_active = model.status == "On" ? "A" : "D";
                        oldData.route_id = model.route_id;
                        oldData.seq_number = model.seq_number;
                        oldData.qrcode1 = model.qrcode1;
                        oldData.qrcode2 = model.qrcode2;
                        oldData.qrcode1_status = model.code1_status == "On" ? "A" : "D";
                        oldData.qrcode2_status = model.code2_status == "On" ? "A" : "D";

                        oldData.location_udate = DateTime.Now;
                        oldData.adminid_update = user.id;
                        _location.Update(oldData);
                    }
                    catch (Exception ex)
                    {
                        if (ex.IsValidateHandler())
                            throw ex.ToValidateHandler();
                        throw new ValidateHandler(MessageLevel.Error, "Error:'" + ex.Message + "'");
                    }

                }
                
            }
        }

        public void SaveQuestion(m_locquestionViewModel model, UserViewModel user)
        {
            _locquestservice.Save(model,user);
        }



        public void UpdateStatus(long id, UserViewModel user)
        {
            var oldData = _location.Filter(c => c.location_id == id).SingleOrDefault();
            try
            {
                oldData.location_active = "D";
                oldData.adminid_update = user.id;
                oldData.location_udate = DateTime.Now;
                _location.Update(oldData);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<quest_item> GetQuestion(long route_id, long location_id, UserViewModel user)
        {
            var data = _questservice.GetQuestItemByRouteID(route_id,location_id,user);
            return data;
        }


        public bool ChkSequentNumber(string locationId)
        {
            var sq = _location.Filter(c => c.qrcode1.Equals(locationId) && c.qrcode1_status.Equals("A")|| c.qrcode2.Equals(locationId) && c.qrcode2_status.Equals("A")).SingleOrDefault().seq_number;
            if (sq!=null)
            {
                if (sq==1)
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


        /// <summary>
        /// for check over the sequent.
        /// </summary>
        /// <param name="locationId">Is guid from scan qrcode</param>
        /// <param name="emp"></param>
        /// <returns></returns>
        public string ChkOverSequentNumber(string locationId ,EmpData emp)
        {
            
            //current location
            var locateionData = _location.Filter(c => c.qrcode1.Equals(locationId) && c.qrcode1_status.Equals("A") || c.qrcode2.Equals(locationId) && c.qrcode2_status.Equals("A")).SingleOrDefault();
            //current location_id is not guid.
           
            var routeId = locateionData.route_id;
            //previous sequent by location id == locationId
            
            var sq = locateionData.seq_number; //current sequent

            //all sequent by location id and employee id

            var sequent = _trn.Filter(c => c.route_id == routeId && c.session_id == emp.id.ToString(), inc => inc.mas_location).OrderByDescending(o => o.transaction_id).
                Select(s => new { s.mas_location.seq_number, s.mas_location.location_title }).FirstOrDefault();
            
            //if sequent is null, This route is not checked.
            int? previous = 0;
            var ls = "";
            if (sequent != null && sequent.seq_number.HasValue)
            {
                previous = sequent.seq_number.Value + 1; //previous sequent
                if (sq != null && sq != 1)
                {
                    if (previous == sq)
                    {
                        ls = null;
                        return ls;
                    }
                    else 
                    {
                        ls = "ไม่สามารถสแกนจุดนี้ได้ เนื่องจากจุดนี้อาจถูกสแกนแล้ว หรือ คุณข้ามการสแกนจุดก่อนหน้า. สถานที่ล่าสุดที่คุณสแกนคือ " + sequent.location_title;
                        return ls;
                    }
                }
                else
                {
                    return null; //not over
                }
                
            }
            else
            {
                if (sq != null && sq == 1)
                {
                    return null;
                }
                else
                {
                    ls = "ไม่สามารถสแกนจุดนี้ได้ เนื่องจากจุดนี้อาจถูกสแกนแล้ว หรือ คุณข้ามการสแกนจุดก่อนหน้า.";
                    return ls;
                }

            }
        }


        public mas_location GetLocationByCode(string code)
        {
            var query = _location.Filter(c => c.qrcode1.Equals(code) && c.qrcode1_status.Equals("A") || c.qrcode2.Equals(code) && c.qrcode2_status.Equals("A")).SingleOrDefault();
            return query;
        }
        //public byte[] GenQRCode()
        //{
        //    string code = "";
        //    QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //    QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
        //    //System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
        //    //imgBarCode.Height = 150;
        //    //imgBarCode.Width = 150;
        //    byte[] byteImage = null;
        //    using (Bitmap bitMap = qrCode.GetGraphic(20))
        //    {
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //            byteImage = ms.ToArray();
        //            //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
        //        }
        //    }
        //    return byteImage;
        //}
    }
}
