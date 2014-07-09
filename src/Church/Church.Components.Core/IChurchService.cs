using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Components.Core
{
    public interface IChurchService
    {
        Church.Model.Core.Church GetById(int churchId);
    }
}
