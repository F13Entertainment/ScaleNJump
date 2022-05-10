using System;
using _GAME.Scripts.Core;
using Assets.F13SDK.Scripts;
using TMPro;
using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public class LevelText : BaseUpdateableText<int>
    {
        protected override string ValueToString()
        {
            return "LEVEL "+Value();
        }

        protected override int Value()
        {
            return GameController.Instance.Level;
        }
    }
}
