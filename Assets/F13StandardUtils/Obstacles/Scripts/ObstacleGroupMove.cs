using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;


public class ObstacleGroupMove : MonoBehaviour
{
    public List<ObstacleMove> obstacleMoves = new List<ObstacleMove>();


    [Button]
    public void Move()
    {
        obstacleMoves.ForEach(x => x.Move());
    }
    
    [Button]
    public void ReStartMove()
    {
        obstacleMoves.ForEach(x => x.ReStartMove());
    }
    
    
    [Button]
    public void KillMove()
    {
        obstacleMoves.ForEach(x => x.KillMove());
    }
    
    [Button]
    public void GoToMoveStart()
    {
        obstacleMoves.ForEach(x => x.DORewind());
    }
    
        
    [Button]
    public void GoToMoveEnd()
    {
        obstacleMoves.ForEach(x => x.GoToMoveEnd());
    }
}