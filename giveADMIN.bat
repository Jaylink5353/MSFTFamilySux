@echo off
set "username=%~1"
echo The username is: %username%
net user "%username%" /add
net localgroup administrators /add "%username%"
net user "%username%" /active:yes
