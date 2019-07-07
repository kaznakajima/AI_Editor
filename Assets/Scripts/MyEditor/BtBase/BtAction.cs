using UnityEngine;

namespace AI.BtGraph.Base
{
    public class BtAction : Base.BtNode
    {
        // 前に接続
        [Input(ShowBackingValue.Never, ConnectionType.Override)] public BtKnobEmpty inputKnob;

        public BtAction() : base (BtNodeType.Action)
        { }
    }
}

