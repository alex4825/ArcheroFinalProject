using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.Enemies;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Assets._Project.Develop.Runtime.Gameplay.Waves
{
    public class Wave : IDisposable
    {
        private const float MaxPointDeviation = 35f;

        private readonly EnemiesFactory _enemiesFactory;
        private WaveConfig _waveConfig;

        private List<EntityConfig> _enemyConfigs;
        private Vector3 _fortressPosition;
        private Entity _fortress;

        private ReactiveEvent<WaveResult> _ended = new();
        private ReactiveEvent _enemyKilled = new();

        private bool _isRunning;
        private float _time;
        private float _currentSpawnDelay;
        private int _currentSpawnIndex;
        private int _defeatedEnemies;
        private int _killedEnemies;

        private List<IDisposable> _disposables = new();

        public Wave(
            EnemiesFactory enemiesFactory,
            LevelConfig levelConfig,
            int waveIndex,
            Entity fortress)
        {
            _enemiesFactory = enemiesFactory;
            _waveConfig = levelConfig.GetWaveConfigBy(waveIndex);
            _enemyConfigs = new List<EntityConfig>(_waveConfig.EnemyConfigs);
            _fortressPosition = levelConfig.FortressPosition;
            _fortress = fortress;
        }

        public IReadonlyEvent<WaveResult> Ended => _ended;
        public IReadonlyEvent EnemyKilled => _enemyKilled;

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
                _ended?.Invoke(new WaveResult(true, _killedEnemies));
                _isRunning = false;
            }

            if (_currentSpawnIndex >= _enemyConfigs.Count)
                return;

            _time += deltaTime;

            if (_time >= _currentSpawnDelay)
            {
                Entity enemy = _enemiesFactory.Create(GetRandomPosition(), _enemyConfigs[_currentSpawnIndex], Teams.Enemies);
                _currentSpawnIndex++;
                _disposables.Add(enemy.IsDead.Subscribe((oldDie, isDie) => OnEnemyDie(isDie, enemy.IsKilled.Value)));

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
                _ended?.Invoke(new WaveResult(false, _killedEnemies));
        }

        private void OnEnemyDie(bool isDie, bool isKilled)
        {
            if (isDie)
            {
                _defeatedEnemies++;

                if (isKilled)
                {
                    _killedEnemies++;
                    _enemyKilled?.Invoke();
                }
            }
        }

        private Vector3 GetRandomPosition()
        {
            Vector2 random2D = Random.insideUnitCircle;
            Vector3 randomDirection = new Vector3(random2D.x, 0f, random2D.y);
            float randomLength = Random.Range(_waveConfig.MinSpawnRadius, _waveConfig.MaxSpawnRadius);

            Vector3 randomPoint = _fortressPosition + randomDirection.normalized * randomLength;

            /*if (NavMesh.SamplePosition(randomPoint, out NavMeshHit navMeshHit, MaxPointDeviation, NavMesh.AllAreas) == false)
            {
                randomPoint = navMeshHit.position;
                Debug.LogWarning("Рандомная точка спавна не найдена!!!");
            }
            else
            {
                randomPoint = new Vector3(40, 0, -8);
            }*/

            NavMesh.SamplePosition(randomPoint, out NavMeshHit navMeshHit, MaxPointDeviation, NavMesh.AllAreas);

            //Debug.Log("Точка спавна: " + randomPoint);

            return navMeshHit.position;
        }

        private void RandomizeSpawnDelay() => _currentSpawnDelay = Random.Range(_waveConfig.MinSpawnDelayTime, _waveConfig.MaxSpawnDelayTime);
    }
}