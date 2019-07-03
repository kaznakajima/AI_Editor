using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace BehaviourTrees
{
    public class BehaviourTreeInstance
    {
        // ノードのステート
        public enum NodeState
        {
            READY,
            SUCCESS,
            FAILURE
        }

        /// <summary>
        /// 終了検知ReactiveProperty
        /// </summary>
        public ReactiveProperty<NodeState> finishRP = new ReactiveProperty<NodeState>(NodeState.READY);

        /// <summary>
        /// 各ノードのNodeStateの状態変化を監視するReactiveProperty
        /// </summary>
        public ReactiveDictionary<string, NodeState> nodeStateDic = new ReactiveDictionary<string, NodeState>();

        private BehaviourTreeBase rootNode;

        public BehaviourTreeInstance(BehaviourTreeBase _rootNode)
        {
            rootNode = _rootNode;

            nodeStateDic.ObserveAdd()
                .Where(item => item.Value == NodeState.READY)
                .Subscribe(item => SetCurrentNodeKey(item.Key));

            nodeStateDic.ObserveReplace()
                .Where(item => item.Key == rootNode.key)
                .Where(item => item.NewValue == NodeState.FAILURE || item.NewValue == NodeState.SUCCESS)
                .Subscribe(item => Finish(item.NewValue));
        }

        /// <summary>
        /// 実行する
        /// </summary>
        public void Excute() {
            rootNode.Execute(this);
        }

        /// <summary>
        /// 状態をリセットして初めから実行
        /// </summary>
        public void Reset()
        {
            nodeStateDic.Clear();
            finishRP.Value = NodeState.READY;
            rootNode.Reset();
            rootNode.Execute(this);
        }

        void Finish(NodeState _state) {
            finishRP.Value = _state;
        }

        void SetCurrentNodeKey(string _key) {
            Debug.Log(_key);
        }
    }
}


