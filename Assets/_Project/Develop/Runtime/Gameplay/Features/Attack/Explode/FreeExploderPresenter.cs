using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explode
{
    public class FreeExploderPresenter : IDisposable
    {
        private FreeExploderView _freeExploderView;
        private ReactiveEvent<Vector3> _explodedEvent;
        private ReactiveVariable<float> _explodeRadius;

        private IDisposable _explodedDisposable;

        public FreeExploderPresenter(FreeExploderView freeExploderView, ReactiveEvent<Vector3> explodedEvent, ReactiveVariable<float> explodeRadius)
        {
            _freeExploderView = freeExploderView;
            _explodedEvent = explodedEvent;
            _explodeRadius = explodeRadius;

            _explodedDisposable = _explodedEvent.Subscribe(OnExploded);
        }

        public void Dispose()
        {
            _explodedDisposable?.Dispose();
        }

        private void OnExploded(Vector3 point)
        {
            _freeExploderView.ShowExplodeIn(point, _explodeRadius.Value);
        }
    }
}