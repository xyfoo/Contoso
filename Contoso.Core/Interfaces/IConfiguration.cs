using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.Core.Interfaces
{
    public interface IConfiguration
    {
        /// <summary>
        /// Load engine configuration
        /// </summary>
        /// <returns></returns>
        EngineConfiguration Load();

        /// <summary>
        /// Save engine configuration
        /// </summary>
        /// <param name="configuation"></param>
        void Save(EngineConfiguration configuation);
    }
}
