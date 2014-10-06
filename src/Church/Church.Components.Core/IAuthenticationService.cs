using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Components.Core
{
    public interface IAuthenticationService
    {
        bool Authenticate(string username, string password);
    }
}
