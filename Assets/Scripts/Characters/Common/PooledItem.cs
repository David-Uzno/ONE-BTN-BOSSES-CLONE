using UnityEngine;

public class PooledItem : MonoBehaviour
{
    public ObjectPool Pool { get; private set; }
    private bool _isRecycling;

    public void SetPool(ObjectPool pool)
    {
        Pool = pool;
    }

    public void MarkAsRecycling()
    {
        _isRecycling = true;
    }

    public void ResetRecyclingFlag()
    {
        _isRecycling = false;
    }

    private void OnDisable()
    {
        if (Pool != null && !_isRecycling)
        {
            Pool.ReturnObjectToPool(transform);
        }
    }
}
