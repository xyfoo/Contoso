using Contoso.Core;
using Contoso.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.ConsoleApp
{
    public class LocalConfiguration : IConfiguration
    {
        public EngineConfiguration Load()
        {
            // TODO: Actually you can load whatever you want here
            // whether it's from a server, from local file, from registry, from system preferences

            // I'm tired, so just always load the default config;
            return new EngineConfiguration();
        }

        public void Save(EngineConfiguration configuation)
        {
            // Not doing anything currently.
        }
    }
}
