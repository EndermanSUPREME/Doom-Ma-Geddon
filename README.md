# Doom-Ma-Geddon
This is a malware project inspired by the "Doom-Ma-Geddon Virus" from the 2010's animated sitcom [Regular Show](https://en.wikipedia.org/wiki/Regular_Show).

## Breakdown
This project is a small malware project targeted against basic Windows machines, the executable when ran
pretends to be a critical error prompting the user for immedient action, when the user chooses to take
action, the malware will execute as `Administrator`, via UAC, an obfuscated encoded-powershell command that modifies
the ExecutionPolicy on the device enabling the later performed execution of remote powershell files from the Internet.
*encoded command not included*

## ⚠️Responsible Usage Notice
This project is intended for educational and authorized security testing purposes only.
Unauthorized use of this program against systems you do not own or have explicit permission
to test is strictly prohibited and may be illegal. Always obtain proper authorization before
running any form of penetration testing or exploitation activity. The creator assumes no
liability for misuse or damage caused by improper or unlawful use of this software.

## :wrench: Build
Install the latest DotNet before running the following:
```powershell
dotnet publish -r win-x64 -c Release --self-contained true -p:PublishSingleFile=true
# Portable Executable (exe) Located At:
cd .\bin\Release\net8.0-windows\win-x64\publish
```