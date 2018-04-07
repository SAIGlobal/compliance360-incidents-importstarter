using System;
using System.IO;
using System.Collections.Generic;
using CsvHelper;
using Compliance360.IncidentsImportStarter.API;

namespace Compliance360.IncidentsImportStarter
{
    public class IncidentImporter : IIncidentImporter
    {
        private const string KeyFilePath = "--filepath";
        private const string KeyBaseUri = "--baseuri";
        private const string KeyOrganization = "--organization";
        private const string KeyUserName = "--username";
        private const string KeyPassword = "--password";


        private IApiClient ApiClient { get; set; }

        public IncidentImporter(IApiClient apiClient)
        {
            ApiClient = apiClient;
        }

        public void Import(Dictionary<string, string> importArgs)
        {
            if (!importArgs.ContainsKey(KeyFilePath))
            {
                throw new ArgumentException("Missing argument [--filepath]");
            }
            if (!importArgs.ContainsKey(KeyBaseUri))
            {
                throw new ArgumentException("Missing argument [--baseuri]");
            }
            if (!importArgs.ContainsKey(KeyOrganization))
            {
                throw new ArgumentException("Missing argument [--organization]");
            }
            if (!importArgs.ContainsKey(KeyUserName))
            {
                throw new ArgumentException("Missing argument [--username]");
            }
            if (!importArgs.ContainsKey(KeyPassword))
            {
                throw new ArgumentException("Missing argument [--password]");
            }
            

            // authenticate with the API
            var token = ApiClient.Authenticate(
                importArgs[KeyBaseUri],
                importArgs[KeyOrganization],
                importArgs[KeyUserName],
                importArgs[KeyPassword]);

            // read the contents of the csv file
            using (var csvDataStream = File.OpenText(importArgs[KeyFilePath]))
            {
                var csv = new CsvReader(csvDataStream);
                
                var incidentsToImport = csv.GetRecords<Incident>();
                foreach(var incident in incidentsToImport)
                {
                    ImportIncident(incident, token);
                }
            }
        }

        /// <summary>
        /// Imports an incident using the Compliance 360 API
        /// </summary>
        /// <param name="incident">The Incident to import.</param>
        /// <param name="token">The authentication token.</param>
        public void ImportIncident(Incident incident, string token)
        {
            // find the division where the incident should be created
            // we are hard-coding this value just for demonstration purposes
            // it could be looked up dynamically based on incident content.
            var divisionId = ApiClient.GetDivisionId(token, "Main Division");
            if (divisionId == null)
            {
                throw new ApplicationException("Could not find the specified Division.");
            }

            // find the folder where the incident should be created
            // we are hard-coding the folder name for demonstration purposes
            // but this value could be based on the incident data being imported.
            var folderId = ApiClient.GetFolderId(token, divisionId, "Incidents");
            if (folderId == null)
            {
                throw new ApplicationException("Could not find the specified Folder.");
            }

            // get the default workflow template
            var workflowTemplateId = ApiClient.GetDefaultIncidentWorkflowTemplateId(token);
            if (workflowTemplateId == null)
            {
                throw new ApplicationException("Could not find the default workflow template.");
            }

            // find the resp party
            var nameSegments = incident.RespParty.Split(' ');
            var firstName = nameSegments[0];
            var lastName = nameSegments[1];
            var respPartId = ApiClient.GetEmployeeId(token, firstName, lastName);

            // create the new incident object
            var incidentId = ApiClient.CreateIncident(
                token,
                divisionId,
                folderId,
                workflowTemplateId,
                respPartId,
                incident.Name,
                incident.Description,
                incident.IncidentDate
            );
        }
    }
}
