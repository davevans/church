using System.Collections.Generic;
using Church.Components.Core.Model;

namespace Church.Components.Core.Repository
{
    public interface IPersonRepository
    {
        IEnumerable<Person> GetByChurchId(int churchId);
        Person GetById(long personId);
    }
}
