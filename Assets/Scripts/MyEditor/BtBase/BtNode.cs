using System;
using XNode;

// すべてのノードが持つ
namespace AI
{
    // ノードのタイプ
    public enum BtNodeType
    {
        Start = 0,
        Action,
        Decorator,
        Selector,
        Sequencer,
        Debug = 9999,
    }

    // ノードの結果
    public enum BtResult
    {
        Success = 0,
        Failure,
        Continue,
        Running,
    }
}

namespace AI.BtGraph.Base
{
    public class BtNode : Node
    {
        public int Prioroty;

        public BtNodeType NodeType { get; protected set; }

        // コンストラクタ(タイプを指定する)
        public BtNode(BtNodeType _type) {
            NodeType = _type;
        }

        /// <summary>
        /// 実行結果を返す(それぞれのノードで処理を明記する)
        /// </summary>
        /// <param name="_data">データ</param>
        /// <returns>実行結果</returns>
        public virtual BtResult Exec(Data _data) {
            return BtResult.Success;
        }

        /// <summary>
        /// 次に実行するノードを取得
        /// </summary>
        /// <returns>次のノード</returns>
        public virtual BtNode GetNext()
        {
            var port = GetOutputPort("outputKnob");
            BtNode next = null;
            if(port != null && port.IsConnected) {
                next = port.Connection.node as BtNode;
            }
            return next;
        }

        public virtual void Setup()
        { }
    }
}

