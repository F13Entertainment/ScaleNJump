using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class FPSCounter : MonoBehaviour
{
    [SerializeField] private Text FPSText;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FpsDelay());
    }

    private IEnumerator FpsDelay()
    {
        while (true)
        {
            FPSText.text = Mathf.Ceil((1f / Time.deltaTime)).ToString();
            yield return new WaitForSeconds(.25f);
        }
    }
}
