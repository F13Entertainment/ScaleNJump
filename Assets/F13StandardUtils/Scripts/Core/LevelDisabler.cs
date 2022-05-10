using System;
using System.Collections;
using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using UnityEngine;

public class LevelDisabler : MonoBehaviour
{
    [SerializeField] private int disablerLevel = 1; 
    [SerializeField] private List<GameObject> _objects=new List<GameObject>();

    private int lastLevel = 0;
    private void FixedUpdate()
    {
        var level = GameController.Instance.Level;

        if (lastLevel != level)
        {
            _objects.ForEach(o=>o.SetActive(level!=disablerLevel));
            lastLevel = level;
        }
    }
}
