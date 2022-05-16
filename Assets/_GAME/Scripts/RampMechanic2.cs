using System.Collections;
using UnityEngine;

public class RampMechanic2 : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private GameObject rampPrefab;
    [SerializeField] private Transform rampPrefabCreateTransform;

    private GameObject lastCreatedRamp;
    private Coroutine rampBlendCor;

    private const int blendShapeIndexCount = 2;

    private bool isWall = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(0f, 0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnMouseDownn();

        if (Input.GetMouseButtonUp(0))
            OnMouseUpp();

        if (Input.GetMouseButton(0))
            OnMouseDragg();

    }

    private void OnMouseDownn()
    {
        RaycastHit hit;
        Ray rr = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(rr, out hit);

        if ((hit.collider.tag == "Wall" || hit.collider.tag == "Ramp") && Time.timeScale > 0)
        {
            var temp = Input.mousePosition;
            temp.y += Screen.height / 10f;
            Ray rr2 = Camera.main.ScreenPointToRay(temp);
            Physics.Raycast(rr2, out hit);

            isWall = true;
            if(lastCreatedRamp)
                lastCreatedRamp.GetComponent<RampPrefab>().IsLastPrefab = false;
            lastCreatedRamp = Instantiate(rampPrefab, new Vector3(hit.point.x, hit.point.y, 0f), Quaternion.identity, rampPrefabCreateTransform);
            lastCreatedRamp.GetComponent<RampPrefab>().IsLastPrefab = true;
            Destroy(lastCreatedRamp, 5f);
        }
    }

    private void OnMouseDragg()
    {
        if (!isWall || lastCreatedRamp == null)
        {
            if (rampBlendCor != null)
                StopCoroutine(rampBlendCor);
            return;
        }

        if (rampBlendCor == null)
        {
            rampBlendCor = StartCoroutine(BlendShapeScale(.5f, lastCreatedRamp.GetComponent<SkinnedMeshRenderer>()));
        }
        lastCreatedRamp.GetComponent<RampPrefab>().Bake(lastCreatedRamp.GetComponent<SkinnedMeshRenderer>(), blendShapeIndexCount);
    }

    private void OnMouseUpp()
    {
        isWall = false;
        if (rampBlendCor != null)
            StopCoroutine(rampBlendCor);
        rampBlendCor = null;
    }

    private void ResetRamp()
    {
        for (int i = 0; i < blendShapeIndexCount; i++)
            skinnedMeshRenderer.SetBlendShapeWeight(i, 100f);

        transform.localPosition = new Vector2(2.17f, 2.1f - 2f);
    }

    private IEnumerator BlendShapeScale(float duration, SkinnedMeshRenderer skinnedMeshRenderer)
    {
        float blendValue = 1f;
        bool isIncrementState = false;
        int blendShapeIndex = blendShapeIndexCount - 1;

        while (true)
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
            }

            yield return null;
        }
    }
}