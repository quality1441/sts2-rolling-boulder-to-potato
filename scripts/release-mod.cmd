@echo off
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0release-mod.ps1" %*
