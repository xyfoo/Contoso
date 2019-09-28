using System.Diagnostics;
using System.Threading.Tasks;
using Contoso.Core.Models;
using Contoso.Core.Enums;
using NUnit.Framework;
using System;
using Contoso.Core.Models.Interfaces;

namespace Contoso.Core.Tests
{
    public class EngineTests
    {
        [Test]
        [TestCaseSource(typeof(EngineTestsDataSet), "Engine_IsMinimumAgeMet")]
        public bool Engine_IsMinimumAgeMet(Person person)
        {
            Engine engine = new Engine(new MockConfigurationLoader(), new MockStorage());
            return engine.IsMinimumAgeMet(person);
        }

        [Test]
        [TestCaseSource(typeof(EngineTestsDataSet), "Engine_IsParentAuthorizationRequired")]
        public bool Engine_IsParentAuthorizationRequired(Person person)
        {
            Engine engine = new Engine(new MockConfigurationLoader(), new MockStorage());
            return engine.IsParentAuthorizationRequired(person);
        }

        [Test]
        [TestCaseSource(typeof(EngineTestsDataSet), "Validate_AllInformationProvided_Success")]
        public ValidationResult Validate_AllInformationProvided_Success(Person person)
        {
            Engine engine = new Engine(new MockConfigurationLoader(), new MockStorage());
            return engine.Validate(person);
        }

        [Test]
        [TestCaseSource(typeof(EngineTestsDataSet), "Validate_MissingBasicInformation_MissingInfoRequired")]
        public ValidationResult Validate_MissingBasicInformation_MissingInfoRequired(Person person)
        {
            Engine engine = new Engine(new MockConfigurationLoader(), new MockStorage());
            return engine.Validate(person);
        }

        [Test]
        [TestCaseSource(typeof(EngineTestsDataSet), "Validate_MarriedButNoSpouseInformation_MissingInfoRequired")]
        public ValidationResult Validate_MarriedButNoSpouseInformation_MissingInfoRequired(Person person)
        {
            Engine engine = new Engine(new MockConfigurationLoader(), new MockStorage());
            return engine.Validate(person);
        }

        [Test]
        [TestCaseSource(typeof(EngineTestsDataSet), "Validate_YoungerThan16YearsOld_MinimumAgeNotMet")]
        public ValidationResult Validate_YoungerThan16YearsOld_MinimumAgeNotMet(Person person)
        {
            Engine engine = new Engine(new MockConfigurationLoader(), new MockStorage());
            return engine.Validate(person);
        }

        [Test]
        [TestCaseSource(typeof(EngineTestsDataSet), "Validate_Between16And18YearsOldNoParentAuthorization_ParentAuthorizationRequired")]
        public ValidationResult Validate_Between16And18YearsOldNoParentAuthorization_ParentAuthorizationRequired(Person person)
        {
            Engine engine = new Engine(new MockConfigurationLoader(), new MockStorage());
            return engine.Validate(person);
        }

        [Test]
        [TestCaseSource(typeof(EngineTestsDataSet), "Validate_Between16And18YearsOldWithParentAuthorization_Success")]
        public ValidationResult Validate_Between16And18YearsOldWithParentAuthorization_Success(Person person)
        {
            Engine engine = new Engine(new MockConfigurationLoader(), new MockStorage());
            return engine.Validate(person);
        }
    }
}