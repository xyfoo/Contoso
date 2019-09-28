using System;
using System.Collections;
using Contoso.Core.Models;
using NUnit.Framework;

namespace Contoso.Core.Tests
{
    public class PersonTestsDataSet
    {
        public static IEnumerable Person_Age
        {
            get
            {
                // Data is based on the assumption that reference date is 1-JAN-2000
                var zeroOffset = new TimeSpan(0);

                yield return new TestCaseData(new DateTimeOffset(new DateTime(1999, 12, 31), zeroOffset)).Returns(0);
                yield return new TestCaseData(new DateTimeOffset(new DateTime(1999, 1, 1), zeroOffset)).Returns(1);
                yield return new TestCaseData(new DateTimeOffset(new DateTime(1998, 1, 1), zeroOffset)).Returns(2);
                yield return new TestCaseData(new DateTimeOffset(new DateTime(1990, 1, 1), zeroOffset)).Returns(10);
                yield return new TestCaseData(new DateTimeOffset(new DateTime(1984, 1, 1), zeroOffset)).Returns(16);
                yield return new TestCaseData(new DateTimeOffset(new DateTime(1982, 12, 31), zeroOffset)).Returns(17);
                yield return new TestCaseData(new DateTimeOffset(new DateTime(1982, 1, 1), zeroOffset)).Returns(18);
                yield return new TestCaseData(new DateTimeOffset(new DateTime(1981, 1, 1), zeroOffset)).Returns(19);
            }
        }

        
    }
}