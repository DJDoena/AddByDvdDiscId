[Setup]
AppName=Add by DVD Disc ID
AppId=AddByDvdDiscId
AppVerName=Add by DVD Disc ID 1.0.0.3
AppCopyright=Copyright © Doena Soft. 2021 - 2022
AppPublisher=Doena Soft.
AppPublisherURL=http://doena-journal.net/en/dvd-profiler-tools/
DefaultDirName={commonpf32}\Doena Soft.\Add by DVD Disc ID
DefaultGroupName=Add by DVD Disc ID
DirExistsWarning=No
SourceDir=..\AddByDvdDiscId\bin\x86\AddByDvdDiscId
Compression=zip/9
AppMutex=InvelosDVDPro
OutputBaseFilename=AddByDvdDiscIdSetup
OutputDir=..\..\..\..\AddByDvdDiscIdSetup\Setup\AddByDvdDiscId
MinVersion=0,6.0
PrivilegesRequired=admin
WizardStyle=modern
DisableReadyPage=yes
ShowLanguageDialog=no
VersionInfoCompany=Doena Soft.
VersionInfoCopyright=2021 - 2022
VersionInfoDescription=Add by DVD Disc ID Setup
VersionInfoVersion=1.0.0.3
UninstallDisplayIcon={app}\djdsoft.ico

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Messages]
WinVersionTooLowError=This program requires Windows XP or above to be installed.%n%nWindows 9x, NT and 2000 are not supported.

[Types]
Name: "full"; Description: "Full installation"

[Files]
Source: "djdsoft.ico"; DestDir: "{app}"; Flags: ignoreversion

Source: "AddByDvdDiscId.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "AddByDvdDiscId.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "DoenaSoft.CalculateDvdDiscId.dll"; DestDir: "{app}"; Flags: ignoreversion

Source: "DoenaSoft.AbstractionLayer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "DoenaSoft.DVDProfilerHelper.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "DoenaSoft.DVDProfilerXML.dll"; DestDir: "{app}"; Flags: ignoreversion

Source: "de\AddByDvdDiscId.resources.dll"; DestDir: "{app}\de"; Flags: ignoreversion
Source: "de\DoenaSoft.DVDProfilerHelper.resources.dll"; DestDir: "{app}\de"; Flags: ignoreversion

; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Run]
Filename: "{win}\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe"; Parameters: "/codebase ""{app}\AddByDvdDiscId.dll"""; Flags: runhidden

;[UninstallDelete]

[UninstallRun]
Filename: "{win}\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe"; Parameters: "/u ""{app}\AddByDvdDiscId.dll"""; Flags: runhidden

[Registry]
; Register - Cleanup ahead of time in case the user didn't uninstall the previous version.
Root: HKCR; Subkey: "CLSID\{{E4A6A6E4-391E-4243-BF65-F42A933B7096}"; Flags: dontcreatekey deletekey
Root: HKCR; Subkey: "DoenaSoft.DVDProfiler.AddByDvdDiscId.Plugin"; Flags: dontcreatekey deletekey
Root: HKCU; Subkey: "Software\Invelos Software\DVD Profiler\Plugins\Identified"; ValueType: none; ValueName: "{{E4A6A6E4-391E-4243-BF65-F42A933B7096}"; ValueData: "0"; Flags: deletevalue
Root: HKCU; Subkey: "Software\Invelos Software\DVD Profiler_beta\Plugins\Identified"; ValueType: none; ValueName: "{{E4A6A6E4-391E-4243-BF65-F42A933B7096}"; ValueData: "0"; Flags: deletevalue
Root: HKLM; Subkey: "Software\Classes\CLSID\{{E4A6A6E4-391E-4243-BF65-F42A933B7096}"; Flags: dontcreatekey deletekey
Root: HKLM; Subkey: "Software\Classes\DoenaSoft.DVDProfiler.AddByDvdDiscId.Plugin"; Flags: dontcreatekey deletekey
; Unregister
Root: HKCR; Subkey: "CLSID\{{E4A6A6E4-391E-4243-BF65-F42A933B7096}"; Flags: dontcreatekey uninsdeletekey
Root: HKCR; Subkey: "DoenaSoft.DVDProfiler.AddByDvdDiscId.Plugin"; Flags: dontcreatekey uninsdeletekey
Root: HKCU; Subkey: "Software\Invelos Software\DVD Profiler\Plugins\Identified"; ValueType: none; ValueName: "{{E4A6A6E4-391E-4243-BF65-F42A933B7096}"; ValueData: "0"; Flags: uninsdeletevalue
Root: HKCU; Subkey: "Software\Invelos Software\DVD Profiler_beta\Plugins\Identified"; ValueType: none; ValueName: "{{E4A6A6E4-391E-4243-BF65-F42A933B7096}"; ValueData: "0"; Flags: uninsdeletevalue
Root: HKLM; Subkey: "Software\Classes\CLSID\{{E4A6A6E4-391E-4243-BF65-F42A933B7096}"; Flags: dontcreatekey uninsdeletekey
Root: HKLM; Subkey: "Software\Classes\DoenaSoft.DVDProfiler.AddByDvdDiscId.Plugin"; Flags: dontcreatekey uninsdeletekey

[Code]
function IsDotNET4Detected(): boolean;
// Function to detect dotNet framework version 4
// Returns true if it is available, false it's not.
var
dotNetStatus: boolean;
begin
dotNetStatus := RegKeyExists(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4');
Result := dotNetStatus;
end;

function InitializeSetup(): Boolean;
// Called at the beginning of the setup package.
begin

if not IsDotNET4Detected then
begin
MsgBox( 'The Microsoft .NET Framework version 4 is not installed. Please install it and try again.', mbInformation, MB_OK );
Result := false;
end
else
Result := true;
end;