using QRT.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.Interface.Service
{
    public interface IDashboardService
    {
        List<answerHeader> GetAnswer(int admin_id);
        List<answerHeader> GetAnswerFilter(dashboardViewModel model, int admin_id);
    }
}
