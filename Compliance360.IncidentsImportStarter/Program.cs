using System;
using System.Collections.Generic;
using Compliance360.IncidentsImportStarter.API;

namespace Compliance360.IncidentsImportStarter
{
    
    /// <summary>
    /// This is a sample application that will read a set of sample Incidents
    /// from a file, in practice this could be from a REST endpoint of SFTP, and insert them
    /// into the Compliance 360 application using the REST API.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point for the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        static void Main(string[] args)
        {
            var arguments = ParseArguments(args);

            var apiClient = new ApiClient();
            var importer = new IncidentImporter(apiClient);

            try 
            {
                importer.Import(arguments);
            }
            catch(Exception ex) 
            {
                Console.Out.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Parses the supplied arguments and populates the Arguments dictionary.
        /// </summary>
        /// <param name="args">The arguments to parse.</param>
        public static Dictionary<string, string> ParseArguments(string[] args)
        {
            string argKey = null;
            var arguments = new Dictionary<string, string>();

            foreach(var arg in args)
            {
                if (arg.StartsWith("-")) {
                    // this is an argument key like "-n" or "--filename"
                    argKey = arg;

                    // add the key to the dictionary
                    if (!arguments.ContainsKey(argKey)) {
                        arguments.Add(argKey, argKey);
                    }
                } else {
                    if (argKey != null) {
                        arguments[argKey] = arg;
                        argKey = null;
                    } else {
                        arguments[arg] = arg;
                    }
                }
            }

            return arguments;
        }
    }
}
