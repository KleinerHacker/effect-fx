; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{0B1C694B-0864-4171-9A9D-A2CB49AAC136}
AppName=${project.name}
AppVersion=${pom.version}
;AppVerName=Effect FX 1.0
AppPublisher=PC-Software
DefaultDirName={pf}\PC-Software\${project.name}
DefaultGroupName=${project.name}
OutputDir=${project.out}\setup
OutputBaseFilename=${setup.name}
Compression=lzma
SolidCompression=yes
UninstallFilesDir={app}\Uninstall
;DiskSpanning=yes
;DiskSliceSize=104857600

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"

[Types]
Name: "full"; Description: "{cm:full}";
Name: "minimal"; Description: "{cm:minimal}";

[Components]
Name: "main"; Description: "{cm:main}"; Flags: fixed; Types: full minimal;
Name: "plugins"; Description: "{cm:plugins}"; Types: full;

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "${project.out}\bin\*"; DestDir: "{app}"; Flags: ignoreversion; Components: main; 
Source: "${project.out}\bin\Plugins\*"; DestDir: "{app}\Plugins"; Flags: ignoreversion; Components: plugins;
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\${project.name}"; Filename: "{app}\${program.file}"
Name: "{group}\{cm:UninstallProgram,${project.name}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\${project.name}"; Filename: "{app}\${program.file}"; Tasks: desktopicon

[Run]
Filename: "{app}\${program.file}"; Description: "{cm:LaunchProgram,${project.name}}"; Flags: nowait postinstall skipifsilent

[CustomMessages]
english.full=Full
english.minimal=Minimal (without plugins)
english.main=Main Program Files
english.plugins=Plugin Files (Effects)
german.full=Voll
german.minimal=Minimal (ohne Plugins)
german.main=Benötigte Programmdateien
german.plugins=Plugin Dateien (Effekte)
