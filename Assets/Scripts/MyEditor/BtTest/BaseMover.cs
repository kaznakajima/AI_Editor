using AI.BtGraph;
using UnityEngine;

public class BaseMover : MonoBehaviour
{
    // キャッシュされた位置情報
    private Transform cachedTransform;
    public Transform CachedTransform
    {
        get
        {
            if(cachedTransform == null) {
                cachedTransform = transform;
            }
            return cachedTransform;
        }
    }
}
