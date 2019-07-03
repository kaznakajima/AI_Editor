using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UniRx;

namespace BehaviourTrees
{
    /// <summary>
	/// Sequencerノードクラス
	/// 子が成功したら次の子を実行させる。
	/// 子が失敗したら Failure を返す。
	/// すべての子の処理が終わったら Success を返す
	/// </summary>
    public class SequencerNode : BehaviourTreeBase
    {
        private IEnumerator<BehaviourTreeBase> actions;

        public SequencerNode(BehaviourTreeBase[] actionArray)
        {
            Init();
            actions = actionArray.ToList().GetEnumerator();
        }

        public override void Reset()
        {
            actions.Reset();
            while (actions.MoveNext()) {
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
            if(_state == BehaviourTreeInstance.NodeState.FAILURE) {
                _instance.nodeStateDic[key] = BehaviourTreeInstance.NodeState.FAILURE;
            }
            else if(_state == BehaviourTreeInstance.NodeState.SUCCESS) {
                if (actions.MoveNext()) {
                    actions.Current.Execute(_instance);
                }
                else {
                    _instance.nodeStateDic[key] = BehaviourTreeInstance.NodeState.SUCCESS;
                }
            }
        }
    }
}

