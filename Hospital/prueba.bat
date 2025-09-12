@echo off
echo Limpiando caché de Visual Studio...
rmdir /s /q .vs
rmdir /s /q bin
rmdir /s /q obj
echo Listo! Ahora abre Visual Studio y recompila.
pause
