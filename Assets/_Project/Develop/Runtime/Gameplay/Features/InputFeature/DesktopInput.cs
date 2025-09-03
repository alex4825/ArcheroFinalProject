using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature
{
    public class DesktopInput : IInputService, IDisposable, IUpdatable
    {
        private const string HorizontalAxisName = "Horizontal";
        private const string VerticalAxisName = "Vertical";
        private const string MouseXName = "Mouse X";
        private const string MouseYName = "Mouse Y";

        private ReactiveVariable<Vector3> _mousePosition = new();

        private IDisposable _mousePositionDisposable;

        public DesktopInput()
        {
            _mousePositionDisposable = _mousePosition.Subscribe(OnMousePositionChanged);
        }

        public bool IsEnabled { get; set; } = true;

        public Vector3 MoveDirection
        {
            get
            {
                if (IsEnabled == false)
                    return Vector3.zero;

                return new Vector3(Input.GetAxisRaw(HorizontalAxisName), 0, Input.GetAxisRaw(VerticalAxisName));
            }
        }

        public Vector3 RotationDirection { get; private set; }

        public void Update(float deltaTime)
        {
            _mousePosition.Value = GetMousePosition();
        }

        public void Dispose()
        {
            _mousePositionDisposable.Dispose();
        }

        private Vector3 GetMousePosition()
            => new Vector3(Input.GetAxisRaw(MouseXName), 0, Input.GetAxisRaw(MouseYName));

        private void OnMousePositionChanged(Vector3 lastPosition, Vector3 currentPosition)
            => RotationDirection = (currentPosition - lastPosition).normalized;
    }
}