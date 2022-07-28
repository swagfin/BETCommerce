using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;

namespace BetCommerce.API.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void UseProcessorInitializables(this IApplicationBuilder builder)
        {
            var processorService = (IEnumerable<IProcessorInitializable>)builder.ApplicationServices.GetService(typeof(IEnumerable<IProcessorInitializable>));
            if (processorService != null)
                foreach (IProcessorInitializable processor in processorService)
                    processor.Initialize();
        }
    }
}
