using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Поломка.db
{ 
    public partial class Client
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
    }
}
