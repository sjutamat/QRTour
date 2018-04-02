using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.Domain.ViewModel;

namespace QRT.Domain.Interface.Service
{
    public interface Itrn_answerService
    {
        void SaveAnswer(List<Answer> model, EmpData emp);
    }
}
