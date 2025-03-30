REM original version https://risk-of-thunder.github.io/R2Wiki/Mod-Creation/C%23-Programming/Networking/UNet/
REM open this in vs it'll be so much nicer

REM call with ./weave.bat $(TargetDir)
set Target=JohnnyMod
set Output=%1
REM set Libs=Weaver\libs
set Zip=..\Build\Release.zip
set Store=..\Build
set Log=%Output%OUTPUT.log

REM copy unpatched dll to weaver folder in case its needed
REM robocopy    %Output%   .\Weaver     %Target%.dll     %Target%.pdb    /log:%Log%
REM ren .\Weaver\%Target%.dll   %Target%.dll.prepatch
REM ren .\Weaver\%Target%.pdb   %Target%.pdb.prepatch

REM le epic networking patch
REM .\Weaver\Unity.UNetWeaver.exe   %Libs%\UnityEngine.CoreModule.dll   %Libs%\com.unity.multiplayer-hlapi.Runtime.dll  %Output%    %Output%%Target%.dll   %Libs%

REM move prepatch back to output
REM robocopy    .\Weaver    %Output%    %Target%.dll.prepatch    %Target%.pdb.prepatch   /log:%Log%
REM del Weaver\%Target%.dll.prepatch
REM del Weaver\%Target%.pdb.prepatch

REM that's it. This is meant to pretend we just built a dll like any other time except this one is networked
REM add your postbuilds in vs like it's any other project

REM rename .bnk to .sound
if exist Assets\JohnnyBank.bnk ren Assets\JohnnyBank.bnk JohnnyBank.sound

robocopy    %Output%   %Store%      JohnnyMod.dll JohnnyMod.pdb     /log+:%Log%
robocopy    Assets     %Store%      johnnyassets  JohnnyBank.sound  /log+:%Log%

REM delete old zip
if exist %Zip% Del %Zip%

powershell Compress-Archive -Path '%Store%\*' -DestinationPath '%Zip%' -Force



REM OLD STUFF IDK
REM follow the Building Your Mod page on the Johnnytutorial wiki for more information on this
REM change this to your username (or add yours if you're working in a team or somethin)
REM if "$(Username)" == "Erikbir" set build=true

REM if defined build (

REM copy the built mod to our Build folder
REM copy "$(TargetPath)" "$(ProjectDir)..\Build\plugins"

REM copy the assetbundle from our unity project to our Build folder
REM change these paths to your (now hopefully renamed) folders
REM if exist "$(ProjectDir)..\JohnnyUnityProject\AssetBundles\myassetbundle" (
REM copy "$(ProjectDir)..\JohnnyUnityProject\AssetBundles\myassetbundle" "$(ProjectDir)..\Build\plugins\AssetBundles"
REM )

REM copy the whole Build\plugins folder into your r2modman profile. This mimics how r2modman will install your mod
REM Xcopy /E /I /Y "$(ProjectDir)..\Build\plugins" "E:\r2Profiles\Blinx Returns\BepInEx\plugins\rob-Johnnymod\"
REM )