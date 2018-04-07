using System;

namespace Compliance360.IncidentsImportStarter.API
{
    public class LoginResponse
    {
        public ApiRequestStatus Status { get; set; }
        public string Token { get; set; }
    }
}