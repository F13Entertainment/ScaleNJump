using System.Collections;
using DG.Tweening;
using F13StandardUtils.Scripts.Core;
using UnityEngine;
using UnityEngine.Events;

public class RampMechanic1 : Singleton<RampMechanic1>
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private GameObject rampPrefab;
    [SerializeField] private Transform rampPrefabCreateTransform;

    private GameObject lastCreatedRamp;
    private Coroutine rampBlendCor = null;

    private const int blendShapeIndexCount = 2;
    [SerializeField] private TutorialController tutorialController;

    public UnityEvent OnRampFullyOpened;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (tutorialController == null)
                OnMouseDragg();
            else
            {
                if (rampBlendCor != null && !tutorialController.IsButtonHoldActive)
                    StopCoroutine(rampBlendCor);
                if (tutorialController.IsButtonHoldActive)
                    OnMouseDragg();
            }
        }


        if (Input.GetMouseButtonUp(0))
        {
            OnMouseUpp();
        }
    }

    private void OnMouseDragg()
    {
        if (!GameController.Instance.IsInGame)
            return;
        if (rampBlendCor==null)
            rampBlendCor = StartCoroutine(BlendShapeScale(.5f));
    }

    private void OnMouseUpp()
    {
        if (!GameController.Instance.IsInGame || Time.timeScale <= 0f)
            return;

        if (rampBlendCor != null)
        {
            transform.DOKill();
            StopCoroutine(rampBlendCor);
        }

        rampBlendCor = null;
        if (tutorialController!= null && !tutorialController.IsButtonUpActive)
            return;
        if (lastCreatedRamp)
            Destroy(lastCreatedRamp);

        lastCreatedRamp = Instantiate(rampPrefab, transform.position, Quaternion.identity, rampPrefabCreateTransform);
        lastCreatedRamp.GetComponent<RampPrefab>().Bake(skinnedMeshRenderer, blendShapeIndexCount);

        ResetRamp();
    }

    private void  ResetRamp()
    {
        for (int i = 0; i < blendShapeIndexCount; i++)
            skinnedMeshRenderer.SetBlendShapeWeight(i, 100f);

        transform.localPosition = new Vector2(2.17f, 2.1f - 2f);
    }

    private IEnumerator BlendShapeScale(float duration)
    {
        float blendValue = 1f;
        bool isIncrementState = false;
        int blendShapeIndex = blendShapeIndexCount - 1;

        while (blendValue > 0f && blendShapeIndex >= 0)
        {
            blendValue = Mathf.Clamp(isIncrementState ? blendValue + (Time.deltaTime / duration) : blendValue - (Time.deltaTime / duration), 0f, 1f);
            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, (blendValue * 100f));

            if (blendValue >= 1f)
            {
                if ((blendShapeIndex + 1) == blendShapeIndexCount)
                {
                    isIncrementState = false;
                    blendValue = 1f;
                }
                else
                {
                    blendShapeIndex++;
                    isIncrementState = true;
                    blendValue = 0f;
                }

                UpdatePosition(blendShapeIndex, isIncrementState, duration);
            }
            else if (blendValue <= 0f)
            {
                if (blendShapeIndex == 0)
                {
                    isIncrementState = true;
                    blendValue = 0f;
                }
                else
                {
                    blendShapeIndex--;
                    isIncrementState = false;
                    blendValue = 1f;
                }

                UpdatePosition(blendShapeIndex, isIncrementState, duration);
            }

            yield return null;
        }
        OnRampFullyOpened.Invoke();
    }

    public void UpdatePosition(int blendShapeIndex, bool isIncrementState, float duration)
    {
        if ((blendShapeIndex == 0 && !isIncrementState) || (blendShapeIndex == 1 && isIncrementState))
            transform.DOLocalMove(new Vector2(2.82f, -1f), duration).SetEase(Ease.Linear);
        else if (blendShapeIndex == 0 && isIncrementState)
            transform.DOLocalMove(new Vector2(3f, -2f), duration).SetEase(Ease.Linear);
        else if (blendShapeIndex == 1 && !isIncrementState)
            transform.DOLocalMove(new Vector2(2.17f, .1f), duration).SetEase(Ease.Linear);
        else
            Debug.LogWarning("Ramp doesn't has blendShapeIndex for update position.");
    }
}