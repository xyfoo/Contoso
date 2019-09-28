using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.Core.Enums
{
    public enum ValidationResult
    {
        MinimumAgeNotMet = -4,
        ParentAuthorizationRequired = -3,
        MissingInformationRequired = -2,
        Denied = -1,
        Success = 0
    }
}
