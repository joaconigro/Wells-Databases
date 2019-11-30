; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Wells Manager"
#define MyAppVersion "1.0.0.0"
#define MyAppPublisher "Joaco's Software"
#define MyAppPublisherFolder "Joaco's Software"
#define MyAppExeName "Wells.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{ACDE65C6-28BB-4325-81DE-03B1E8177EBA}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
VersionInfoVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={commonpf}\{#MyAppPublisherFolder}\{#MyAppName}
DisableProgramGroupPage=yes
OutputDir=D:\Trabajo\Observatorio
OutputBaseFilename=Wells_Installer_{#MyAppVersion}
SetupIconFile=D:\Trabajo\Observatorio\Wells\Others\icon128x128.ico
UninstallDisplayIcon="{app}\{#MyAppExeName}"
Compression=lzma
SolidCompression=yes
ChangesAssociations=yes
ArchitecturesInstallIn64BitMode=x64
ArchitecturesAllowed=x64

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "D:\Trabajo\Observatorio\Wells\Wells\bin\Release\netcoreapp3.0\win-x64\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
Source: "D:\Trabajo\Observatorio\Wells\Others\AppSettings.was"; DestDir: "{commonappdata}\WellManager"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "D:\Trabajo\Observatorio\Wells\Others\Gradients.wgr"; DestDir: "{commonappdata}\WellManager"; Flags: onlyifdoesntexist skipifsourcedoesntexist
Source: "D:\Trabajo\Observatorio\Wells\Others\Graphics.wpg"; DestDir: "{commonappdata}\WellManager"; Flags: onlyifdoesntexist skipifsourcedoesntexist
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{commonprograms}\{#MyAppPublisherFolder}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange("Wells Manager", '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[CustomMessages]
OnlyOnTheseArchitectures=Wells Manager solo se puede ejecutar en sistemas operativos de 64 bits.

