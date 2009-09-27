@echo off

call "C:\Program Files\Microsoft Visual Studio 9.0\Common7\Tools\vsvars32.bat"
:svcutil http://localhost:8081 /async /tcv:Version35
svcutil http://localhost:8081
