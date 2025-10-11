using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.Timer;
using System;
using System.Drawing;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.Wave
{
    public class RestTimerPresenter : IPresenter
    {
        private RestTimerView _view;
        private TimerService _timer;
        private readonly ViewsFactory _viewFactory;

        private IDisposable _timerDisposable;

        public RestTimerPresenter(RestTimerView view, TimerService timer, ViewsFactory viewFactory)
        {
            _view = view;
            _timer = timer;
            _viewFactory = viewFactory;
        }

        public void Initialize()
        {
            _timerDisposable = _timer.CurrentTime.Subscribe(OnTimerChanged);
        }

        public void Dispose()
        {
            _viewFactory.Release(_view);
            _timerDisposable?.Dispose();
        }

        private void OnTimerChanged(float arg1, float time)
        {
            float koef = time / _timer.Cooldown;

            _view.SetFill(koef);
            _view.SetColor(koef);
        }
    }
}