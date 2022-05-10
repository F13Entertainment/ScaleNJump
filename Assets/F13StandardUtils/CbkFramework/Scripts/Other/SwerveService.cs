using F13StandardUtils.CbkFramework.Scripts.Core.Event.Interface;
using F13StandardUtils.CbkFramework.Scripts.Core.ServiceLocator;
using F13StandardUtils.CbkFramework.Scripts.Core.ServiceLocator.Attribute;
using F13StandardUtils.CbkFramework.Scripts.Core.ServiceLocator.Interface;
using UnityEngine;

namespace F13StandardUtils.CbkFramework.Scripts.Other
{
    public class SwerveEvent :IEvent
    {
        public Vector3 Value;
    }

    public interface ISwerveService : IService
    {
        public float LastPosition();
    }


    [Service(false)]
    public class SwerveService : BaseMonoService ,ISwerveService
    {
        [SerializeField] private bool canSwerve;
        [SerializeField] private float lastPosition;
        [SerializeField] private float swerveSpeed;
        [SerializeField] private float clampX;


        private void Update()
        {
            Swerve();
        }

        private void Swerve()
        {
            if (!canSwerve)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                lastPosition = Input.mousePosition.x;
            }

            if (Input.GetMouseButton(0))
            {
                if (lastPosition == 0)
                {
                    lastPosition = Input.mousePosition.x;
                }

                float currentX = Input.mousePosition.x;
                float deltaX = currentX - lastPosition;
                float targetX = deltaX * swerveSpeed * Time.deltaTime;
                Vector3 targetPosition = transform.localPosition + Vector3.right * (targetX);
                targetPosition.x = Mathf.Clamp(targetPosition.x, -clampX, clampX);
                transform.localPosition = targetPosition;
                lastPosition = Input.mousePosition.x;
                Fire(GameEvents.ON_SWERVE,new SwerveEvent(){Value = targetPosition});
            }
        }

        public void Initialize()
        {
        
        
        }

        public float LastPosition()
        {
            return lastPosition;
        }
    }
}