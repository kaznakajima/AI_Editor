using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTrees
{
    // 番号
    public class UniqNum { public static int num = 0; }

    public abstract class BehaviourTreeBase
    {
        // 初期化
        protected virtual void Init() {
            var intNum = UniqNum.num++;
            key = ToString() + intNum.ToString();
        }

        // ディスプレイネーム
        public string key {
            private set; get;
        }

        // 更新状態の確認
        public abstract ExecutionResult Execute(BehaviourTreeInstance _instance);

        // リセット
        public abstract void Reset();
    }
}
