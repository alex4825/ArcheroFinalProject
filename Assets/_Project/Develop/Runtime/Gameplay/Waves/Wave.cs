using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.Enemies;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;

namespace Assets._Project.Develop.Runtime.Gameplay.Waves
{
    public class Wave : IDisposable
    {
        private readonly EntitiesBrainsFactory _entitiesBrainsFactory;
        private WaveConfig _waveConfig;

        private List<EntityConfig> _enemyConfigs;
        private Vector3 _fortressPosition;
        private Entity _fortress;

        private ReactiveEvent<WaveResult> _ended = new();

        private bool _isRunning;
        private float _time;
        private float _currentSpawnDelay;
        private int _currentSpawnIndex;
        private int _defeatedEnemies;
        private int _spawnedEnemies;

        private List<IDisposable> _disposables = new();

        public Wave(
            EntitiesBrainsFactory enemiesFactory,
            LevelConfig levelConfig,
            int waveIndex,
            Entity fortress)
        {
            _entitiesBrainsFactory = enemiesFactory;
            _waveConfig = levelConfig.GetWaveConfigBy(waveIndex);
            _enemyConfigs = new List<EntityConfig>(_waveConfig.EnemyConfigs);
            _fortressPosition = levelConfig.FortressPosition;
            _fortress = fortress;
        }

        public IReadonlyEvent<WaveResult> Ended => _ended;

        public void Run()
        {
            _isRunning = true;
            RandomizeSpawnDelay();

            _disposables.Add(_fortress.IsDead.Subscribe(OnFortressDestroyed));
        }

        public void Update(float deltaTime)
        {
            if (_isRunning == false)
                return;

            if (_defeatedEnemies >= _enemyConfigs.Count)
            {
                _ended?.Invoke(new WaveResult(true, _defeatedEnemies));
                _isRunning = false;
            }

            if (_spawnedEnemies >= _enemyConfigs.Count)
                return;

            _time += deltaTime;

            if (_time >= _currentSpawnDelay)
            {
                Entity enemy = _entitiesBrainsFactory.Create(GetRandomPosition(), _enemyConfigs[_currentSpawnIndex], Teams.Enemies);
                _spawnedEnemies++;
                _disposables.Add(enemy.IsDead.Subscribe(OnEnemyDie));

                _time = 0;

                RandomizeSpawnDelay();
            }
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }

        private void OnFortressDestroyed(bool arg1, bool isDestroyed)
        {
            if (isDestroyed)
                _ended?.Invoke(new WaveResult(false, _defeatedEnemies));
        }

        private void OnEnemyDie(bool arg1, bool isDie)
        {
            if (isDie)
                _defeatedEnemies++;
        }

        private Vector3 GetRandomPosition()
        {
            Vector2 random2D = Random.insideUnitCircle;

            Vector3 randomDirection = new Vector3(random2D.x, 0f, random2D.y);

            float randomLength = Random.Range(_waveConfig.MinSpawnRadius, _waveConfig.MaxSpawnRadius);

            return _fortressPosition + randomDirection.normalized * randomLength;
        }

        private void RandomizeSpawnDelay() => _currentSpawnDelay = Random.Range(_waveConfig.MinSpawnDelayTime, _waveConfig.MaxSpawnDelayTime);
    }
}