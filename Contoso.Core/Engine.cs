using Contoso.Core.Enums;
using Contoso.Core.Interfaces;
using Contoso.Core.Models;
using System;
using System.Threading.Tasks;

namespace Contoso.Core
{
    public class Engine: IDisposable
    {
        IConfiguration _configLoader;
        IStorage _storage;

        EngineConfiguration _configuration;

        public Engine(IConfiguration configLoader, IStorage storage)
        {
            _configLoader = configLoader;
            _storage = storage;

            _configuration = _configLoader.Load();

            _storage.Setup(_configuration);
        }

        /// <summary>
        /// Validate if a person record is ready for storage.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public ValidationResult Validate(Person person)
        {
            try
            {
                // Check for missing information
                if (person.IsBasicInformationProvided == false)
                {
                    return ValidationResult.MissingInformationRequired;
                }

                // Check for marital status
                if (person.MaritalStatus == MaritalStatus.Married && person.Spouse is null)
                {
                    return ValidationResult.MissingInformationRequired;
                }

                // check age
                if (person.IsMinimumAgeMet == false)
                {
                    return ValidationResult.MinimumAgeNotMet;
                }

                if (person.IsParentAuthorizationRequired && person.IsAuthorizedByParent == false)
                {
                    return ValidationResult.ParentAuthorizationRequired;
                }

                return ValidationResult.Success;
            }
            catch(Exception)
            {
                // TODO: Should sent some telemetry
                return ValidationResult.Denied;
            }
            
        }

        /// <summary>
        /// Save a record onto storge system.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task CreateAsync(Person person)
        {
            if(Validate(person) != ValidationResult.Success)
            {
                throw new Exception("Person validation failed");
            }

            await _storage.SaveAsync(person);
        }

        public bool IsBasicInformationProvided(Person person) => (string.IsNullOrWhiteSpace(person.FirstName) == false && string.IsNullOrWhiteSpace(person.Surname) == false);

        public bool IsMinimumAgeMet(Person person) => (person.Age >= _configuration.MinimumAge);

        public bool IsParentAuthorizationRequired(Person person) => (person.Age >= 16 && person.Age < 18);

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _storage.Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Engine()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
