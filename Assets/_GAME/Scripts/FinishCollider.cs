using System.Collections;
using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using UnityEngine;

public class FinishCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            GameController.Instance.SuccessLevel();
        }

    }
}
