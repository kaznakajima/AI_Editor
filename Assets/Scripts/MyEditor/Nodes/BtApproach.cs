using UnityEngine;
using AI.BtGraph.Base;

namespace AI.BtGraph
{
    public class BtApproach : BtAction
    {
        /// <summary>
        /// 実行結果を返す
        /// </summary>
        /// <param name="_data">データ</param>
        /// <returns>実行結果</returns>
        public override BtResult Exec(Data _data)
        {
            Vector3 dir = _data.goal.CachedTransform.position - _data.mover.CachedTransform.position;
            _data.mover.CachedTransform.position += dir.normalized * 1.0f * Time.deltaTime;
            return BtResult.Success;
        }
    }
}
