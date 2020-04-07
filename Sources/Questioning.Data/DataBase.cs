namespace Questioning.Data
{
    public abstract class DataBase : IDataBase
    {
        public abstract IDataProvider DataProvider { get; }
    }
}
