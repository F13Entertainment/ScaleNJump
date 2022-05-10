using UnityEngine;

namespace F13StandardUtils.Crowd.Scripts
{
    public class ShowCrowdAuraWithCount : MonoBehaviour
    {
        public CrowdManager crowd;

        private int lastCount;
        private void Update()
        {
            if (lastCount != crowd.Count)
            {

                UpdateAuraProcess();
                lastCount = crowd.Count;
            }
        }

        private void UpdateAuraProcess()
        {
            if (crowd.Count > 0)
            {
                crowd.crowdAura.ShowAura();
            }
            else
            {
                crowd.crowdAura.HideAura();

            }
        }
    }
}
