public interface IFeatureInitBehavior<in T> where T : FeatureData
{
    Feature Init(Feature feature, T data);
}
