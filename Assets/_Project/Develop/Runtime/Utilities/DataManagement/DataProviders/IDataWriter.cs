namespace Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProvoders
{
    public interface IDataWriter<TData> where TData: ISaveData
    {
        void WriteTo(TData data);
    }
}
