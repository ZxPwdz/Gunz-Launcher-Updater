@echo off
setlocal enabledelayedexpansion

echo ============================================
echo   Gunz Launcher Updater - GitHub Upload
echo   Created by ZxPwd
echo ============================================
echo.

:: Check if git is installed
where git >nul 2>nul
if %errorlevel% neq 0 (
    echo ERROR: Git is not installed or not in PATH.
    echo Please install Git from https://git-scm.com/
    pause
    exit /b 1
)

:: Set repository URL
set REPO_URL=https://github.com/ZxPwdz/Gunz-Launcher-Updater.git

echo Repository: %REPO_URL%
echo.

:: Ask for GitHub token
echo Enter your GitHub Personal Access Token:
echo (Create one at: https://github.com/settings/tokens)
echo.
set /p TOKEN=Token:

if "%TOKEN%"=="" (
    echo ERROR: Token cannot be empty.
    pause
    exit /b 1
)

echo.
echo Preparing to upload...
echo.

:: Navigate to project directory
cd /d "%~dp0"

:: Initialize git if not already initialized
if not exist ".git" (
    echo Initializing git repository...
    git init
    git branch -M main
)

:: Configure git to use token
set AUTH_URL=https://%TOKEN%@github.com/ZxPwdz/Gunz-Launcher-Updater.git

:: Check if remote exists
git remote get-url origin >nul 2>nul
if %errorlevel% neq 0 (
    echo Adding remote origin...
    git remote add origin %AUTH_URL%
) else (
    echo Updating remote origin...
    git remote set-url origin %AUTH_URL%
)

:: Stage all files
echo.
echo Staging files...
git add .

:: Show status
echo.
echo Files to be committed:
git status --short
echo.

:: Ask for commit message
set /p COMMIT_MSG=Enter commit message (or press Enter for default):

if "%COMMIT_MSG%"=="" (
    set COMMIT_MSG=Update Gunz Launcher Updater
)

:: Commit
echo.
echo Committing changes...
git commit -m "%COMMIT_MSG%"

:: Push to GitHub
echo.
echo Pushing to GitHub...
git push -u origin main

if %errorlevel% equ 0 (
    echo.
    echo ============================================
    echo   SUCCESS! Repository updated.
    echo   https://github.com/ZxPwdz/Gunz-Launcher-Updater
    echo ============================================
) else (
    echo.
    echo ============================================
    echo   Push failed. Trying force push...
    echo ============================================
    git push -u origin main --force

    if %errorlevel% equ 0 (
        echo.
        echo SUCCESS! Repository force-updated.
    ) else (
        echo.
        echo ERROR: Failed to push to GitHub.
        echo Check your token permissions and try again.
    )
)

:: Clear token from remote URL for security
git remote set-url origin %REPO_URL%

echo.
pause
