using System.Collections.Generic;
using Assets.F13SDK.Scripts;
using F13StandardUtils.Scripts.Core;
using UnityEngine;

public class TutorialController : Singleton<TutorialController>
{
    public bool IsTutorialActive => !PlayerPrefsManager.Instance.playerData.IsTutorialCompleted;

    public bool IsButtonUpActive => IsTutorialActive && activeStep != null && activeStep.IsButtonUpActive;
    public bool IsButtonHoldActive => IsTutorialActive && activeStep != null && activeStep.IsButtonHoldActive;

    private int currentStepIndex = 0;

    [SerializeField] private GameObject tutorialHolder;
    [SerializeField] private GameObject levelHolder;
    [SerializeField] private List<GameObject> stepList;
    private TutorialStepBase activeStep = null;

    private void OnEnable()
    {
        GameController.Instance.OnGameplayEnter.AddListener(TutorialInit);
    }

    private void OnDisable()
    {
        GameController.Instance?.OnGameplayEnter.RemoveListener(TutorialInit);
    }

    private void TutorialInit()
    {
        tutorialHolder.SetActive(IsTutorialActive);
        levelHolder.SetActive(!IsTutorialActive);
        if (IsTutorialActive)
            NextStep();
    }

    private void Reset()
    {
        currentStepIndex = 0;
    }

    public void NextStep()
    {
        if(activeStep)
            Destroy(activeStep.gameObject);
        if (currentStepIndex < stepList.Count)
        {
            activeStep = Instantiate(stepList[currentStepIndex], transform).GetComponent<TutorialStepBase>();
            currentStepIndex++;
        }
    }
}
