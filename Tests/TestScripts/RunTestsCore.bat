@echo off
cd "C:\\Users\\david\\OneDrive\\Documents\\GitHub\\chess\\Tests"
dotnet test --filter "Category=CORE" > C:\Temp\core_tests_output.txt 2>&1
pause