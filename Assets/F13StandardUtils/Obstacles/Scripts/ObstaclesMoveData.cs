using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;


public enum MoveType
{
    Position,
    LocalPosition,
    Rotation,
    LocalRotation,
    Scale,
    Wait,
}

public enum Process
{
    NewTarget,
    Sum
    
}

[Serializable]
public class MoveData
{
    public bool isWait = true;
    public MoveType moveType = MoveType.Position;
    public float duration = 1;
    [HideIf("moveType",MoveType.Wait)]
    public Process process = Process.NewTarget;
    [HideIf("moveType",MoveType.Wait)]
    public Vector3 newVector;
    [HideIf("moveType",MoveType.Wait)]
    public Ease ease = Ease.Linear;
}



[CreateAssetMenu(fileName = "MoveData", menuName = "ScriptableObjects/MoveData", order = 1)]
public class ObstaclesMoveData : ScriptableObject
{
        
    
    public bool onStart, isLoop;
    [ShowIf("isLoop")]
    public LoopType loopType = LoopType.Restart;
    public List<MoveData> moveDatas = new List<MoveData>();

}
