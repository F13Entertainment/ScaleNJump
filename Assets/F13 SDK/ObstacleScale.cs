using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ObstacleScale : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        transform.DOScaleY(-5f, 9f).SetEase(Ease.Linear);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
