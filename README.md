# dependency-monitor
Given a CSPROJ file return all dependencies used by the project 


Usage:

dependency-monitor.dll 
  arg[0] = Dependency name
  arg[1] = CSPROJ file path
  
e.g.

```
dependency-monitor.dll Microsoft.AspNetCore.Mvc.NewtonsoftJson /Users/yourusername/Desktop/YourCSharpProjectFile.csproj
```

Dependency="Name" Version="2.6.0"

Dependency="Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="5.0.1"

Dependency="Newtonsoft.Json" Version="12.0.3" 

Note: target dependency will be highlighted in RED if found, otherwise Green
