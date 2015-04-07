@echo off
echo Build Setup (Inno Setup)
echo ========================
echo.
call "%IS_HOME%\iscc.exe" %1
echo.
echo ================================
if "%ERRORLEVEL%" == "0" (
	echo "Successfully!"
) else (
	echo "Failed!"
)
echo ================================

exit /B %ERRORLEVEL%