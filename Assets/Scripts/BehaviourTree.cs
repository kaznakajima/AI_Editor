using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace BehaviourTrees
{
    /// <summary>
    /// BehaviourTreeクラス
    /// </summary>
    public class BehaviourTree : MonoBehaviour
    {
        BehaviourTreeInstance node;
        
        void Start()
        {
            var attackNode = new DecoratorNode(IsHpLessThan, new ActionNode(AttackEnemy));

            var moveTowerNode = new SequencerNode(new BehaviourTreeBase[] {
                new ActionNode(MoveNearTower),
                new ActionNode(Wait)
            });

            var rootNode = new SelectorNode(new BehaviourTreeBase[] {
                attackNode,
                moveTowerNode
            });

            node = new BehaviourTreeInstance(rootNode);
            node.finishRP.Where(item => item != BehaviourTreeInstance.NodeState.READY)
                .Subscribe(item => ResetCoroutineStart());
            node.Excute();
        }

        private ExecutionResult IsHpLessThan(BehaviourTreeInstance _instance)
        {
            var enemyHp = Random.Range(0, 10);
            if(enemyHp <= 4) {
                Debug.Log("敵のHPが4以下。チャンス。");
                return new ExecutionResult(true);
            }
            Debug.Log("敵のHPが4より大きい。まだ慌てる時間じゃない。");
            return new ExecutionResult(false);
        }

        private ExecutionResult AttackEnemy(BehaviourTreeInstance _instance)
        {
            Debug.Log("敵を攻撃する");

            return new ExecutionResult(true);
        }

        private ExecutionResult MoveNearTower(BehaviourTreeInstance instance)
        {

            Debug.Log("一番近くのタワーにいく。");

            return new ExecutionResult(true);
        }

        private ExecutionResult Wait(BehaviourTreeInstance instance)
        {

            Debug.Log("待機。");

            return new ExecutionResult(true);
        }

        void ResetCoroutineStart()
        {
            StartCoroutine(WaitCoroutine());
        }

        IEnumerator WaitCoroutine()
        {
            yield return new WaitForSeconds(1.5f);
            node.Reset();
        }
    }
}

