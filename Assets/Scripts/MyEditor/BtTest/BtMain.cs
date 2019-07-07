using UnityEngine;

public class Data
{
    public Goal goal;
    public Mover mover;
}

public class BtMain : MonoBehaviour
{
    public Goal goal;
    public Mover mover;

    private Data data;

    /// <summary>
    /// 必要なデータを取得
    /// </summary>
    private void Start()
    {
        data = new Data();
        data.goal = goal;
        data.mover = mover;

        mover.Init(data);
    }
}
