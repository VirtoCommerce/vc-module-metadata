using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.MetadataModule.Core.Services;
using VirtoCommerce.MetadataModule.Data.Services;
using VirtoCommerce.Platform.Core.Modularity;


namespace VirtoCommerce.MetadataModule.Web
{
    public class Module : IModule
    {
        public ManifestModuleInfo ModuleInfo { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(typeof(IMetadataService), new MetadataService());

        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
        }

        public void Uninstall()
        {
            //Nothing special here
        }


    }
}
