# Doom-Ma-Geddon
This is a malware project inspired by the "Doom-Ma-Geddon Virus" from the 2010's animated sitcom [Regular Show](https://en.wikipedia.org/wiki/Regular_Show).

## :wrench: Build
Install the latest DotNet before running the following:
```powershell
dotnet publish -r win-x64 -c Release --self-contained true -p:PublishSingleFile=true
# Portable Executable (exe) Located At:
cd .\bin\Release\net8.0-windows\win-x64\publish
```