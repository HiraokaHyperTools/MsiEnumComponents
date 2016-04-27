# MsiEnumComponents

Usage:
```
MsiEnumComponents /all
MsiEnumComponents /find vcruntime140.dll
MsiEnumComponents /productof ComponentId
MsiEnumComponents /productof {83CCA637-410A-58E4-BD1E-8DE029108ADB}
MsiEnumComponents /productof {D282951C-BBCA-572D-83E5-CC72E934A4B2}
MsiEnumComponents /productof {E8E39D3B-4F35-36D8-B892-4B28336FE041}
MsiEnumComponents /productof {B33258FD-750C-3B42-8BE4-535B48E97DB4}
```

MsiEnumComponents /productof {E8E39D3B-4F35-36D8-B892-4B28336FE041}
```
{A2563E55-3BEC-3828-8D67-E5E8B9E8B675}  Microsoft Visual C++ 2015 x86 Minimum Runtime - 14.0.23026
```

MsiEnumComponents /productof {B33258FD-750C-3B42-8BE4-535B48E97DB4}
```
{0D3E9E15-DE7A-300B-96F1-B4AF12B96488}  Microsoft Visual C++ 2015 x64 Minimum Runtime - 14.0.23026
```

MsiEnumComponents.exe /find vcruntime140.dll
```
44208	{4E906323-911A-3D41-B920-BCE7B086F8BF}	Local	C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Remote Debugger\x64\vcruntime140.dll	{DE064F60-6522-3310-9665-B5E3E78B3638}	Microsoft Visual Studio Community 2015
77553	{83CCA637-410A-58E4-BD1E-8DE029108ADB}	Local	C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\redist\x86\Microsoft.VC140.CRT\vcruntime140.dll	{AD4CA91C-0F04-3E3E-9A7E-4A1A943BCBB8}	Visual C++ Library CRT X86 Redist Package
97623	{F33EF9A9-99C3-53B9-8E1A-E8759C3A7922}	Local	C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\redist\arm\Microsoft.VC140.CRT\vcruntime140.dll	{ABB55246-7198-37BA-BB9B-8F7844667BB8}	Visual C++ Library CRT ARM Redist Package
110624	{E8E39D3B-4F35-36D8-B892-4B28336FE041}	Local	C:\Windows\SysWOW64\vcruntime140.dll	{A2563E55-3BEC-3828-8D67-E5E8B9E8B675}	Microsoft Visual C++ 2015 x86 Minimum Runtime - 14.0.23026
111063	{AB434B4B-388B-30EC-8D42-C0BCCD9E34C0}	Local	C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Remote Debugger\x86\vcruntime140.dll	{DE064F60-6522-3310-9665-B5E3E78B3638}	Microsoft Visual Studio Community 2015
117658	{D282951C-BBCA-572D-83E5-CC72E934A4B2}	Local	C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\redist\x64\Microsoft.VC140.CRT\vcruntime140.dll	{E6C22A5A-8C4C-369B-881B-273396D2CB44}	Visual C++ Library CRT X64 Redist Package
133122	{B33258FD-750C-3B42-8BE4-535B48E97DB4}	Local	C:\Windows\system32\vcruntime140.dll	{0D3E9E15-DE7A-300B-96F1-B4AF12B96488}	Microsoft Visual C++ 2015 x64 Minimum Runtime - 14.0.23026
```

Output format of: `/all` or `/find`
- ComponentIndex TAB ComponentId TAB INSTALLSTATE TAB ComponentLocation TAB ProductId TAB ProductName

Output format of: `/productof`
- ProductId TAB ProductName
