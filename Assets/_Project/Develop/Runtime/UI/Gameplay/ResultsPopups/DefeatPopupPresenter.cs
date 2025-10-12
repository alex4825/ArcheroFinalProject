using Assets._Project.Develop.Runtime.Infrastracture.Gameplay.Infrastracture;
using Assets._Project.Develop.Runtime.Meta.Sound;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.ResultsPopups
{
    public class DefeatPopupPresenter : PopupPresenterBase
    {
        private const string TitleName = "YOU LOOSE!";

        private readonly DefeatPopupView _view;
        private readonly SceneSwitcherService _sceneSwitcher;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly GameplayInputArgs _currentLevelArgs;
        private readonly SoundLauncher _soundLauncher;

        public DefeatPopupPresenter(
            ICoroutinesPerformer coroutinesPerformer,
            DefeatPopupView view,
            SceneSwitcherService sceneSwitcher,
            GameplayInputArgs currentLevelArgs,
            SoundLauncher soundLauncher) : base(coroutinesPerformer)
        {
            _coroutinesPerformer = coroutinesPerformer;
            _view = view;
            _sceneSwitcher = sceneSwitcher;
            _currentLevelArgs = currentLevelArgs;
            _soundLauncher = soundLauncher;
        }

        protected override PopupViewBase PopupView => _view;

        public override void Initialize()
        {
            base.Initialize();

            _view.SetTitle(TitleName);

            _view.ContinueClicked += OnContinueClicked;
            _view.RestartClicked += OnRestartClicked;
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            _view.ContinueClicked -= OnContinueClicked;
            _view.RestartClicked -= OnRestartClicked;
        }

        public override void Dispose()
        {
            base.Dispose();

            _view.ContinueClicked -= OnContinueClicked;
            _view.RestartClicked -= OnRestartClicked;
        }

        private void OnContinueClicked()
        {
            _soundLauncher.PlayClickSound();

            _coroutinesPerformer.StartPerform(_sceneSwitcher.ProcessSwitchTo(Scenes.MainMenu));
            OnCloseRequest();
        }

        private void OnRestartClicked()
        {
            _soundLauncher.PlayClickSound();

            _coroutinesPerformer.StartPerform(_sceneSwitcher.ProcessSwitchTo(Scenes.Gameplay, new GameplayInputArgs(_currentLevelArgs.LevelNumber)));
            OnCloseRequest();
        }
    }
}