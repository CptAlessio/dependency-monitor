# dependency-monitor
Discover Github repositories using a particular dependency. It supports both public and private repositories and can scan multiple repositories.

Authentication is handled by the GitHub APIs using a personal access token. The repository is cloned locally in .zip format. Dependencies are discovered by reading the content of every .csproj file in the solution.
Data is purged after each scan to save space.

Let's assume you want to know how many projects across your GitHub organization are using Newtonsoft.Json.
You could either clone all the repositories in your organization, open them one by one and look under "Packages" and make a list.

![depedency image](https://i.ibb.co/rHVRkjL/dependency.png)

or use  depedency-monitor and just type this instead:
```
dependency-monitor.[dll|exe] -batchscan myOrg Newtonsoft.Json
```
### Configuration and requirements
To run the application requires a valid GitHub personal access token. To generate one, click on your profile icon > settings > developers settings > personal access tokens > generate new token.
Grant repo rights to the token. No other permission required.

Update `Token` with your GitHub personal access token. You can find it in `Program.cs` > `Main` > `Token`

```csharp
/// <summary>
/// GitHub API Personal access token
/// </summary>
private const string TOKEN = "<YOUR-TOKEN-HERE>";
```
Update `OutputZipAnalysisFolder`
with a valid local path. This where the application stores source-code files and the cloned repository. Deleted after each scan.
```csharp
/// <summary>
/// Analysis folder used to download/unzip and search Dependencies
/// </summary>
private const string OutputZipAnalysisFolder = @"<YOUR-LOCAL-PATH-HERE>";
```

## Scan one repository
To scan one single repository, start dependency-monitor with the following arguments:

```
dependency-monitor.[dll|exe] 
      args[0] = GitHub Organization/User
      args[1] = Repository Name
      args[2] = Vulnerable Dependency
```
### Example
```
dependency-monitor.[dll|exe] "myOrg" "myRepo" "Microsoft.NET.Test.Sdk" 
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
## Scan two or more repositories
Use batch scan to scan two or more repositories at the same time.
- Add the repository name to `repositories.txt` file. 

### Example
```
PublicRepository1
PublicRepository2
PublicRepository3
```

- Start dependency-monitor using `-batchscan`:

```
dependency-monitor.[dll|exe] 
      args[0] = -batchscan
      args[1] = Organization
      args[2] = Vulnerable Dependency
```
### Example
```
dependency-monitor.[dll|exe] "-batchscan" "myOrg" "Newtonsoft.Json"
```
### Output
```
-----------------------------------------------------
Found 1 C# project(s) in repository PublicRepository1
-----------------------------------------------------
Dependencies in repository:
- Dependency name = "Newtonsoft.Json" Version="13.0.1"
[WARNING] Vulnerable dependency found 1 time(s)

-----------------------------------------------------
Found 1 C# project(s) in repository PublicRepository2
-----------------------------------------------------
Dependencies in repository:

[OK] No dependencies in project.
Multi-repository scan mode complete..
```
Note: there is a delay of two seconds between scans. To remove this feature, remove the following line from `Program.cs` method `BatchScanRepositories`
```csharp
System.Threading.Thread.Sleep(new TimeSpan(0, 0, 2));
```