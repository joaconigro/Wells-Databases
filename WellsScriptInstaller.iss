; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Wells"
#define MyAppVersion "1.0.1.0"
#define MyAppPublisher "Joaco's Software"
#define MyAppPublisherFolder "Joaco's Software"
#define MyAppExeName "Wells.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{78BFE797-02E2-486C-A500-3324A43D3859}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
VersionInfoVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={commonpf}\{#MyAppPublisherFolder}\{#MyAppName}
DisableProgramGroupPage=yes
OutputDir=D:\Trabajo\Observatorio\Instaladores
OutputBaseFilename={#MyAppName}_{#MyAppVersion}
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
Source: "D:\Trabajo\Observatorio\Wells\Others\Settings.was"; DestDir: "{commonappdata}\Wells"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "D:\Trabajo\Observatorio\Wells\Others\Gradients.wgr"; DestDir: "{commonappdata}\Wells"; Flags: onlyifdoesntexist skipifsourcedoesntexist
Source: "D:\Trabajo\Observatorio\Wells\Others\Graphics.wpg"; DestDir: "{commonappdata}\Wells"; Flags: onlyifdoesntexist skipifsourcedoesntexist
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{commonprograms}\{#MyAppPublisherFolder}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange("Wells Manager", '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[CustomMessages]
OnlyOnTheseArchitectures=Wells Manager solo se puede ejecutar en sistemas operativos de 64 bits.

