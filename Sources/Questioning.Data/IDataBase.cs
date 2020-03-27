using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questioning.Data
{
    public interface IDataBase
    {
        IDataProvider DataProvider { get; }
    }
}
