using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.DB;
using QRT.Domain.Interface.Service;
using QRT.Domain.Interface.Repository;
using Op3ration.ExceptionHandler;
using QRT.Domain.ViewModel;

namespace QRT.Service.Implement
{
    public class mas_locquestionService : Imas_locquestionService
    {
        #region Utility
        private readonly Imas_locquestionRepository _locquestion;
        private readonly Imas_locationRepository _location;
        private readonly Imas_questionRepository _question;
        private ValidateHandler Validator;
        public mas_locquestionService(Imas_locationRepository imas_locationrepository, 
            Imas_questionRepository imas_questionrepository,
            Imas_locquestionRepository imas_locquestionRepository)
        {
            _location = imas_locationrepository;
            _question = imas_questionrepository;
            _locquestion = imas_locquestionRepository;
        }


        private ValidateHandler ValidateModel(m_locquestionViewModel model)
        {
            Validator = new ValidateHandler();
            return Validator;
        }
        #endregion

        public List<m_locquestionViewModel> GetAllLocQuestion(UserViewModel user)
        {
            var d = _locquestion.Filter(c => c.locquestion_active != "D", inc => inc.mas_location, i => i.mas_question).ToList().GroupBy(c => c.location_id);
            var data = _locquestion.Filter(c => c.locquestion_active != "D", inc => inc.mas_location, i => i.mas_question).ToList();
            if (data != null)
            {
                var locquest = data.Select(x => new m_locquestionViewModel()
                {
                    id = x.locquestion_id,
                    location_name = x.mas_location.location_title,
                    question_name = x.mas_question.question_title,
                    status = x.locquestion_active == "A" ? "Active" : "Deactive",
                    created_date = x.locquestion_cdate
                }).OrderByDescending(c => c.created_date).ToList();

                return locquest;
            }
            else
            {
                List<m_locquestionViewModel> locquest = new List<m_locquestionViewModel>();
                return locquest;

            }
        }

        public m_locquestionViewModel GetByLocationId(long loccationid, UserViewModel user)
        {
            var data = _locquestion.Filter(c => c.location_id == loccationid && c.adminid_create == user.id).SingleOrDefault();
            m_locquestionViewModel locquest = new m_locquestionViewModel();
            locquest.id = data.locquestion_id;
            locquest.location_id = data.location_id;
            locquest.question_id = data.question_id;


            //get location
            locquest.location = new List<location_item>();
            var locitem = _location.Filter(c => c.location_active != "D").ToList();
            if (locitem != null)
            {
                foreach (var item in locitem)
                {
                    location_item c = new location_item();
                    c.id = item.location_id;
                    c.text = item.location_title;
                    locquest.location.Add(c);
                }
            }

            //get question
            locquest.question = new List<question_item>();
            var questitem = _question.Filter(c => c.question_active != "D").ToList();
            if (questitem != null)
            {
                foreach (var item in questitem)
                {
                    question_item q = new question_item();
                    q.id = item.question_id;
                    q.text = item.question_title;
                    locquest.question.Add(q);
                }
            }

            return locquest;
        }

        public void Save(m_locquestionViewModel model, UserViewModel user)
        {
            if (model.location_id != null || model.location_id != 0)
            {
               var hasdata =  _locquestion.Filter(c => c.location_id == model.location_id).Any();
                if (hasdata == true)
                {
                    _locquestion.Delete(c => c.location_id == model.location_id);


                    Validator = ValidateModel(model);
                    if (Validator.HasError())
                    {
                        throw Validator;
                    }
                    else
                    {
                        try
                        {
                            if (model.question_arr != null && model.question_arr.Any())
                            {
                                for (int i = 0; i < model.question_arr.Count(); i++)
                                {
                                    mas_locquestion locquest = new mas_locquestion();
                                    locquest.question_id = Convert.ToInt32(model.question_arr[i]);
                                    locquest.location_id = model.location_id;
                                    locquest.locquestion_active = "A";
                                    locquest.adminid_create = user.id;
                                    locquest.locquestion_cdate = DateTime.Now;
                                    _locquestion.Create(locquest);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            if (ex.IsValidateHandler())
                                throw ex.ToValidateHandler();
                            throw new ValidateHandler(MessageLevel.Error, "Error:'" + ex.Message + "'");
                        }
                    }
                }
                else
                {
                    Validator = ValidateModel(model);
                    if (Validator.HasError())
                    {
                        throw Validator;
                    }
                    else
                    {
                        try
                        {
                            if (model.question_arr != null && model.question_arr.Any())
                            {
                                for (int i = 0; i < model.question_arr.Count(); i++)
                                {
                                    mas_locquestion locquest = new mas_locquestion();
                                    locquest.question_id = Convert.ToInt32(model.question_arr[i]);
                                    locquest.location_id = model.location_id;
                                    locquest.locquestion_active = "A";
                                    locquest.adminid_create = user.id;
                                    locquest.locquestion_cdate = DateTime.Now;
                                    _locquestion.Create(locquest);
                                }
                            }

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
            
        }

        public void UpdateStatus(long id, UserViewModel user)
        {
            var oldData = _locquestion.Filter(c => c.locquestion_id == id).SingleOrDefault();
            try
            {
                oldData.locquestion_active = "D";
                _locquestion.Update(oldData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public m_locquestionViewModel InitailModel(UserViewModel user)
        {
            m_locquestionViewModel model = new m_locquestionViewModel();
            //get location
            model.location = new List<location_item>();
            var locitem = _location.Filter(c => c.location_active != "D" && c.adminid_create == user.id).ToList();
            if (locitem != null)
            {
                foreach (var item in locitem)
                {
                    location_item c = new location_item();
                    c.id = item.location_id;
                    c.text = item.location_title;
                    model.location.Add(c);
                }
            }

            //get question
            model.question = new List<question_item>();
            var questitem = _question.Filter(c => c.question_active != "D" && c.adminid_create == user.id).ToList();
            if (questitem != null)
            {
                foreach (var item in questitem)
                {
                    question_item q = new question_item();
                    q.id = item.question_id;
                    q.text = item.question_title;
                    model.question.Add(q);
                }
            }
            return model;
        }
    }
}
