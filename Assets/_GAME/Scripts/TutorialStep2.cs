using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStep2 : TutorialStepBase
{
    [SerializeField] private GameObject HoldUIPanel;
    [SerializeField] private Text infoText;
    private Vector3 curVelocity;

    void Start()
    {
        StepSequence();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
            OnMouseUp();

        if (Input.GetMouseButtonDown(0))
            OnMouseDown();

        if (Input.GetMouseButton(0))
            OnMouseDown();
    }

    protected override void StepSequence()
    {
        infoText.text = "HOLD";
        isButtonHoldActive = true;
        isButtonUpActive = true;
        Ball.Instance.Rb.useGravity = false;
        curVelocity = Ball.Instance.Rb.velocity;
        Ball.Instance.Rb.velocity = Vector3.zero;
        HoldUIPanel.SetActive(true);
    }

    private void OnMouseUp()
    {
        Ball.Instance.Rb.useGravity = true;
        isButtonUpActive = false;
        StartCoroutine(NextStepDelay());
    }

    private void OnMouseDown()
    {
        infoText.text = "RELEASE";
        isButtonHoldActive = false;
    }


    private IEnumerator NextStepDelay()
    {
        yield return new WaitForSeconds(1.2f);
        TutorialController.Instance.NextStep();
    }
}
