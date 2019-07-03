using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UniRx;

namespace BehaviourTrees
{
    /// <summary>
    /// 処理を実行するクラス
    /// </summary>
    public class ActionNode : BehaviourTreeBase
    {
        private readonly Func<BehaviourTreeInstance, ExecutionResult> action;
        
        public ActionNode(Func<BehaviourTreeInstance, ExecutionResult> _action)
        {
            Init();
            action = _action;
        }

        public override void Reset()
        {

        }

        public override ExecutionResult Execute(BehaviourTreeInstance _instance)
        {
            _instance.nodeStateDic[key] = BehaviourTreeInstance.NodeState.READY;

            var result = action(_instance);

            if (result.booleanResult) {
                _instance.nodeStateDic[key] = BehaviourTreeInstance.NodeState.SUCCESS;
            }
            else {
                _instance.nodeStateDic[key] = BehaviourTreeInstance.NodeState.FAILURE;
            }
            return result;
        }
    }
}

