using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Поломка.db;
using Поломка.mvvm;

namespace Поломка.ViewModels
{
    public class ClientVisitsViewModel : BaseViewModel
    {
        public List<ViewVisit> Visits { get; set; }

        public ClientVisitsViewModel(Client selectedClient)
        {
            Visits = selectedClient.ClientService.Select(s => new ViewVisit
            {
                Service = s.Service.Title,
                Date = s.StartTime,
                CountFiles = $"всего файлов: {s.DocumentByService.Count}"
            }).ToList();
        }
    }
}
