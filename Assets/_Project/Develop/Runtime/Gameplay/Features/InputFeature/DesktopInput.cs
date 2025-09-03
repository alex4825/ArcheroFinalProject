using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature
{
    public class DesktopInput : IInputService, IDisposable, IUpdatable
    {
        private const string HorizontalAxisName = "Horizontal";
        private const string VerticalAxisName = "Vertical";
        private const int LeftMouseButton = 0;
        private readonly Vector3 ScreenCenter = new Vector3(Screen.width / 2, 0, Screen.height / 2);

        private ReactiveVariable<Vector3> _mousePosition = new();

        private ReactiveEvent _attacked = new();

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

        public IReadonlyEvent Attacked => _attacked;

        public void Update(float deltaTime)
        {
            _mousePosition.Value = GetMousePosition();

            if (Input.GetMouseButtonDown(LeftMouseButton))
                _attacked.Invoke();
        }

        public void Dispose()
        {
            _mousePositionDisposable.Dispose();
        }

        private Vector3 GetMousePosition()
            => new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);

        private void OnMousePositionChanged(Vector3 lastPosition, Vector3 currentPosition)
        {
            Vector3 cursorMoveVector = currentPosition - ScreenCenter;

            RotationDirection = cursorMoveVector.normalized;
        }
    }
}