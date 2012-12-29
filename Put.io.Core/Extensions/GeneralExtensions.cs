namespace Put.io.Core.Extensions
{
    public static class GeneralExtensions
    {
        public static void Copy<T>(T source, T target) where T : class
        {
            // Iterate the Properties of the destination instance and 
            // populate them from their source counterparts

            var targetProperties = target.GetType().GetProperties();
            foreach (var targetProperty in targetProperties)
            {
                if (!targetProperty.CanWrite)
                    continue;
                
                var sourceProperty = source.GetType().GetProperty(targetProperty.Name);

                if (!sourceProperty.CanRead)
                    continue;

                targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
            }
        }
    }
}