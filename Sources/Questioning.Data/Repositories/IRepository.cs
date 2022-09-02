using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Questioning.Data.Repositories
{
    public interface IRepository<T>
    {
        T MapObject(DbDataReader reader);
        List<T> GetAll();
        T GetById(Guid id);
    }
}
