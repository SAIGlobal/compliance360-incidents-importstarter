using System;

namespace Compliance360.IncidentsImportStarter.API
{
    public class LoginRequest
    {
        public string Organization { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Culture { get; set; }
    }
}
