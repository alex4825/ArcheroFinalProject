using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;
using DG.Tweening;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explode
{
    public class ExplodeView : EntityView
    {
        [SerializeField] private ParticleSystem _explosionPrefab;
        [SerializeField] private ParticleSystem _explosionDecalPrefab;

        private ReactiveEvent<Vector3> _explodedEvent;

        private IDisposable _explodedDisposable;

        protected override void OnEntityStartedWork(Entity entity)
        {
            _explodedEvent = entity.ExplodedEvent;
            _explodedDisposable = _explodedEvent.Subscribe(OnEntityExploded);
        }

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);

            _explodedDisposable?.Dispose();
        }

        private void OnEntityExploded(Vector3 point)
        {
            Instantiate(_explosionPrefab, point, Quaternion.identity, null);
            Instantiate(_explosionDecalPrefab, point, Quaternion.identity, null);
        }
    }
}