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
        private ReactiveVariable<float> _explodeRadius;

        private IDisposable _explodedDisposable;

        protected override void OnEntityStartedWork(Entity entity)
        {
            _explodedEvent = entity.ExplodedEvent;
            _explodeRadius = entity.ExplodeRadius;
            _explodedDisposable = _explodedEvent.Subscribe(OnEntityExploded);
        }

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);

            _explodedDisposable?.Dispose();
        }

        private void OnEntityExploded(Vector3 point)
        {
            ParticleSystem explosion = Instantiate(_explosionPrefab, point, _explosionPrefab.transform.rotation, null);
            explosion.transform.localScale *= _explodeRadius.Value / 2;

            if (_explosionDecalPrefab != null)
            {
                ParticleSystem explosionDecal = Instantiate(_explosionDecalPrefab, point, _explosionDecalPrefab.transform.rotation, null);
                explosionDecal.transform.localScale *= _explodeRadius.Value / 2;
            }
        }
    }
}