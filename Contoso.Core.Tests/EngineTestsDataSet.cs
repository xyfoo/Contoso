using System;
using System.Collections;
using Contoso.Core.Enums;
using Contoso.Core.Models;
using Contoso.Core.Models.Interfaces;
using NUnit.Framework;

namespace Contoso.Core.Tests
{
    public class EngineTestsDataSet
    {
        /// <summary>
        /// Generate person which date of reference is set to 01-JAN-2000.
        /// </summary>
        /// <param name="dobYear"></param>
        /// <param name="dobMonth"></param>
        /// <param name="dobDay"></param>
        /// <returns></returns>
        private static Person Person2000Factory(int dobYear, int dobMonth, int dobDay)
        {
            var p = new Person()
            {
                DateOfBirth = new DateTimeOffset(new DateTime(dobYear, dobMonth, dobDay), new TimeSpan(0))
            };
            (p as ITestableDate).ReferenceDate = new DateTimeOffset(new DateTime(2000, 1, 1), new TimeSpan(0));
            return p;
        }

        public static IEnumerable Engine_IsMinimumAgeMet
        {
            get
            {
                // Minimum age is 16
                yield return new TestCaseData(Person2000Factory(1999, 1, 1)).Returns(false); // 1 years
                yield return new TestCaseData(Person2000Factory(1986, 1, 1)).Returns(false); // 14 years
                yield return new TestCaseData(Person2000Factory(1985, 1, 1)).Returns(false); // 15 years
                yield return new TestCaseData(Person2000Factory(1984, 1, 1)).Returns(true); // 16 years
                yield return new TestCaseData(Person2000Factory(1983, 1, 1)).Returns(true); // 17 years
                yield return new TestCaseData(Person2000Factory(1900, 1, 1)).Returns(true); // 100 years
            }
        }

        public static IEnumerable Engine_IsParentAuthorizationRequired
        {
            get
            {
                // Between 16 to 18
                yield return new TestCaseData(Person2000Factory(1986, 1, 1)).Returns(false); // 14 years
                yield return new TestCaseData(Person2000Factory(1985, 1, 1)).Returns(false); // 15 years
                yield return new TestCaseData(Person2000Factory(1984, 1, 1)).Returns(true); // 16 years
                yield return new TestCaseData(Person2000Factory(1983, 1, 1)).Returns(true); // 17 years
                yield return new TestCaseData(Person2000Factory(1982, 1, 1)).Returns(false); // 18 years
                yield return new TestCaseData(Person2000Factory(1981, 1, 1)).Returns(false); // 19 years
                yield return new TestCaseData(Person2000Factory(1900, 1, 1)).Returns(false); // 100 years
            }
        }

        public static IEnumerable Validate_AllInformationProvided_Success
        {
            get
            {
                // Presuming we're in 2019
                Func<int, int, int, Person> personGenerator = (int y, int m, int d) =>
                {
                    var zeroOffset = new TimeSpan(0);
                    var p = new Person { FirstName = "John", Surname = "Doe", DateOfBirth = new DateTimeOffset(new DateTime(y, m, d), zeroOffset) };
                    (p as ITestableDate).ReferenceDate = new DateTimeOffset(new DateTime(2019, 1, 1), zeroOffset);
                    return p;
                };

                var personSingle19 = personGenerator(2000, 1, 1);
                personSingle19.MaritalStatus = MaritalStatus.Single;
                yield return new TestCaseData(personSingle19).Returns(ValidationResult.Success);

                var personSingle17 = personGenerator(2002, 1, 1);
                personSingle17.MaritalStatus = MaritalStatus.Single;
                personSingle17.IsAuthorizedByParent = true;
                yield return new TestCaseData(personSingle17).Returns(ValidationResult.Success);

                var personMarried = personGenerator(2000, 1, 1);
                personMarried.MaritalStatus = MaritalStatus.Married;
                personMarried.Spouse = new Person
                {
                    FirstName = "Jane",
                    Surname = "Doe",
                    DateOfBirth = new DateTime(2000, 1, 1)
                };
                yield return new TestCaseData(personMarried).Returns(ValidationResult.Success);
            }
        }

