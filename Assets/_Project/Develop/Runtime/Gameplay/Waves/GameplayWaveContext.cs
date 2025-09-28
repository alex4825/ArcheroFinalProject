using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Waves
{
    public class GameplayWaveContext : IDisposable, IUpdatable
    {
        private Wave _currentWave;

        private ReactiveEvent<WaveResult> _currentWaveEnded = new();
        private ReactiveVariable<int> _currentWaveNumber = new();

        private IDisposable _waveEndedDisposable;

        public IReadonlyEvent<WaveResult> CurrentWaveEnded => _currentWaveEnded;

        public IReadonlyVariable<int> CurrentWaveNumber => _currentWaveNumber;

        public int WavesPassed { get; private set; }

        public void Set(Wave wave)
        {
            if (_currentWave != null)
                throw new InvalidOperationException("Wave is already running");

            _currentWave = wave;
            _currentWaveNumber.Value++;

            _waveEndedDisposable = _currentWave.Ended.Subscribe(OnCurrentWaveEnded);
        }

        public void Update(float deltaTime)
        {
            _currentWave?.Update(deltaTime);
        }

        public void Dispose()
        {
            _waveEndedDisposable?.Dispose();
            _currentWave?.Dispose();
            _currentWave = null;

            WavesPassed = 0;
            _currentWaveNumber.Value = 0;
        }

        private void OnCurrentWaveEnded(WaveResult result)
        {
            _currentWave.Dispose();
            _currentWave = null;
            _currentWaveEnded?.Invoke(result);
            _waveEndedDisposable?.Dispose();

            WavesPassed++;
        }
    }
}