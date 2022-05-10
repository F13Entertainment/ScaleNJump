using System;
using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    public class RunnerPanel : MonoBehaviour, IOmegaPanel
    {
        virtual public void OmegaPanelAwake()
        {
            this.gameObject.SetActive(false);
        }
        virtual public void HidePanel()
        {
            this.gameObject.SetActive(false);
        }
        virtual public void ShowPanel()
        {
            this.gameObject.SetActive(true);
        }
        virtual public void OmageOnUpdate()
        {
        }
    }
}
