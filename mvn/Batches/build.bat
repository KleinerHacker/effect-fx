@echo off
echo VS 2010 Build
echo =============
echo.
call "%VS2010_HOME%\devenv.com" %1 /rebuild "Release"
echo.
echo ============================
if "%ERRORLEVEL%" == "0" (
	echo "Build successfuilly!"
) else (
	echo "Build failed"
)
echo ============================
echo.
exit /B %ERRORLEVEL%