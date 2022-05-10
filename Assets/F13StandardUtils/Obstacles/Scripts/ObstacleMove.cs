using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    public ObstaclesMoveData moveData;
    private Sequence _sequence;

    private void Start()
    {

       
        if (moveData.onStart) Move();
    }


    [Button]
    public void Move()
    {
        _sequence = DOTween.Sequence();
        foreach (var mData in moveData.moveDatas)
        {
            //Waiting
            if (mData.moveType == MoveType.Wait) _sequence.AppendInterval(mData.duration);

            else
            {
                //Append
                if (mData.isWait)
                {
                    switch (mData.moveType)
                    {
                        case MoveType.Position:
                            if (mData.process == Process.NewTarget)
                                _sequence.Append(transform.DOMove(mData.newVector, mData.duration).SetEase(mData.ease));
                            else if (mData.process == Process.Sum)
                                _sequence.Append(transform.DOMove(transform.position + mData.newVector, mData.duration)
                                    .SetEase(mData.ease));
                            break;
                        case MoveType.LocalPosition:
                            if (mData.process == Process.NewTarget)
                                _sequence.Append(transform.DOLocalMove(mData.newVector, mData.duration).SetEase(mData.ease));
                            else if (mData.process == Process.Sum)
                                _sequence.Append(transform.DOLocalMove(transform.localPosition + mData.newVector, mData.duration)
                                    .SetEase(mData.ease));
                            break;
                        case MoveType.Rotation:
                            if (mData.process == Process.NewTarget)
                                _sequence.Append(transform.DORotate(mData.newVector, mData.duration, RotateMode.FastBeyond360)
                                    .SetEase(mData.ease));
                            else if (mData.process == Process.Sum)
                                _sequence.Append(transform
                                    .DORotate(transform.eulerAngles + mData.newVector, mData.duration, RotateMode.FastBeyond360)
                                    .SetEase(mData.ease));
                            break;
                        case MoveType.LocalRotation:
                            if (mData.process == Process.NewTarget)
                                _sequence.Append(transform.DOLocalRotate(mData.newVector, mData.duration, RotateMode.FastBeyond360)
                                    .SetEase(mData.ease));
                            else if (mData.process == Process.Sum)
                                _sequence.Append(transform
                                    .DOLocalRotate(transform.localEulerAngles + mData.newVector, mData.duration, RotateMode.FastBeyond360)
                                    .SetEase(mData.ease));
                            print(transform.localEulerAngles + mData.newVector);
                            break;
                        case MoveType.Scale:
                            if (mData.process == Process.NewTarget)
                                _sequence.Append(transform.DOScale(mData.newVector, mData.duration).SetEase(mData.ease));
                            else if (mData.process == Process.Sum)
                                _sequence.Append(transform.DOScale(transform.localScale + mData.newVector, mData.duration)
                                    .SetEase(mData.ease));
                            break;
                    }
                }
                //Join
                else
                {
                    switch (mData.moveType)
                    {
                        case MoveType.Position:
                            if (mData.process == Process.NewTarget)
                                _sequence.Join(transform.DOMove(mData.newVector, mData.duration).SetEase(mData.ease));
                            else if (mData.process == Process.Sum)
                                _sequence.Join(transform.DOMove(transform.position + mData.newVector, mData.duration)
                                    .SetEase(mData.ease));
                            break;
                        case MoveType.LocalPosition:
                            if (mData.process == Process.NewTarget)
                                _sequence.Join(transform.DOLocalMove(mData.newVector, mData.duration).SetEase(mData.ease));
                            else if (mData.process == Process.Sum)
                                _sequence.Join(transform.DOLocalMove(transform.localPosition + mData.newVector, mData.duration)
                                    .SetEase(mData.ease));
                            break;
                        case MoveType.Rotation:
                            if (mData.process == Process.NewTarget)
                                _sequence.Join(transform.DORotate(mData.newVector, mData.duration, RotateMode.FastBeyond360)
                                    .SetEase(mData.ease));
                            else if (mData.process == Process.Sum)
                                _sequence.Join(transform
                                    .DORotate(transform.eulerAngles + mData.newVector, mData.duration, RotateMode.FastBeyond360)
                                    .SetEase(mData.ease));
                            break;
                        case MoveType.LocalRotation:
                            if (mData.process == Process.NewTarget)
                                _sequence.Join(transform.DOLocalRotate(mData.newVector, mData.duration, RotateMode.FastBeyond360)
                                    .SetEase(mData.ease));
                            else if (mData.process == Process.Sum)
                                _sequence.Join(transform
                                    .DOLocalRotate(transform.localEulerAngles + mData.newVector, mData.duration, RotateMode.FastBeyond360)
                                    .SetEase(mData.ease));
                            break;
                        case MoveType.Scale:
                            if (mData.process == Process.NewTarget)
                                _sequence.Join(transform.DOScale(mData.newVector, mData.duration).SetEase(mData.ease));
                            else if (mData.process == Process.Sum)
                                _sequence.Join(transform.DOScale(transform.localScale + mData.newVector, mData.duration)
                                    .SetEase(mData.ease));
                            break;
                    }
                }
            }
        }

        if (moveData.isLoop) _sequence.SetLoops(-1, moveData.loopType);
    }

    [Button]
    public void ReStartMove()
    {
        _sequence.Restart();
    }
    
    
    [Button]
    public void KillMove()
    {
        _sequence.Kill();
    }
    
    [Button]
    public void GoToMoveStart()
    {
        _sequence.Rewind();
    }
    
        
    [Button]
    public void GoToMoveEnd()
    {
        _sequence.Complete();
    }
}