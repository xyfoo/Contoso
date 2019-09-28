using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.Core
{
    public class EngineConfiguration
    {
        public int MinimumAge { get; set; } = 16;
        public int ParentAuthorizationAgeLimit { get; set; } = 18;

        // TODO: Put more attributes here
        // Example:
        //    Storage file location
        //    Spouse filename conflict resolution
        //    etc etc
    }
}
