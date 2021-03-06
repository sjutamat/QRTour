﻿using System;
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
    public class mas_questionService:Imas_questionService
    {
        #region Utility
        private readonly Imas_questionRepository _question;
        private readonly Imas_locquestionRepository _locquest;
        private readonly Imas_locationRepository _location;

        private readonly Imas_routeService _routeservice;
     
        public mas_questionService(Imas_questionRepository imas_questionrepository
            ,Imas_locquestionRepository imas_locquestionrepository
            ,Imas_locationRepository imas_locationrepository

            ,Imas_routeService imasrouteservice
            )
        {
            _question = imas_questionrepository;
            _locquest = imas_locquestionrepository;
            _location = imas_locationrepository;

            _routeservice = imasrouteservice;
        }

        
        #endregion

        public m_questionViewModel GetAllQuesion(UserViewModel user)
        {
            m_questionViewModel model = new m_questionViewModel();
            var data = _question.Filter(c => c.question_active == "A" && c.adminid_create == user.id, inc => inc.mas_route).ToList();
            if (data != null)
            {
                var question = data.Select(x => new QuestionData()
                {
                    id = x.question_id,
                    title = x.question_title,
                    route_name = x.mas_route.route_title,
                    status = x.question_active == "A" ? "Active" : "Deactive",
                    created_date = x.question_cdate,
                }).OrderByDescending(c => c.created_date).ToList();

                model.s_question = new SearchQuestionData();
                model.s_questionData = question;
                model.route = _routeservice.GetRouteItem(user);
                return model;
            }
            else
            {
                model.s_questionData = new List<QuestionData>();
                return model;

            }
        }

        public m_questionViewModel FilterQuestion(m_questionViewModel model, UserViewModel user)
        {
            var id = (model.s_question.id == null) ? 0 : Convert.ToInt32(model.s_question.id);
            var title = model.s_question.title;
            var route = Convert.ToInt32(model.s_question.route);

            var data = _question.Filter(c => c.question_active == "A" && c.adminid_create == user.id, inc => inc.mas_route).ToList();
            if (id != 0)
            {
                data = data.Where(c => c.question_id == id).ToList();
            }

            if (!String.IsNullOrEmpty(title))
            {
                data = data.Where(c => c.question_title.Contains(title)).ToList();
            }

            if (route != 0)
            {
                data = data.Where(c => c.route_id == route).ToList();
            }

            if (data != null)
            {
                var question = data.Select(x => new QuestionData()
                {
                    id = x.question_id,
                    title = x.question_title,
                    route_name = x.mas_route.route_title,
                    status = x.question_active == "A" ? "Active" : "Deactive",
                    created_date = x.question_cdate,
                }).OrderByDescending(c => c.created_date).ToList();

                model.s_questionData = question;
                model.route = _routeservice.GetRouteItem(user);
                return model;
            }
            else
            {
                model.s_questionData = new List<QuestionData>();
                return model;

            }
        }

        public m_questionViewModel GetById(long id, UserViewModel user)
        {
            var data = _question.Filter(c => c.question_id == id && c.adminid_create == user.id, inc => inc.mas_route).SingleOrDefault();
            m_questionViewModel quest = new m_questionViewModel();
            if (data != null)
            {
                quest.id = data.question_id;
                quest.title = data.question_title;
                quest.route_id = data.route_id;
                quest.description = data.question_desc;
                quest.status = data.question_active == "A" ? "On" : "Off";
                quest.created_date = data.question_cdate;
                quest.created_by = data.adminid_create;
                quest.route = _routeservice.GetRouteItem(user);
            }
            else
            {
                quest.id = id;
            }
            
            return quest;
        }

        public void Save(m_questionViewModel model, UserViewModel user)
        {
            if (model.id == 0)
            {
               
                try
                {
                    mas_question question = new mas_question();
                    question.question_title = model.title;
                    question.route_id = model.route_id;
                    question.question_desc = model.description;
                    question.question_active = model.status == "On" ? "A" : "D";
                    question.question_cdate = DateTime.Now;
                    question.adminid_create = user.id;
                    question.question_udate = DateTime.Now;
                    question.adminid_update = user.id;
                    _question.Create(question);
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
               
                var oldData = _question.Filter(c => c.question_id == model.id).SingleOrDefault();
                if (oldData != null)
                {
                    try
                    {
                        oldData.question_title = model.title;
                        oldData.route_id = model.route_id;
                        oldData.question_desc = model.description;
                        oldData.question_active = model.status == "On" ? "A" : "D";
                        oldData.question_udate = DateTime.Now;
                        oldData.adminid_update = user.id;
                        _question.Update(oldData);
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

        public void UpdateStatus(long id, UserViewModel user)
        {
            var oldData = _question.Filter(c => c.question_id == id).SingleOrDefault();
            try
            {
                oldData.question_active = "D";
                oldData.question_udate = DateTime.Now;
                oldData.adminid_update = user.id;
                _question.Update(oldData);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<quest_item> GetQuestItemByRouteID(long route_id, long location_id, UserViewModel user)
        {
            List<quest_item> itemRoute = new List<quest_item>();
            var routeData = _question.Filter(c =>c.route_id == route_id && c.question_active == "A" && c.adminid_create == user.id).ToList();
            if (routeData != null)
            {
                foreach (var item in routeData)
                {
                    quest_item r = new quest_item();
                    r.id = item.question_id;
                    r.text = item.question_title;
                    r.set_select = "";
                    itemRoute.Add(r);
                }
            }

            var lq = _locquest.Filter(c => c.location_id == location_id && c.locquestion_active == "A" && c.adminid_create == user.id).ToList();
            if (lq != null && lq.Count != 0)
            {
               foreach (var item in itemRoute)
               {
                   for (int i = 0; i < lq.Count; i++)
                   {
                        if (item.id == lq[i].question_id)
                        {
                            item.set_select = "selected";
                        }
                        
                   }
                   
               }
                
            }
            return itemRoute;
        }

        public m_questionViewModel GetQuestion(UserViewModel user)
        {
            m_questionViewModel model = new m_questionViewModel();
            model.route = _routeservice.GetRouteItem(user);
            return model;
        }

        public List<QuestionList> GetQuestionByLocation(string locationcode)
        {
            var jah = _question.QuestionByLocation(locationcode);
            //var d = jah.Where(c => c.location_id.Equals(locationcode)).ToList();
            List<QuestionList> data = new List<QuestionList>();
            return jah;
        }
    }
}
