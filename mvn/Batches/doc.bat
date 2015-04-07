@echo off
echo Build Doxygen Documentation
echo ===========================
echo.

mkdir %2
"%DOXY_HOME%\bin\doxygen" %1

echo.
echo ====================================
if "%ERRORLEVEL%" == "0" (
	echo Doxygen Build successfully
) else (
	echo Doxygen Build failed
)
echo ====================================

echo.
if NOT "%ERRORLEVEL%" == "0" (
	goto end
)

echo Build chm File
echo ==============
echo.

"%HELP_HOME%\hhc.exe" "%~2\html\index.hhp"

echo ====================================
if "%ERRORLEVEL%" == "1" (
	echo chm File Build successfully
) else (
	echo chm File Build failed
)
echo ====================================
if NOT "%ERRORLEVEL%" == "1" (
	goto end
)

mkdir "%~2\docs"
copy "%~2\html\index.chm" "%~2\docs\doc.chm"
exit /B 0

:end
exit /B %ERRORLEVEL%