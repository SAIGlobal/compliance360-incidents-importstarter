# Compliance 360 Incidents Import Starter Application
* [Introduction](#introduction)
* [API Documentation](#api-documentation)
* [Project Setup](#project-setup)
* [Running the Application](#running-th-application)
* [Tools](#tools)
* [How To Contribute](#how-to-contribute)
* [Maintainers](#maintainers)

# Introduction
The Compliance 360 application is the leading Governance Risk and Compliance (GRC) platform. The Compliance 360 system is a highly-configurable application framework underpinned by a metadata-driven architecture. 

The Incidents Import Starter Application is a .NET Core 2.0 Console application that leverages the Compliance 360 Security, Metadata and Data APIs to import Incidents from a file.
This is a sample application designed to provide an idea of what it is like to work with the Compliance 360 REST API to manage Incident records. The concepts used in this sample
application are applicable to other application modules and components that are part of the Compliance 360 platform.

# API Documentation
Our APIs are documented in the [OpenAPI Specification](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.0.md) format and can be found here:
1. [Security API](https://github.com/SAIGlobal/compliance360-security-api)
1. [Metadata API](https://github.com/SAIGlobal/compliance360-metadata-api)
1. [Data API](https://github.com/SAIGlobal/compliance360-data-api)

You can find Incident API specific information [here](https://github.com/SAIGlobal/compliance360-incidents-api). 

# Project Setup
The application is written in C# and is a .NET Core 2.0 based application. To build and run this application: 
1. Ensure that you have the .Net Core Command-Line interface (CLI) tools installed. 
2. Clone the repository.
3. From the command line run the following:
```
-- from the Compliance360.IncidentsImportStarter directory.
dotnet restore
dotnet build
``` 

# Running the Application
To run the app from the command lime:
```
dotnet run ./bin/Debug/netcoreapp2.0/Compliance360.IncidentsImportStarter.dll [options]
```

The following lists the required command line options:
| Option | Description |
| ------ | ----------- |
| --filepath | The path to the *.CSV file to import |
| --baseuri | The base uri to the Compliance 360 application. Most likely this value should be https://secure.compliance360.com |
| --organization | The organization name. |
| --username | The username to use for authentication. |
| --password | The password to use for authentication. |

The command line options are in specified in the format: {option}[space]{value}. 
```
Example: 
--filepath incident-data.csv --baseuri https://secure.compliance360.com --organization testorg --username myuseraccount --password mypassword
```


# Tools
Here are links to other applications that leverage the Compliance 360 Metadata, Data, and Security APIs.
* [Active Directory User Sync](https://github.com/SAIGlobal/compliance360-activedirectory-sync)

# How To Contribute
Contribute by submitting a PR and a bug report in GitHub.

# Maintainers
* [Thomas Lee](https://github.com/thethomaslee) - [@thethomaslee](https://twitter.com/thethomaslee)


