using System;
using System.Collections.Generic;

namespace Compliance360.IncidentsImportStarter
{
  public interface IIncidentImporter 
  {

    /// <summary>
    /// Imports the Incidents data from the file specified in the arguments.
    /// </summary>
    /// <param name="importArgs">Import arguments</param>
    void Import(Dictionary<string, string> importArgs);
  }
}
