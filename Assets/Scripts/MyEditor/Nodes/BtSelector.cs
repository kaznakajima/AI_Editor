using System.Collections.Generic;
using UnityEngine;

namespace AI.BtGraph
{
    public class BtSelector : Base.BtNode
    {
        [Input] public BtKnobEmpty inputKnob;
        [Output] public BtKnobEmpty outputKnob;

        protected List<Base.BtNode> btNodeList;

        public BtSelector() : base(BtNodeType.Selector)
        {
            btNodeList = new List<Base.BtNode>();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        protected override void Init()
        {
            base.Init();

            btNodeList.Clear();
            Base.BtNode btNode = null;
            foreach(var port in Outputs) {
                if(port.IsConnected) {
                    int cnt = port.ConnectionCount;
                    for(int i = 0;i < cnt; ++i) {
                        var p = port.GetConnection(i);
                        if(p != null) {
                            btNode = p.node as Base.BtNode;
                            if(btNode != null) {
                                btNodeList.Add(btNode);
                            }
                        }
                    }
                }
            }
            // 降順 : priorityの値が大きい順に実行
            btNodeList.Sort((a, b) => b.Prioroty - a.Prioroty);
        }

        /// <summary>
        /// 実行結果を返す
        /// </summary>
        /// <param name="_data">データ</param>
        /// <returns>実行結果</returns>
        public override BtResult Exec(Data _data)
        {
            foreach(var node in btNodeList) {
                if(node.Exec(_data) == BtResult.Success) {
                    return BtResult.Success;
                }
            }
            return BtResult.Failure;
        }
    }
}