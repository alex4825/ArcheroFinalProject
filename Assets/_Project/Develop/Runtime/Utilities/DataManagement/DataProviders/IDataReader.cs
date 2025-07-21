namespace Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProvoders
{
    public interface IDataReader<TData> where TData: ISaveData
    {
        void ReadFrom(TData data);
    }
}
