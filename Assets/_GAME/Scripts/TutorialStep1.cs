using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialStep1 : TutorialStepBase
{
    [SerializeField] private GameObject HoldUIPanel;
    [SerializeField] private Text infoText;

    private void OnEnable()
    {
        RampMechanic1.Instance.OnRampFullyOpened.AddListener(OnRampFullyOpened);
    }

    private void OnDisable()
    {
        RampMechanic1.Instance?.OnRampFullyOpened.RemoveListener(OnRampFullyOpened);
    }

    void Start()
    {
        StepSequence();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
            OnMouseUp();
    }

    protected override void StepSequence()
    {
        infoText.text = "HOLD";
        isButtonHoldActive = true;
        isButtonUpActive = false;
        Ball.Instance.Rb.useGravity = false;
        HoldUIPanel.SetActive(true);
    }

    private void OnRampFullyOpened()
    {
        infoText.text = "RELEASE";
        isButtonUpActive = true;

    }

    private void OnMouseUp()
    {
        if (isButtonUpActive)
        {
            Ball.Instance.Rb.useGravity = true;
            StartCoroutine(NextStepDelay());
            isButtonHoldActive = false;
            isButtonUpActive = false;
        }
    }

    private IEnumerator NextStepDelay()
    {
        yield return new WaitForSeconds(3.2f);
        TutorialController.Instance.NextStep();
    }
}
