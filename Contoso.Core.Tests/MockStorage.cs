using System.Threading.Tasks;
using Contoso.Core.Interfaces;
using Contoso.Core.Models;

namespace Contoso.Core.Tests
{
    public class MockStorage : IStorage
    {
        public void Close()
        {
            // Do nothing
        }

        public async Task SaveAsync(Person person)
        {
            // Do nothing
        }

        public void Setup(EngineConfiguration configuration)
        {
            // Do nothing
        }
    }
}