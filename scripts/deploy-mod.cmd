@echo off
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0deploy-mod.ps1" %*
