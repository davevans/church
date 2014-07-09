using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Components.Core
{
    public class ChurchService : IChurchService
    {
        private Repository.ICoreRepository _coreRepository;
        public ChurchService(Repository.ICoreRepository coreRepository)
        {
            _coreRepository = coreRepository;
        }

        public Model.Core.Church GetById(int churchId)
        {
            return _coreRepository.GetById(churchId);
        }
    }
}
