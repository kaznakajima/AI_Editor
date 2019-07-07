using UnityEngine;

namespace AI.BtGraph.Base
{
    public class BtDecorator : Base.BtNode
    {
        // 前に接続
        [Input] public BtKnobEmpty inputKnob;
        // 次へ接続
        [Output] public BtKnobEmpty outputKnob;

        public BtDecorator() : base(BtNodeType.Decorator)
        { }

        // 結果を返す
        public override BtResult Exec(Data _data)
        {
            if (Branch(_data)) {
                Base.BtNode btNode = GetNext();
                if(btNode != null) {
                    return btNode.Exec(_data);
                }
                return BtResult.Success;
            }
            return BtResult.Failure;
        }

        public virtual bool Branch(Data _data)
        {
            return true;
        }
    }
}