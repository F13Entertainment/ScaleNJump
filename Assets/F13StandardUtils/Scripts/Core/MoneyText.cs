using System;
using System.Collections;
using System.Collections.Generic;
using _GAME.Scripts.Core;
using TMPro;
using UnityEngine;

public class MoneyText : BaseUpdateableText<int>
{
    public static MoneyText Instance;

    private void Awake()
    {
        MoneyText.Instance = this;
    }

    protected override string ValueToString()
    {
        return Value().ToString();
    }

    protected override int Value()
    {
        return EconomyManager.Instance.MoneyCount;
    }
}
