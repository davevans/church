using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Church.Components.Core.Model;

namespace Church.Components.Core.Repository
{
    interface IPersonRepository
    {
        IEnumerable<Person> GetByChurchId(int churchId);
        Person GetById(long personId);
    }
}
