@echo off
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0build-mod.ps1" %*
