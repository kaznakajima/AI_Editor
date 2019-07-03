using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System.Linq;

namespace BehaviourTrees
{
    /// <summary>
    /// Dicoratorノードクラス
    /// </summary>
    public class DecoratorNode : BehaviourTreeBase
    {
        private Func<BehaviourTreeInstance, ExecutionResult> conditionFunction;
        private BehaviourTreeBase action;

        public DecoratorNode(Func<BehaviourTreeInstance, ExecutionResult> _func, BehaviourTreeBase _action)
        {
            Init();
            conditionFunction = _func;
            action = _action;
        }

        public override void Reset()
        {
            //this.actions.Reset();
        }

        /// <summary>
        /// 実行処理
        /// </summary>
        /// <param name="_instance">BehaviourTreeInstance</param>
        /// <returns>実行結果</returns>
        public override ExecutionResult Execute(BehaviourTreeInstance _instance)
        {
            _instance.nodeStateDic[key] = BehaviourTreeInstance.NodeState.READY;

            if(conditionFunction(_instance).booleanResult) {
                _instance.nodeStateDic.ObserveReplace()
                    .Where(item => item.Key == action.key)
                    .Subscribe(item => NextState(item.NewValue, _instance));
                action.Execute(_instance);
            }
            else {
                _instance.nodeStateDic[key] = BehaviourTreeInstance.NodeState.FAILURE;
            }

            return new ExecutionResult(true);
        }

        void NextState(BehaviourTreeInstance.NodeState _state, BehaviourTreeInstance _instance)
        {
            if(_state == BehaviourTreeInstance.NodeState.SUCCESS) {
                _instance.nodeStateDic[key] = BehaviourTreeInstance.NodeState.SUCCESS;
            }
            else if(_state == BehaviourTreeInstance.NodeState.FAILURE) {
                _instance.nodeStateDic[key] = BehaviourTreeInstance.NodeState.FAILURE;
            }
        }
    }
}

