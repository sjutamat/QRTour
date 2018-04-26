using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Op3ration.ExceptionHandler;
using QRT.Domain.Interface.Repository;
using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;
using QRT.DB;
using System.Web;

namespace QRT.Service.Implement
{
    class trn_answerService :Itrn_answerService
    {
        #region Utility
        private readonly Itrn_answerRepository _answer;
        private readonly Itrn_transactionRepository _trn;
        private readonly Imas_locationRepository _location;
        private readonly Imas_routeRepository _route;

        private ValidateHandler Validator;
        public trn_answerService(Itrn_answerRepository itrn_answerrepository,
            Imas_locationRepository imas_locationrepository,
            Imas_routeRepository imas_routerepository,
            Itrn_transactionRepository itrn_transactionrepository
            )
        {
            _answer = itrn_answerrepository;
            _location = imas_locationrepository;
            _route = imas_routerepository;
            _trn = itrn_transactionrepository;
        }


        private ValidateHandler ValidateModel(List<Answer> model)
        {
            Validator = new ValidateHandler();
            return Validator;
        }
        #endregion


        //public void SaveAnswer(List<Answer> model, EmpData emp)
        public void SaveAnswer(answer model, EmpData emp)
        {
            if (model != null)
            {
                try
                {
                    List<trn_answer> ans = new List<trn_answer>();
                    var lid = Convert.ToInt32(model.answerlist[0].location_id);
                    var locationData = _location.Filter(c => c.location_id == lid).Select(s => new
                    {
                        r_id = s.route_id,
                        sq = s.seq_number
                    }).SingleOrDefault();

                    //var route_id = _location.Filter(c => c.location_id == lid).SingleOrDefault().route_id;
                    var route_id = locationData.r_id;
                    var sequent = locationData.sq;
                    long? round_number = 0;
                    if (sequent == 1)
                    {
                        var trn_round_number = _trn.All().OrderByDescending(o => o.round_number).Select(s => s.round_number).FirstOrDefault();
                        if (trn_round_number != null)
                        {
                            round_number = trn_round_number + 1;
                        }
                    }
                    else
                    {
                        round_number = _trn.Filter(c => c.route_id == route_id && c.emp_id == emp.id).OrderByDescending(o => o.round_number).FirstOrDefault().round_number;
                    }
                    var getflag = model.answerlist.Where(c => c.answer_flag == "No").ToList();

                    bool flag = true;
                    if (getflag.Count() > 0)
                    {
                        flag = false;
                    }

                    trn_transaction trn = new trn_transaction();
                    trn.transaction_comment = model.answer_text;
                    trn.session_id = emp.id.ToString();
                    trn.transaction_cdate = DateTime.Now;
                    trn.transaction_answer = flag; //true = yes, false = no
                    trn.location_id = Convert.ToInt32(model.answerlist[0].location_id);
                    trn.route_id = route_id;
                    trn.round_number = round_number;
                    trn.emp_id = emp.id;
                    _trn.Create(trn);

                    var trn_id = _trn.All().OrderByDescending(c=>c.transaction_id).First().transaction_id;

                    for (int i = 0; i < model.answerlist.Count(); i++)
                    {
                        trn_answer answer = new trn_answer();
                        answer.emp_id = emp.id;
                        answer.transaction_id = trn_id;
                        answer.location_id = Convert.ToInt32(model.answerlist[i].location_id);
                        answer.question_id = Convert.ToInt32(model.answerlist[i].question_id);
                        //answer.answer_flag = model[i].answer_flag;
                        answer.answer_txt = model.answerlist[i].answer_flag.TrimStart().TrimEnd();
                        answer.answer_cdate = DateTime.Now;
                        answer.session_id = HttpContext.Current.Session.SessionID;
                        _answer.Create(answer);
                    }
                }
                catch(Exception ex)
                {
                    if (ex.IsValidateHandler())
                        throw ex.ToValidateHandler();
                    throw new ValidateHandler(MessageLevel.Error, "Error:'" + ex.Message + "'");
                }

                
            }
        }

        
    }
}
