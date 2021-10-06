using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Поломка.db
{
    public partial class Tag
    {
        public string ColorForXaml { get => '#' + Color; }
    }
}
