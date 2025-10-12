using Assets._Project.Develop.Runtime.Meta.Sound;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.ResultsPopups
{
    public class WinPopupPresenter : PopupPresenterBase
    {
        private const string TitleName = "YOU WIN!";

        private readonly WinPopupView _view;
        private readonly SceneSwitcherService _sceneSwitcher;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly SoundLauncher _soundLauncher;

        public WinPopupPresenter(
            ICoroutinesPerformer coroutinesPerformer,
            WinPopupView view,
            SceneSwitcherService sceneSwitcher,
            SoundLauncher soundLauncher) : base(coroutinesPerformer)
        {
            _coroutinesPerformer = coroutinesPerformer;
            _view = view;
            _sceneSwitcher = sceneSwitcher;
            _soundLauncher = soundLauncher;
        }

        protected override PopupViewBase PopupView => _view;

        public override void Initialize()
        {
            base.Initialize();

            _view.SetTitle(TitleName);

            _view.ContinueClicked += OnContinueClicked;
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            _view.ContinueClicked -= OnContinueClicked;
        }

        public override void Dispose()
        {
            base.Dispose();

            _view.ContinueClicked -= OnContinueClicked;
        }

        private void OnContinueClicked()
        {
            _soundLauncher.PlayClickSound();

            _coroutinesPerformer.StartPerform(_sceneSwitcher.ProcessSwitchTo(Scenes.MainMenu));
            OnCloseRequest();
        }
    }
}