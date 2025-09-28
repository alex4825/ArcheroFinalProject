using Assets._Project.Develop.Runtime.Gameplay.Waves;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using System;

namespace Assets._Project.Develop.Runtime.UI.Statistics
{
    public class WaveCountPresenter : IPresenter
    {
        private readonly GameplayWaveContext _gameplayWaveContext;
        private readonly IconTextView _waveCountView;
        private int _maxWavesCount;

        private IDisposable _waveNumberDisposable;

        public WaveCountPresenter(GameplayWaveContext gameplayWaveContext, IconTextView waveCountView, int maxWavesCount)
        {
            _gameplayWaveContext = gameplayWaveContext;
            _waveCountView = waveCountView;
            _maxWavesCount = maxWavesCount;
        }

        public void Initialize()
        {
            UpdateView(_gameplayWaveContext.CurrentWaveNumber.Value);
            _waveNumberDisposable = _gameplayWaveContext.CurrentWaveNumber.Subscribe(OnWaveNumberChanged);
        }

        public void Dispose()
        {
            _waveNumberDisposable.Dispose();
        }

        private void OnWaveNumberChanged(int arg1, int newNumber) => UpdateView(newNumber);

        private void UpdateView(int newNumber) => _waveCountView.SetText(newNumber.ToString() + "/" + _maxWavesCount.ToString());
    }
}