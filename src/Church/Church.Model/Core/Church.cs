using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Model.Core
{
    public class Church
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual TimeZone TimeZone { get; set; }
    }
}
