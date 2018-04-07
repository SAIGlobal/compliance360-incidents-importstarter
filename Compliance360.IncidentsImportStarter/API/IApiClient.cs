using System;

namespace Compliance360.IncidentsImportStarter.API
{
    public interface IApiClient
    {
        /// <summary>
        /// Authenticates the user with the Compliance360 API, returning an auth token
        /// that can be used to access protected resources.
        /// </summary>
        /// <param name="baseUri">The base web address of the Compliance 360 API.</param>
        /// <param name="organization">The name of the Compliance 360 organization.</param>
        /// <param name="userName">The name of the user.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>String auth token.</returns>
        string Authenticate(string baseUri, string organization, string userName, string password);

        /// <summary>
        /// Creates a new Incident record.
        /// </summary>
        /// <param name="token">The current auth token.</param>
        /// <param name="divisionId">The Id of the Division.</param>
        /// <param name="folderId">The Id of the Folder.</param>
        /// <param name="workflowTemplateId">The Id of the WorkflowTemplate.</param>
        /// <param name="respPartId">The Id of the Resp Party.</param>
        /// <param name="name">The name of the Incident.</param>
        /// <param name="description">The description of the Incident.</param>
        /// <param name="incidentDate">The date of the Incident.</param>
        /// <returns></returns>
        EntityId CreateIncident(
                string token,
                EntityId divisionId,
                EntityId folderId,
                EntityId workflowTemplateId,
                EntityId respPartId,
                string name,
                string description,
                DateTime incidentDate
            );

        /// <summary>
        /// Gets the default Incident workflow template
        /// </summary>
        /// <param name="token">The current auth token.</param>
        /// <returns>Id of the Incident Workflow Template.</returns>
        EntityId GetDefaultIncidentWorkflowTemplateId(string token);

        /// <summary>
        /// Gets a Division's Id based on the supplied division path.
        /// </summary>
        /// <param name="token">The current auth token.</param>
        /// <param name="divisionPath">The path of the division for which the Id should be returned</param>
        /// <returns>Id of the Division.</returns>
        EntityId GetDivisionId(string token, string divisionPath);

        /// <summary>
        /// Gets an Employee's Id based on the supplied first and last name.
        /// </summary>
        /// <param name="token">The current auth token.</param>
        /// <param name="firstName">Employee's first name.</param>
        /// <param name="lastName">Employee's last name.</param>
        /// <returns>Id of the Employee.</returns>
        EntityId GetEmployeeId(string token, string firstName, string lastName);

        /// <summary>
        /// Gets the FolderId based on the supplied name
        /// </summary>
        /// <param name="token">The current auth token.</param>
        /// <param name="divisionId">The id of the division that contains the folder</param>
        /// <param name="folderName">The name of the folder for which the Id should be returned</param>
        /// <returns>Id of the Folder.</returns>
        EntityId GetFolderId(string token, EntityId divisionId, string folderName);
    }
}