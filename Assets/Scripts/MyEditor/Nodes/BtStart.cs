namespace AI.BtGraph
{
    public class BtStart : Base.BtNode
    {
        // 次へ接続
        [Output(ShowBackingValue.Never, ConnectionType.Override)] public BtKnobEmpty outputKnob;

        public BtStart() : base(BtNodeType.Start)
        { }

        /// <summary>
        /// 実行結果を返す
        /// </summary>
        /// <param name="_data">データ</param>
        /// <returns>実行結果</returns>
        public override BtResult Exec(Data _data)
        {
            var next = GetNext();
            if(next != null) {
                return next.Exec(_data);
            }
            return BtResult.Failure;
        }
    }
}

