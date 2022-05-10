using UnityEngine;
using UnityEngine.Events;

namespace F13StandardUtils.Scripts.Core
{
    [System.Serializable] public class SwerveEvent : UnityEvent<float>{}
    public class SwerveController : Singleton<SwerveController>
    {
        public bool canSwerve = true;
        [SerializeField] private float swerveSpeed = 0.5f;
        [SerializeField] private float clampX = 60;
        public SwerveEvent OnSwerveChanged= new SwerveEvent();

        private float _currentPositionX=0;
        private float _lastPositionX;

        private void FixedUpdate()
        {
            Swerve();
        }

        private void Swerve()
        {
            if (!canSwerve) return;

            if (Input.GetMouseButtonDown(0))
            {
                _lastPositionX = Input.mousePosition.x;
            }

            if (Input.GetMouseButton(0))
            {
                if (_lastPositionX == 0)
                {
                    _lastPositionX = Input.mousePosition.x;
                }

                float currentX = Input.mousePosition.x;
                float deltaX = currentX - _lastPositionX;
                float targetX = deltaX * swerveSpeed * Time.deltaTime;
                _currentPositionX += targetX;
                _currentPositionX = Mathf.Clamp(_currentPositionX, -clampX, clampX);
                _lastPositionX = Input.mousePosition.x;
                OnSwerveChanged.Invoke(_currentPositionX);
            }
        }
    }
}