        public static IEnumerable Validate_MissingBasicInformation_MissingInfoRequired
        {
            get
            {
                yield return new TestCaseData(new Person()).Returns(ValidationResult.MissingInformationRequired);
                yield return new TestCaseData(new Person { Surname = "Doe", DateOfBirth = new DateTime(2000, 1, 1) }).Returns(ValidationResult.MissingInformationRequired);
                yield return new TestCaseData(new Person { FirstName = "John", DateOfBirth = new DateTime(2000, 1, 1) }).Returns(ValidationResult.MissingInformationRequired);
            }
        }

        public static IEnumerable Validate_MarriedButNoSpouseInformation_MissingInfoRequired
        {
            get
            {
                yield return new TestCaseData(new Person { FirstName = "John", Surname = "Doe", DateOfBirth = new DateTime(2000, 1, 1), MaritalStatus = Enums.MaritalStatus.Married }).Returns(ValidationResult.MissingInformationRequired);
            }
        }

        public static IEnumerable Validate_YoungerThan16YearsOld_MinimumAgeNotMet
        {
            get
            {
                // Presuming we're in 2019
                Func<int, int, int, Person> personGenerator = (int y, int m, int d) =>
                {
                    var zeroOffset = new TimeSpan(0);
                    var p = new Person { FirstName = "John", Surname = "Doe", DateOfBirth = new DateTimeOffset(new DateTime(y, m, d), zeroOffset) };
                    (p as ITestableDate).ReferenceDate = new DateTimeOffset(new DateTime(2019, 1, 1), zeroOffset);
                    return p;
                };

                yield return new TestCaseData(personGenerator(2004,1,1)).Returns(ValidationResult.MinimumAgeNotMet);
                yield return new TestCaseData(personGenerator(2005,1,1)).Returns(ValidationResult.MinimumAgeNotMet);
                yield return new TestCaseData(personGenerator(2016,1,1)).Returns(ValidationResult.MinimumAgeNotMet);
            }
        }

        public static IEnumerable Validate_Between16And18YearsOldNoParentAuthorization_ParentAuthorizationRequired
        {
            get
            {
                // Presuming we're in 2019
                Func<int, int, int, Person> personGenerator = (int y, int m, int d) =>
                {
                    var zeroOffset = new TimeSpan(0);
                    var p = new Person { FirstName = "John", Surname = "Doe", DateOfBirth = new DateTimeOffset(new DateTime(y, m, d), zeroOffset), IsAuthorizedByParent = false };
                    (p as ITestableDate).ReferenceDate = new DateTimeOffset(new DateTime(2019, 1, 1), zeroOffset);
                    return p;
                };

                yield return new TestCaseData(personGenerator(2002, 12, 31)).Returns(ValidationResult.ParentAuthorizationRequired);
                yield return new TestCaseData(personGenerator(2002, 1, 1)).Returns(ValidationResult.ParentAuthorizationRequired);
                yield return new TestCaseData(personGenerator(2003, 1, 1)).Returns(ValidationResult.ParentAuthorizationRequired);
            }
        }

        public static IEnumerable Validate_Between16And18YearsOldWithParentAuthorization_Success
        {
            get
            {
                // Presuming we're in 2019
                Func<int, int, int, Person> personGenerator = (int y, int m, int d) =>
                {
                    var zeroOffset = new TimeSpan(0);
                    var p = new Person { FirstName = "John", Surname = "Doe", DateOfBirth = new DateTimeOffset(new DateTime(y, m, d), zeroOffset), IsAuthorizedByParent = true };
                    (p as ITestableDate).ReferenceDate = new DateTimeOffset(new DateTime(2019, 1, 1), zeroOffset);
                    return p;
                };

                yield return new TestCaseData(personGenerator(2002, 12, 31)).Returns(ValidationResult.Success);
                yield return new TestCaseData(personGenerator(2002, 1, 1)).Returns(ValidationResult.Success);
                yield return new TestCaseData(personGenerator(2003, 1, 1)).Returns(ValidationResult.Success);
            }
        }
    }
}