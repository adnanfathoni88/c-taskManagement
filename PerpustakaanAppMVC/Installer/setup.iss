#define AppName "TWEEKLE"
#define AppExe "TWEEKLE.exe"
#define AppVer "1.0.0"

[Setup]
AppId={{D91E2E5B-1234-4A98-ABCD-999999999999}
AppName={#AppName}
AppVersion={#AppVer}
AppVerName={#AppName} {#AppVer}
DefaultDirName={pf}\{#AppName}
DefaultGroupName={#AppName}
OutputDir=Output
OutputBaseFilename=Setup_{#AppName}
PrivilegesRequired=admin

[Files]
Source: "..\bin\Release\TWEEKLE.exe"; DestDir: "{app}"
Source: "..\bin\Release\TWEEKLE.exe.config"; DestDir: "{app}"
Source: "..\bin\Release\System.Data.SQLite.dll"; DestDir: "{app}"
Source: "..\bin\Release\x86\SQLite.Interop.dll"; DestDir: "{app}\x86"

[Icons]
Name: "{group}\{#AppName}"; Filename: "{app}\{#AppExe}"
Name: "{commondesktop}\{#AppName}"; Filename: "{app}\{#AppExe}"
