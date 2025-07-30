using DG.Tweening;

namespace Assets._Project.Develop.Runtime.UI.Core
{
    public interface IShovableView : IView
    {
        Tween Show();

        Tween Hide();
    }
}
