using Contoso.Core.Enums;
using Contoso.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.Core.Models
{

    public class Person: ITestableDate
    {
        DateTimeOffset? _referenceDate = null;
        DateTimeOffset ITestableDate.ReferenceDate
        {
            get => _referenceDate.HasValue ? _referenceDate.Value : DateTimeOffset.Now;
            set => _referenceDate = value;
        }

        public string FirstName { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public DateTimeOffset DateOfBirth { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public Person? Spouse { get; set; } = null;
        public bool IsAuthorizedByParent { get; set; } = false;


        // Computed
        public int Age => CalculateAge((this as ITestableDate).ReferenceDate, DateOfBirth);
        public bool IsBasicInformationProvided => (string.IsNullOrWhiteSpace(FirstName) == false && string.IsNullOrWhiteSpace(Surname) == false);
        public bool IsMinimumAgeMet => (Age >= 16);
        public bool IsParentAuthorizationRequired => (Age >= 16 && Age < 18);


        /// <summary>
        /// Calculate the age
        /// </summary>
        /// <param name="referenceDate">Base date which calculation will be performed on</param>
        /// <param name="targetDate"></param>
        /// <returns></returns>
        int CalculateAge(DateTimeOffset referenceDate, DateTimeOffset targetDate)
        {
            var dateOfBirthSynced = targetDate.ToOffset(referenceDate.Offset);
            var age = referenceDate.Year - dateOfBirthSynced.Year;

            if (dateOfBirthSynced.Date.AddYears(age) > referenceDate.Date)
            {
                // this is to catch people who haven't really complete one full year
                // born 31 dec, if cut off date is 1 Jan, we won't consider that as 1 year old
                age--;
            }
            return age;
        }
    }
}
