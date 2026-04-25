#!/usr/bin/env bash
# Verifica la compilación localmente usando dotnet SDK.
# Compila los 3 proyectos wrapper (net8.0-windows con EnableWindowsTargeting) en macOS/Linux.
# Requiere: dotnet SDK instalado (brew install dotnet)
# Uso: ./build-local.sh [--no-restore]

set -e
cd "$(dirname "$0")"

NO_RESTORE=""
if [[ "$1" == "--no-restore" ]]; then
  NO_RESTORE="--no-restore"
fi

echo "▶ build-local.sh — verificación de compilación C#"
echo ""

if [[ -z "$NO_RESTORE" ]]; then
  echo "→ Restoring packages..."
  dotnet restore TgcViewerCheck.csproj -v q
  dotnet restore AlumnoEjemplosCheck.csproj -v q
  dotnet restore HorrorGameCheck.csproj -v q
fi

echo "→ Building TgcViewer..."
dotnet build TgcViewerCheck.csproj -c Debug --nologo $NO_RESTORE 2>&1 | grep -E "error CS|Error\(s\)" || true

echo "→ Building AlumnoEjemplos (game code)..."
dotnet build AlumnoEjemplosCheck.csproj -c Debug --nologo $NO_RESTORE 2>&1 | grep -E "error CS|Error\(s\)" || true

echo "→ Building HorrorGame..."
dotnet build HorrorGameCheck.csproj -c Debug --nologo $NO_RESTORE 2>&1 | grep -E "error CS|Error\(s\)" || true

# Fail if any project has errors
ERRORS=0
dotnet build TgcViewerCheck.csproj -c Debug --nologo $NO_RESTORE 2>&1 | grep -q "[1-9][0-9]* Error(s)" && ERRORS=1
dotnet build AlumnoEjemplosCheck.csproj -c Debug --nologo $NO_RESTORE 2>&1 | grep -q "[1-9][0-9]* Error(s)" && ERRORS=1
dotnet build HorrorGameCheck.csproj -c Debug --nologo $NO_RESTORE 2>&1 | grep -q "[1-9][0-9]* Error(s)" && ERRORS=1

if [ $ERRORS -eq 0 ]; then
  echo ""
  echo "✓ Build exitoso — 0 errores de compilación."
else
  echo ""
  echo "✗ Build fallido — hay errores de compilación."
  exit 1
fi
