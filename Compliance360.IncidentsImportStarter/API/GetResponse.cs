using System;
using System.Collections.Generic;

namespace Compliance360.IncidentsImportStarter.API
{
    public class GetResponse<T>
    {
        public ApiRequestStatus Status { get; set; }
        public List<T> Data { get; set; }
    }
}
