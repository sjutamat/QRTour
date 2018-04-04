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
        private ValidateHandler Validator;
        public trn_answerService(Itrn_answerRepository itrn_answerrepository)
        {
            _answer = itrn_answerrepository;
        }


        private ValidateHandler ValidateModel(List<Answer> model)
        {
            Validator = new ValidateHandler();
            return Validator;
        }
        #endregion


        public void SaveAnswer(List<Answer> model, EmpData emp)
        {
            if (model != null)
            {
                try
                {
                    for (int i = 0; i < model.Count(); i++)
                    {
                        trn_answer answer = new trn_answer();
                        answer.emp_id = emp.id;
                        answer.location_id = Convert.ToInt32(model[i].location_id);
                        answer.question_id = Convert.ToInt32(model[i].question_id);
                        answer.answer_flag = model[i].answer_flag;
                       // answer.answer_txt = model[i].answer_text;
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
