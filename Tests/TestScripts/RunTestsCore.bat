@echo off
set PROJECT_DIR="C:\\Users\\david\\OneDrive\\Documents\\GitHub\\chess"
set TEST_DIR="%PROJECT_DIR%\\Tests"
set OUTPUT_FILE="C:\\Temp\\core_tests_output.txt"

cd %PROJECT_DIR%

:: Clean and build the project with the macro defined
dotnet clean
REM dotnet build --configuration Debug /p:DefineConstants="COMPILE_WITH_CHECK_SERVICE" /v:diag

REM echo %TEST_DIR%

cd %TEST_DIR%

:: Run the tests and output to file
dotnet test --filter "Category=CORE" /p:DefineConstants="COMPILE_WITH_CHECK_SERVICE" --logger:"console;verbosity=detailed"

REM Output redirection is commented out, but you can uncomment it to save the log if needed.
REM > %OUTPUT_FILE% 2>&1
REM pause