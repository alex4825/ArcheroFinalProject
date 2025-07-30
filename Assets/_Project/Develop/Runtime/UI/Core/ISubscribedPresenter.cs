namespace Assets._Project.Develop.Runtime.UI.Core
{
    public interface ISubscribedPresenter : ISubscribedPresenter
    {
        void Subscribe();

        void Unsubscribe();
    }
}
