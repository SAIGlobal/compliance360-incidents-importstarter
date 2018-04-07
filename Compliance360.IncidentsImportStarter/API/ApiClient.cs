using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using Newtonsoft.Json;

namespace Compliance360.IncidentsImportStarter.API
{
    public class ApiClient : IApiClient
    {
        private RestClient _client = null;
        private const string UriLogin = "/API/2.0/Security/Login";
        private const string UriLogout = "/API/2.0/Security/Logout";
        private const string UriOrganizationHost = "/API/2.0/Security/OrganizationHost?organization={0}";
        private const string UriGetDivisionId = "/API/2.0/Data/EmployeeManagement/EmployeeDivision/Default?take=1&where=Path='{0}'&Token={1}";
        private const string UriGetFolderId = "/API/2.0/Data/Global/Folders/Default?take=1&where=Name='{0}';Division='{1}'&Token={2}";
        private const string UriGetEmployeeId = "/API/2.0/Data/EmployeeManagement/Employee/Default?take=1&where=FirstName='{0}';LastName='{1}'&Token={2}";
        private const string UriCreateIncident = "/API/2.0/Data/Incidents/MetaIncident/Default?Token={0}";
        private const string UriGetDefaultIncidentWorkflowTemplateId = "/API/2.0/Data/Global/WorkflowTemplates/MetaIncident?take=1&where=IsDefault='True'&token={0}";

        public string Authenticate(string baseUri, string organization, string userName, string password)
        {
            InitializeRestClient(baseUri, organization);

            var req = new RestRequest(UriLogin, Method.POST);

            req.AddHeader("Accept", "application/json");
            req.AddHeader("Content-Type", "application/json");
            req.AddJsonBody(new LoginRequest
            {
                Organization  = organization,
                Username = userName,
                Password = password,
                Culture = "en-US"
            });
            
            var resp = _client.Execute(req);
            if (resp.ErrorException != null)
            {
                throw resp.ErrorException;
            }

            var result = Encoding.Unicode.GetString(resp.RawBytes);
            var auth = JsonConvert.DeserializeObject<LoginResponse>(result);

            return auth.Token;
        }

        public EntityId CreateIncident(
                string token,
                EntityId divisionId,
                EntityId folderId,
                EntityId workflowTemplateId,
                EntityId respPartId,
                string name,
                string description,
                DateTime incidentDate) 
        {
            var req = new RestRequest(string.Format(UriCreateIncident, token));
            req.AddHeader("Accept", "application/json");
            req.AddHeader("Content-Type", "application/json;");
            req.Method = Method.POST;
            var json = JsonConvert.SerializeObject(new
            {
                Folder = folderId,
                WorkflowTemplate = workflowTemplateId,
                RespParty = respPartId,
                Name = name,
                Description = description,
                DateReceived = incidentDate
            });
            req.AddParameter("text/json", json, ParameterType.RequestBody);
            
            var resp = _client.Execute(req);
            if (resp.ErrorException != null)
            {
                throw resp.ErrorException;
            }

            var result = Encoding.Unicode.GetString(resp.RawBytes);
            var response = JsonConvert.DeserializeObject<CreateIncidentResponse>(result);

            return response.Id;
        }

        public EntityId GetDefaultIncidentWorkflowTemplateId(string token)
        {
            var req = new RestRequest(string.Format(UriGetDefaultIncidentWorkflowTemplateId, token));
            req.AddHeader("Accept", "application/json");
            req.AddHeader("Content-Type", "application/json;");
            var resp = _client.Execute(req);
            if (resp.ErrorException != null)
            {
                throw resp.ErrorException;
            }

            var result = Encoding.Unicode.GetString(resp.RawBytes);
            var responseData = JsonConvert.DeserializeObject<GetResponse<WorkflowTemplateResponse>>(result);

            if (responseData.Data.Count > 0)
            {
                return responseData.Data[0].Id;
            }

            return null;
        }

        public EntityId GetDivisionId(string token, string divisionName)
        {
            var req = new RestRequest(string.Format(UriGetDivisionId, divisionName, token));
            req.AddHeader("Accept", "application/json");
            req.AddHeader("Content-Type", "application/json;");
            var resp = _client.Execute(req);
            if (resp.ErrorException != null)
            {
                throw resp.ErrorException;
            }

            var result = Encoding.Unicode.GetString(resp.RawBytes);
            var responseData = JsonConvert.DeserializeObject<GetResponse<DivisionResponse>>(result);

            if (responseData.Data.Count > 0)
            {
                return responseData.Data[0].Id;
            }

            return null;
        }


        public EntityId GetEmployeeId(string token, string firstName, string lastName)
        {
            var req = new RestRequest(string.Format(UriGetEmployeeId, firstName, lastName, token));
            req.AddHeader("Accept", "application/json");
            req.AddHeader("Content-Type", "application/json;");
            var resp = _client.Execute(req);
            if (resp.ErrorException != null)
            {
                throw resp.ErrorException;
            }

            var result = Encoding.Unicode.GetString(resp.RawBytes);
            var responseData = JsonConvert.DeserializeObject<GetResponse<EmployeeResponse>>(result);

            if (responseData.Data.Count > 0)
            {
                return responseData.Data[0].Id;
            }

            return null;
        }

        public EntityId GetFolderId(string token, EntityId divisionId, string folderName)
        {
            var req = new RestRequest(string.Format(UriGetFolderId, folderName, divisionId.Token, token));
            req.AddHeader("Accept", "application/json");
            req.AddHeader("Content-Type", "application/json;");
            var resp = _client.Execute(req);
            if (resp.ErrorException != null)
            {
                throw resp.ErrorException;
            }

            var result = Encoding.Unicode.GetString(resp.RawBytes);
            var responseData = JsonConvert.DeserializeObject<GetResponse<FolderResponse>>(result);

            if (responseData.Data.Count > 0)
            {
                return responseData.Data[0].Id;
            }

            return null;
        }

        public void InitializeRestClient(string baseUri, string organization)
        {
            _client = new RestClient(baseUri);

            // first make the request to get the host address for the api for the 
            // given organization
            var req = new RestRequest(string.Format(UriOrganizationHost, organization), Method.GET);
            req.AddHeader("Accept", "application/json");
            req.AddHeader("Content-Type", "application/json;");
            var resp = _client.Execute(req);
            if (resp.ErrorException != null)
            {
                throw resp.ErrorException;
            }

            var result = Encoding.Unicode.GetString(resp.RawBytes);
            var orgHost = JsonConvert.DeserializeObject<OrganizationHostResponse>(result);

            // if the host address is not the same as the one we started with,
            // which is possible as your organization changes C360 versions, then
            // update the rest client with the new base uri
            if (orgHost.Host != "http://localhost" && orgHost.Host.ToLowerInvariant() != baseUri.ToLowerInvariant())
            {
                _client = new RestClient(orgHost.Host);
            }

             // Convert casing
            SimpleJson.SimpleJson.CurrentJsonSerializerStrategy = new CamelCaseJsonSerializerStrategy();
        }
    }

    internal class CamelCaseJsonSerializerStrategy : SimpleJson.PocoJsonSerializerStrategy
    {
        protected override string MapClrMemberNameToJsonFieldName(string clrPropertyName)
        {
            if (clrPropertyName.Length >= 2)
            {
                return char.ToLower(clrPropertyName[0]).ToString() + clrPropertyName.Substring(1);
            }
            else
            {
                //Single char name e.g Property.X
                return clrPropertyName.ToLower();
            }
        }
    }
}
