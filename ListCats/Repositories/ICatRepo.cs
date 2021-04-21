using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListCats.Repositories
{
    public interface ICatRepo
    {
        public Task<string> ProcessRepositories();
    }
}
