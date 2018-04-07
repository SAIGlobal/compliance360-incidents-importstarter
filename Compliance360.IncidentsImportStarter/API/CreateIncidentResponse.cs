
using System;

namespace Compliance360.IncidentsImportStarter.API
{
    public class CreateIncidentResponse
    {
        public ApiRequestStatus Status { get; set; }
        public EntityId Id { get; set; }
    }
}
