using System.Collections.Generic;
using Church.Common.Service;
using Church.Components.Core.Model;

namespace Church.Components.Core
{
    public interface IPersonService : IService
    {
        IEnumerable<Person> GetPeopleByChurchId(int churchId);
    }
}
