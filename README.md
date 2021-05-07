# dependency-monitor
Given a CSPROJ file return all dependencies used by the project 


Usage:

```
dependency-monitor.dll 
      arg[0] = Dependency name
      arg[1] = CSPROJ file path
```
e.g.

```
dependency-monitor.dll Microsoft.AspNetCore.Mvc.NewtonsoftJson /Users/yourusername/Desktop/YourCSharpProjectFile.csproj
```

Output:

```
-------------------------------------
Found 2 C# Project files in archive
-------------------------------------

    Dependency="Microsoft.NET.Test.Sdk" Version="16.9.1" 
    Dependency="xunit.runner.visualstudio" Version="2.4.3" 
    Dependency="Google.Protobuf" Version="3.15.6" 
 - [ALERT] Target reference found 1 times

    Dependency="Figgle" Version="0.4.0" 
    Dependency="FluentAssertions" Version="5.10.3" 
    Dependency="Microsoft.AspNetCore.TestHost" Version="3.1.13" 
    Dependency="WireMock.Net" Version="1.4.8" 
 - [SUCCESS] Target reference not found

```

Note: target dependency will be highlighted in RED if found, otherwise Green
