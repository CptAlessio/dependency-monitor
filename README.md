# dependency-monitor
Scan one or more Github (private and public) repositories for any given dependency.
Built using GitHub APIs and .NET core.
Only support C# projects.

### Requirements
Requires GitHub personal access token. 
To generate one click on your profile image > settings > developers settings > personal access tokens > generate new token.
Token must have "repo" rights and nothing else.

### How to scan one repository:
To scan one single repository start dependency-monitor as follows:

```
dependency-monitor.dll 
      args[0] = GitHub Organization/User
      args[1] = Repository Name
      args[2] = Vulnerable Dependency
```
### Example
```
dependency-monitor.dll myOrg myRepo Microsoft.NET.Test.Sdk 
```
### Output:
```
-------------------------------------
Found 2 C# Project files in archive
-------------------------------------

    Dependency="Microsoft.NET.Test.Sdk" Version="16.9.1" 
    Dependency="xunit.runner.visualstudio" Version="2.4.3" 
    Dependency="Google.Protobuf" Version="3.15.6" 
    
[WARNING] Vulnerable dependency found 1 time(s)

    Dependency="Figgle" Version="0.4.0" 
    Dependency="FluentAssertions" Version="5.10.3" 
    Dependency="Microsoft.AspNetCore.TestHost" Version="3.1.13" 
    Dependency="WireMock.Net" Version="1.4.8" 
    
[OK] Vulnerable dependency not found

```

## How scan multiple projects
Add repos names to repositories.txt

Start dependency-monitor using `-batchscan` mode as follows:

```
dependency-monitor.dll 
      args[0] = -batchscan
      args[1] = Organization
      args[2] = Vulnerable Dependency
```
### Example
```
dependency-monitor.dll -batchscan myOrg Newtonsoft.Json
```
### Output
```angular2html
----------------------------------------------------
Found 1 C# project(s) in repository CodingChallenges
----------------------------------------------------
Dependencies in repository:
- Dependency name = "Newtonsoft.Json" Version="13.0.1"
[WARNING] Vulnerable dependency found 1 time(s)

--------------------------------------------------------
Found 1 C# project(s) in repository ValheimExpertBuilder
--------------------------------------------------------
Dependencies in repository:

[OK] No dependencies in project.
Multi-repository scan mode complete..
```