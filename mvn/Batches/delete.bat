@echo off
echo Cleanup
echo =======
echo.
rmdir %1 /S /Q
echo.
echo OK

echo.

echo VS 2010 Cleanup
echo ===============
echo.
call "%VS2010_HOME%\devenv.com" %2 /clean
echo.
echo ============================
if "%ERRORLEVEL%" == "0" (
	echo "Cleanup successfuilly!"
) else (
	echo "Cleanup failed"
)
echo ============================
echo.
exit /B %ERRORLEVEL%