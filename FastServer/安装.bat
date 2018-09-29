@echo off
cd /d %~dp0
c:\windows\Microsoft.Net\Framework\v4.0.30319\installutil.exe FastEtlService.exe
net start FastEtlService
net start FastEtlService
net start FastEtlService
pause