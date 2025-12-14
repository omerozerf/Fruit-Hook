namespace _Game._Scripts.ObjectPoolSystem
{
    /// <summary>
    /// Optional hook for pooled objects (SwordBubble etc.)
    /// </summary>
    public interface IPoolable
    {
        void OnSpawnedFromPool();
        void OnDespawnedToPool();
    }
}