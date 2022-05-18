using DG.Tweening;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float tourTime;

    // Start is called before the first frame update
    void Start()
    {
        transform.DORotate(new Vector3(0f, 0f, -360f), tourTime,RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1);
    }
}
