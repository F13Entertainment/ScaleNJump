using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    public class OmegaHapticManager : OmegaSingletonManager<OmegaHapticManager>
    {
        public static float HAPTIC_INTERVAL = 0.35f;
        
        private float _lastHapticTime;
        
        public bool isActive = true;

        public void AwakeOmageHapticManager()
        {
            UIInputManager.On_HapticToggle += HapticToggle;
            OmegaDebugManager.Instance.PrintDebug("isActive: " + isActive, DebugType.HapticManager);
        }

        public void SuccessHaptic()
        {
            if (isActive)
            {
                if ((Time.time - _lastHapticTime) > HAPTIC_INTERVAL)
                {
                    _lastHapticTime = Time.time;
                    Taptic.Success();
                    OmegaDebugManager.Instance.PrintDebug("Success Haptic", DebugType.HapticManager);
                }
            }
        }
        public void FailedHaptic()
        {
            if (isActive)
            {
                if ((Time.time - _lastHapticTime) > HAPTIC_INTERVAL)
                {
                    _lastHapticTime = Time.time;
                    Taptic.Failure();
                }
            }
        }
        public void LightHaptic()
        {
            if (isActive)
            {
                if ((Time.time - _lastHapticTime) > HAPTIC_INTERVAL)
                {
                    _lastHapticTime = Time.time;
                    Taptic.Light();
                    OmegaDebugManager.Instance.PrintDebug("Light Haptic", DebugType.HapticManager);
                }
            }
        }
        public void MediumHaptic()
        {
            if (isActive)
            {
                if ((Time.time - _lastHapticTime) > HAPTIC_INTERVAL)
                {
                    _lastHapticTime = Time.time;
                    Taptic.Medium();
                    OmegaDebugManager.Instance.PrintDebug("Medium Haptic", DebugType.HapticManager);
                }
            }
        }
        public void HeavyHaptic()
        {
            if (isActive)
            {
                if ((Time.time - _lastHapticTime) > HAPTIC_INTERVAL)
                {
                    _lastHapticTime = Time.time;
                    Taptic.Heavy();
                    OmegaDebugManager.Instance.PrintDebug("Heavy Haptic", DebugType.HapticManager);
                }
            }

        }
        public void SetActive()
        {
            isActive = true;
            OmegaDebugManager.Instance.PrintDebug("Haptic Manager set activated", DebugType.HapticManager);
            PlayerPrefsManager.Instance.SaveHapticManagerPrefs(isActive);

        }
        public void SetDeactive()
        {
            isActive = false;
            OmegaDebugManager.Instance.PrintDebug("Haptic Manager set deactivated", DebugType.HapticManager);
            PlayerPrefsManager.Instance.SaveHapticManagerPrefs(isActive);
        }
        private void HapticToggle()
        {
            if (isActive) SetDeactive();
            else SetActive();
        }
    }

}
