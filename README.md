# dependency-monitor
dependency-monitor-cli is a .net core application that can be used to scan Github repositories for any given dependency.

Support public and private repositories and leverages GitHub APIs to connect and download repositories.

In order to work you must provide a valid GitHub personal access token. 
You can generate one by clicking on your profile image > settings > developers settings > personal access tokens > generate new token.

Important : Token must have "repo" rights and nothing else.

Programming languages supported : C# 

### How to use:

```
dependency-monitor.dll 
      arg[0] = GitHub Organization/User
      arg[1] = Repository Name
      arg[2] = Vulnerable Dependency
```
### Example

```
dependency-monitor.dll "YourOrganization" "YourRepository" "Microsoft.NET.Test.Sdk" 
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

Vulnerable dependencies will be highlighted in red if found, otherwise green
