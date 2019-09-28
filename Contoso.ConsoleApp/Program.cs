using Contoso.Core;
using Contoso.Core.Models;
using Contoso.Core.Enums;
using System;
using System.Threading.Tasks;

namespace Contoso.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string appName = "Contoso.ConsoleApp";
            bool createNewRecord = true;
            Person person;

            LocalConfiguration localConfig = new LocalConfiguration();
            LocalStorage localStorage = new LocalStorage();

            Engine engine = new Engine(localConfig, localStorage);

            Console.WriteLine(appName);

            while (createNewRecord == true)
            {
                person = new Person();

                #region Gather user input

                Console.WriteLine("Please enter your infomation");
                Console.WriteLine();

                person.FirstName = PromptAndValidate("First name", StringValidator);
                person.Surname = PromptAndValidate("Surname", StringValidator);
                person.DateOfBirth = PromptAndValidate("Date of Birth (DD-MM-YYYY)", DateValidator);
                if(person.IsMinimumAgeMet == true)
                {
                    if (person.IsParentAuthorizationRequired)
                    {
                        person.IsAuthorizedByParent = PromptAndValidate("Authorization from your parent (Y/N)", YesNoValidator);
                    }

                    person.MaritalStatus = PromptAndValidate("Married (Y/N)", YesNoValidator) ? MaritalStatus.Married : MaritalStatus.Single;
                    if (person.MaritalStatus == MaritalStatus.Married)
                    {
                        Person spouse = new Person();

                        spouse.FirstName = PromptAndValidate("Spouse - First name", StringValidator);
                        spouse.Surname = PromptAndValidate("Spouse - Surname", StringValidator);
                        spouse.DateOfBirth = PromptAndValidate("Spouse - Date of Birth (DD-MM-YYYY)", DateValidator);
                        spouse.MaritalStatus = MaritalStatus.Married;

                        person.Spouse = spouse;
                    }
                }

                #endregion

                var result = engine.Validate(person);

                #region Handle validation result

                if (result == ValidationResult.Success)
                {
                    await engine.CreateAsync(person);
                    Console.WriteLine("> SUCCESS: Save successfully");
                }
                else if (result == ValidationResult.MissingInformationRequired)
                {
                    Console.WriteLine("> FAILED: Some required information is missing!");
                }
                else if (result == ValidationResult.MinimumAgeNotMet)
                {
                    Console.WriteLine("> FAILED: You're too young to enroll");
                }
                else if (result == ValidationResult.ParentAuthorizationRequired)
                {
                    Console.WriteLine("> FAILED: Authorization from parent required");
                }
                else if (result == ValidationResult.Denied)
                {
                    Console.WriteLine("> FAILED: For some reason you're not allowed to enroll");
                }
                else
                {
                    Console.WriteLine($"> FAILED: Unknown response: {result}");
                }
                Console.WriteLine();

                #endregion

                createNewRecord = PromptAndValidate("Create a new record (Y/N)", YesNoValidator);

                Console.Clear();
                Console.WriteLine(appName);
            }
        }

        /// <summary>
        /// Prompt user for information, and run the input against the validator.
        /// User wil be reprompt if validation failed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">Question to asl</param>
        /// <param name="validator">Func to validate the input. Return null/throw error if failed.</param>
        /// <returns></returns>
        static T PromptAndValidate<T>(string query, Func<string, T> validator)
        {
            var isValidInput = false;
            object valResult = null;

            while (isValidInput == false)
            {
                try
                {
                    Console.Write($"? {query}: ");
                    var rawInput = Console.ReadLine().Trim();

                    valResult = validator(rawInput);
                    isValidInput = (valResult is T);
                }
                catch (Exception)
                {
                    // Assuming any failure in the validator requires re-entry by user
                    isValidInput = false;
                }
            }

            return (T)valResult;
        }

        /// <summary>
        /// Validate user input as string. Fail if input is empty.
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns>Validated & trimmed string input.</returns>
        static string StringValidator(string userInput) => string.IsNullOrWhiteSpace(userInput) ? null : userInput;

        /// <summary>
        /// Validate user input as string. Fail if input is invalid according to the date format.
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns>Validated date time offset</returns>
        static DateTimeOffset DateValidator(string userInput)
        {
            try
            {
                var dtFormat = "d-M-yyyy";
                return DateTimeOffset.ParseExact(userInput, dtFormat, null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Validate user input as boolean. Failed if user didn't enter the required characters.
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns>Validate boolean</returns>
        static bool YesNoValidator(string userInput)
        {
            if (string.Compare(userInput, "y", true) == 0 || string.Compare(userInput, "yes", true) == 0)
            {
                return true;
            }
            else if (string.Compare(userInput, "n", true) == 0 || string.Compare(userInput, "No", true) == 0)
            {
                return false;
            }
            else
            {
                throw new Exception("Not a recognizable selection");
            }
        }
    }
}
