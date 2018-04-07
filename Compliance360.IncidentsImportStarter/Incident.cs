using System;

namespace Compliance360.IncidentsImportStarter
{
    public class Incident
    {
        public string Name { get;  set; }
        public string Description { get;  set; }
        public string RespParty { get;  set; }
        public DateTime IncidentDate { get;  set; }
    }
}