using UnityEngine;
using AI.BtGraph;

public class Mover : BaseMover
{
    [SerializeField]
    private BtGraph btGraph;
    private Data data;

    // 初期化処理
    public void Init(Data _data)
    {
        data = _data;
    }
    
    // 更新処理
    private void Update()
    {
        if(btGraph != null && data != null) {
            btGraph.Exec(data);
        }
    }
}
