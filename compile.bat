if "%~x1" == ".c" (
	if exist C:\borland\bcc55\Bin (
		bcc32 -n%~dp1 %1
	)
)

if "%~x1" == ".cpp" (
	if exist C:\borland\bcc55\Bin (
		bcc32 -n%~dp1 %1
	)
)

if "%~x1" == ".cs" (
	csc /out:%~dp1%~n1.exe %1
)
