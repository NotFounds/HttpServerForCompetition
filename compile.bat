@echo off
if "%~x1" == ".c" (
	PATH = C:\borland\bcc55\Bin
	if exist C:\borland\bcc55\Bin (
		bcc32 -n%~dp1 %1
	)
)

if "%~x1" == ".cpp" (
	Set PATH = C:\borland\bcc55\Bin
	if exist C:\borland\bcc55\Bin (
		bcc32 -n%~dp1 %1
	)
)

if "%~x1" == ".cs" (
	Set PATH = C:\Windows\Microsoft.NET\Framework\v4.0.30319
	csc /out:%~dp1%~n1.exe %1
)
