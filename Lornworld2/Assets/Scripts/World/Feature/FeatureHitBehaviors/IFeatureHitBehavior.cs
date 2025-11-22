public interface IFeatureHitBehavior<in T> where T : FeatureData
{
    void Hit(T data);
}
