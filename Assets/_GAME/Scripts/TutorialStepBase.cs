using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialStepBase : MonoBehaviour
{
    protected bool isButtonUpActive;
    protected bool isButtonHoldActive;

    public bool IsButtonUpActive
    {
        get=> isButtonUpActive;
    } 
    public bool IsButtonHoldActive
    {
        get => isButtonHoldActive;
    }

    protected abstract void StepSequence();
}
