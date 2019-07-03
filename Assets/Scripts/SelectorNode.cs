using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UniRx;

namespace BehaviourTrees
{
    /// <summary>
	/// Selectorノードクラス
	/// 成功する子が見つかるまで子を実行する 
	/// 成功するのが見つかったらこれ以降の処理は行わず Successを返す
	/// すべて実行しすべてが失敗に終わったら Failureを返す
	/// </summary>
    public class SelectorNode : BehaviourTreeBase
    {
        private IEnumerator<BehaviourTreeBase> actions;

        public SelectorNode(BehaviourTreeBase[] actionArray)
        {
            Init();
            actions = actionArray.ToList().GetEnumerator();
        }

        public override void Reset()
        {
            actions.Reset();
            while (actions.MoveNext())
            {
                actions.Current.Reset();
            }
            actions.Reset();
        }

        public override ExecutionResult Execute(BehaviourTreeInstance _instance)
        {
            _instance.nodeStateDic[key] = BehaviourTreeInstance.NodeState.READY;
            _instance.nodeStateDic.ObserveReplace()
                .Where(item => item.Key == actions.Current.key)
                .Subscribe(item => NextState(item.NewValue, _instance));
            actions.MoveNext();
            actions.Current.Execute(_instance);

            return new ExecutionResult(true);
        }

        void NextState(BehaviourTreeInstance.NodeState _state, BehaviourTreeInstance _instance)
        {
            if(_state == BehaviourTreeInstance.NodeState.SUCCESS) {
                _instance.nodeStateDic[key] = BehaviourTreeInstance.NodeState.SUCCESS;
            }
            else {
                if (actions.MoveNext()) {
                    actions.Current.Execute(_instance);
                }
                else {
                    _instance.nodeStateDic[key] = BehaviourTreeInstance.NodeState.FAILURE;
                }
            }
        }
    }
}


