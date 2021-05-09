# dependency-monitor
dependency-monitor is an application designed to detect GitHub repositories using a specific dependency. Both public and private repositories are supported.

Using GitHub APIs, dependency-monitor authenticates and downloads the repository to your hard drive. It then looks for projects utilising the dependency by querying .csproj files.

Let's assume you need to know how many projects across your organization are using Newtonsoft.Json.
You could either clone all the repositories in your organization, open them one by one and look under "Packages" and make a list.

![depedency image](https://i.ibb.co/rHVRkjL/dependency.png)

or use  depedency-monitor and just type this instead:
```
dependency-monitor.dll -batchscan myOrg Newtonsoft.Json
```
Dependency-monitor deletes all files when the analysis is complete.
### Requirements
To run the application requires a valid GitHub personal access token.

To generate one, click on your profile image > settings > developers settings > personal access tokens > generate new token.
Grant "repo" rights to the token. No other permission required.

### How to scan one repository:
If you want to scan just one single repository, start dependency-monitor as follows:

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

## Automation (Batch scan)
Use batch scan if you need to scan two or more repos.
Add the repository name to `repositories.txt` file. One line, one repository.

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
Note: there is a delay of two seconds between scans. if you want to remove this feature, remove the following line
```csharp
System.Threading.Thread.Sleep(new TimeSpan(0, 0, 2));
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