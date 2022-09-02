using System;
using System.Collections.Generic;
using System.Data.Common;
using Questioning.Data.Repositories;
using QuestioningLibrary;

namespace Questioning.Data.MySql.Repositories.Catalog
{
    public class QuestionBlocksRepository : Repository, IRepository<QuestionBlock>

    {
        public QuestionBlocksRepository(IDataProvider dataProvider) : base(dataProvider)
        {
        }

        public QuestionBlock MapObject(DbDataReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            
            return new QuestionBlock()
            {
                Id = (int) reader["id"],
                Title = reader["name"].ToString(),
                ShortName = reader["description"].ToString(),
                IdDirection = (int)reader["idDirections"]
            };
        }

        public List<QuestionBlock> GetAll()
        {
            throw new NotImplementedException();
        }

        public QuestionBlock GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
