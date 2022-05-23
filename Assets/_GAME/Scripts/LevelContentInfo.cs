using F13StandardUtils.Scripts.Core;
using UnityEngine;

public class LevelContentInfo : Singleton<LevelContentInfo>
{
    [SerializeField] private Transform finishTransform;
    public float LevelWidth => finishTransform.position.x;
}
