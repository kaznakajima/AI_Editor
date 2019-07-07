using UnityEngine;
using AI.BtGraph.Base;

namespace AI.BtGraph
{
    public class BtCheckDistance : BtDecorator
    {
        public enum Condition
        {
            Equal = 0,
            NotEqual,
            Less,
            LessEqual,
            Greater,
            GreaterEqual,
        }

        public Condition consition;
        public float distance;

        public override bool Branch(Data _data)
        {
            float dist = Vector3.Distance(_data.goal.CachedTransform.position, _data.mover.CachedTransform.position);
            switch(consition)
            {
                case Condition.Equal:
                    return Mathf.Approximately(dist, distance);
                case Condition.Less:
                    return dist < distance;
                case Condition.LessEqual:
                    return dist <= distance;
                case Condition.Greater:
                    return dist > distance;
                case Condition.GreaterEqual:
                    return dist >= distance;
            }
            return false;
        }
    }
}