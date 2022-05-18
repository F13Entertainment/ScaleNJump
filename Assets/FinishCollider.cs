using System.Collections;
using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using UnityEngine;

public class FinishCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            GameController.Instance.SuccessLevel();
        }

    }
}
