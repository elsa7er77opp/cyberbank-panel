#define AppName "CyberBank Panel"
#define AppVersion "0.1.0 Alpha"
#define AppPublisher "CyberBank Labs"
#define AppExeName "CyberBankPanel.Desktop.exe"
#define ProductFolder "CyberBank\CyberBank Panel"
#define BuildRoot "..\artifacts\CyberBankPanel-win-x64"
#define BrandIcon "..\src\CyberBankPanel.Desktop\Assets\Brand\cyberbank-panel-app-icon.ico"

[Setup]
AppId={{A8EE5E1C-790F-41A1-86DF-96DD9361D281}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher={#AppPublisher}
DefaultDirName={localappdata}\Programs\{#ProductFolder}
DefaultGroupName={#AppName}
DisableProgramGroupPage=yes
OutputDir=..\artifacts\installer
OutputBaseFilename=CyberBank-Panel-Alpha-Setup-win-x64
SetupIconFile={#BrandIcon}
UninstallDisplayIcon={app}\{#AppExeName}
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=lowest
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
CloseApplications=yes
RestartApplications=no

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional shortcuts:"; Flags: unchecked

[Files]
Source: "{#BuildRoot}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\{#AppName}"; Filename: "{app}\{#AppExeName}"; IconFilename: "{app}\{#AppExeName}"
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExeName}"; IconFilename: "{app}\{#AppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#AppExeName}"; Description: "Launch {#AppName}"; Flags: nowait postinstall skipifsilent
