@echo off
REM Projeyi derle
dotnet build

REM Unit testleri çalıştır ve kod kapsamı raporunu oluştur
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=./TestResults/

REM Projeyi dizinine geç
cd ShopsRus

dotnet build --project "ShopsRus.csproj"

REM Projeyi ayağa kaldır
dotnet run
