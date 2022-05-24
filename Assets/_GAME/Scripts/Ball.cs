using System;
using F13StandardUtils.Scripts.Core;
using UnityEngine;

public class Ball : Singleton<Ball>
{
    [SerializeField]private Rigidbody rb;
    public Rigidbody Rb => rb;

    private void Awake()
    {
        rb.useGravity = false;
    }

    private void OnEnable()
    {
        GameController.Instance.OnGameplayEnter.AddListener(OnGameplayEnter);
    }

    private void OnDisable()
    {
        GameController.Instance?.OnGameplayEnter.RemoveListener(OnGameplayEnter);
    }

    private void OnGameplayEnter()
    {
        rb.useGravity = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Border" || collision.gameObject.tag == "Obstacle")
            GameController.Instance.FailLevel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Diamond")
            Destroy(other.gameObject);
    }
}
