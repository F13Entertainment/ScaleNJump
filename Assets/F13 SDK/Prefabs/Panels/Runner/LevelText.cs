using System;
using F13StandardUtils.Scripts.Core;
using TMPro;
using UnityEngine;

public class LevelText : MonoBehaviour
{
    private int level=-1;
    [SerializeField] private TextMeshProUGUI _text;

    private void Update()
    {
        if (GameController.Instance)
        {
            var levelId = GameController.Instance.Level;
            if (level != levelId)
            {
                _text.text = "LEVEL " + levelId;
            }
        }
        else
        {
            if(!_text.text.Equals(String.Empty))
                _text.text = string.Empty;
        }
    }
}
