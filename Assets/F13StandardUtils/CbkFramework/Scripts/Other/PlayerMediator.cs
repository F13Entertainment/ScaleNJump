using F13StandardUtils.CbkFramework.Scripts.Core.Event.Interface;
using F13StandardUtils.CbkFramework.Scripts.Core.View;
using UnityEngine;

namespace F13StandardUtils.CbkFramework.Scripts.Other
{
    public class PlayerMediator : BaseMediator
    {
        // Start is called before the first frame update
        void OnEnable()
        {
            Subscribe(GameEvents.ON_SWERVE,OnSwerve);
        }

        private void OnSwerve(IEvent obj)
        {
            if (obj is SwerveEvent sweve)
            {
                transform.position = sweve.Value;
                Debug.Log(GetService<ISwerveService>().LastPosition());
            
            }
        }

        // Update is called once per frame
        void OnDisable()
        {
            Subscribe(GameEvents.ON_SWERVE,OnSwerve);

        }
    }
}
