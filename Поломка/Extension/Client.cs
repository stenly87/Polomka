using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Поломка.db
{ 
    public partial class Client : IDataErrorInfo
    {

        public int CountVisit { get => ClientService.Count; }

        public DateTime? LastVisit { 
            get {
                if (ClientService.Count() > 0)
                    return ClientService.Max(x => x.StartTime);
                else
                    return null;
            } 
        }

        public System.Windows.Visibility NewClient
        {
            get => ID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
        }

        public bool Man
        {
            get => GenderCode == "м";
            set {
                if (value)
                    GenderCode = "м";
            }
        }
        public bool Woman
        {
            get => GenderCode == "ж";
            set {
                if (value)
                    GenderCode = "ж";
            }
        }

        public string this[string columnName]
        { 
            get
            {
                string error = null;
                if (columnName == "LastName")
                {
                    if (LastName?.Length > 50)
                        error = "Длина ФИО не должна превышать 50 символов";
                }
                else if (columnName == "FirstName")
                {
                    if (FirstName?.Length > 50)
                        error = "Длина ФИО не должна превышать 50 символов";
                }
                else if (columnName == "Patronymic")
                {
                    if (Patronymic?.Length > 50)
                        error = "Длина ФИО не должна превышать 50 символов";
                }
                return error;
            }
        }

        public string Error => throw new NotImplementedException();
    }
}
