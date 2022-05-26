using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStep3 : TutorialStepBase
{
    Vector3 curVelocity = new Vector3();
    void Start()
    {
        StepSequence();
    }

    protected override void StepSequence()
    {
        isButtonHoldActive = false;
        isButtonUpActive = false;
        Ball.Instance.Rb.useGravity = false;
        curVelocity = Ball.Instance.Rb.velocity;
        Ball.Instance.Rb.velocity = Vector3.zero;
        StartCoroutine(NextStepDelay());
    }


    private IEnumerator NextStepDelay()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("GİRDİM");
        isButtonHoldActive = true;
        isButtonUpActive = true;
        Ball.Instance.Rb.useGravity = true;
        Ball.Instance.Rb.velocity = curVelocity;
        TutorialController.Instance.NextStep();
    }
}
