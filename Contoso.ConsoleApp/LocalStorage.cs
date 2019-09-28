using Contoso.Core;
using Contoso.Core.Enums;
using Contoso.Core.Interfaces;
using Contoso.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Contoso.ConsoleApp
{
    public class LocalStorage : IStorage
    {
        EngineConfiguration _configuration;
        string _defaultFileName = "People.txt";
        string _defaultSpousesFolderName = "Spouses";

        string _folderFullPath = "";
        string _peopleFullPath = "";
        string _spousesFolderPath = "";

        public void Setup(EngineConfiguration configuration)
        {
            _configuration = configuration;

            _folderFullPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
            _peopleFullPath = Path.Combine(_folderFullPath, _defaultFileName);
            _spousesFolderPath = Path.Combine(_folderFullPath, _defaultSpousesFolderName);

            if (File.Exists(_peopleFullPath) == false)
            {
                File.Create(_peopleFullPath).Close();
            }

            if (Directory.Exists(_spousesFolderPath) == false)
            {
                Directory.CreateDirectory(_spousesFolderPath);
            }
        }

        public void Close()
        {
            // Do nothing
        }

        public async Task SaveAsync(Person person)
        {
            string content = PersonInfoToText(person);
            string spouseFileFullPath = "null";

            if (person.MaritalStatus == MaritalStatus.Married)
            {
                string spouseFileNameWoExtension = $"{person.Spouse.FirstName}{person.Spouse.Surname}";
                spouseFileFullPath = Path.Combine(_defaultSpousesFolderName, $"{spouseFileNameWoExtension}.txt");

                if(File.Exists(spouseFileFullPath))
                {
                    // Someone with the same name!!!
                    // Use guid to differentiate
                    spouseFileNameWoExtension = $"{spouseFileNameWoExtension}_{Guid.NewGuid().ToString()}";
                    spouseFileFullPath = Path.Combine(_defaultSpousesFolderName, $"{spouseFileNameWoExtension}.txt");
                }

                // Create spouse file first
                await File.WriteAllTextAsync(spouseFileFullPath, PersonInfoToText(person.Spouse));
            }

            content += $"|{spouseFileFullPath}";
            await File.AppendAllLinesAsync(_peopleFullPath, new[] { content });
        }

        private string PersonInfoToText(Person p) => $"{p.FirstName}|{p.Surname}|{p.DateOfBirth.ToString("dd-MMM-yyyy")}|{p.MaritalStatus}";
    }
}
