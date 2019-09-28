using System.Diagnostics;
using System.Threading.Tasks;
using Contoso.Core.Models;
using Contoso.Core.Enums;
using NUnit.Framework;
using System;
using Contoso.Core.Models.Interfaces;

namespace Contoso.Core.Tests
{
    public class PersonTests
    {
        [Test]
        [TestCaseSource(typeof(PersonTestsDataSet), "Person_Age")]
        public int Person_Age(DateTimeOffset dob)
        {
            // Assuming we're in 1-JAN-2000
            Person person = new Person { DateOfBirth = dob };
            (person as ITestableDate).ReferenceDate = new DateTimeOffset(new DateTime(2000, 1, 1), new TimeSpan(0));

            return person.Age;
        }
    }
}