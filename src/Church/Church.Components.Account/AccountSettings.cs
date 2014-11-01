using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Church.Common.Settings;

namespace Church.Components.Account
{
    public class AccountSettings : ISetting
    {
        public string Database { get; set; }
    }
}
