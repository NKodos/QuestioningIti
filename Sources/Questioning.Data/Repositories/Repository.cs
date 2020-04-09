using System;

namespace Questioning.Data.Repositories
{
    public class Repository
    {
        protected readonly IDataProvider DataProvider;

        public Repository(IDataProvider dataProvider)
        {
            DataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }
    }
}
