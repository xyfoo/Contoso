using Contoso.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.Core.Tests
{
    public class MockConfigurationLoader : IConfiguration
    {
        public EngineConfiguration Load()
        {
            // Always return default configuration
            return new EngineConfiguration();
        }

        public void Save(EngineConfiguration configuation)
        {
            // Do nothing
        }
    }
}
