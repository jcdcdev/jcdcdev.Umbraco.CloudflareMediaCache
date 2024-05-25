using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace jcdcdev.Umbraco.CloudflareMediaCache.Composing;

public class CloudflareMediaCacheComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.AddCloudflareMediaCache();
    }
}
