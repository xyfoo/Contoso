using Contoso.Core.Enums;
using Contoso.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contoso.Core.Interfaces
{
    /// <summary>
    /// Interface for classes support persistent storing mechanicsm
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Interface to set up the storage system.
        /// E.g Establish db connection client (windows), asking for permission (iOS/Android).
        /// </summary>
        /// <returns></returns>
        void Setup(EngineConfiguration configuration);

        /// <summary>
        /// Tear down the storage system.
        /// </summary>
        void Close();

        /// <summary>
        /// Save a record onto storage syste,.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Task SaveAsync(Person person);
    }
}
