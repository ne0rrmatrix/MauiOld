@echo off
cd samples
dotnet build -c:Debug
cd ..
cd src\CommunityToolkit.Maui.UI.UnitTests
dotnet build .\UITests.Android\UITests.Android.csproj
dotnet test .\UITests.Android\UITests.Android.csproj
dotnet build .\UITests.Windows\UITests.Windows.csproj
dotnet test .\UITests.Windows\UITests.Windows.csproj