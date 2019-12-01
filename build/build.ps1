param(
    # Build configuration
    [ValidateSet("Release", "Debug")]
    [string]$Configuration = "Release",
    
    # Target platform
    [ValidateSet("x86", "x64")]
    [string]$Platform = "x86",

    # Directory for the output
    [string]$Output = $PSScriptRoot,
	
	# Temp directory
    [string]$TempDir = (Join-Path $env:TEMP "PlayniteDeSmuMEBuild")
)

##########################
# RESTORE NUGET PACKAGES #
##########################

mkdir -p $TempDir | out-null
Invoke-WebRequest -Uri "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -OutFile $TempDir\nuget.exe
& "$TempDir\nuget.exe" restore "$PSScriptRoot\..\source\"
Remove-Item -LiteralPath $TempDir -Force -Recurse | out-null

###########
# COMPILE #
###########

cd $PSScriptRoot\..\source\
$args = '/p:Configuration="{0}"' -f $Configuration
$path = & 'C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe' -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe | select-object -first 1
if ($path) {
	& $path $args
}

############################
# PREPARE OUTPUT DIRECTORY #
############################

cd $Output
Remove-Item -LiteralPath $Configuration -Force -Recurse -ErrorAction:Ignore | out-null
mkdir -p $Configuration/Resources | out-null
cd .\$Configuration

############################
# COPY TO OUTPUT DIRECTORY #
############################

#File: desmumeicon.png
Invoke-WebRequest -Uri "https://upload.wikimedia.org/wikipedia/commons/2/2c/Desmume.png" -OutFile .\Resources\desmumeicon.png

#File: extension.yaml
Copy-Item "$PSScriptRoot\..\media\extension.yaml"

#File: FullscreenDeSmuME.dll
Copy-Item "$PSScriptRoot\..\source\FullscreenDeSmuME\bin\Release\FullscreenDeSmuME.dll"

#######
# END #
#######

cmd /c pause
cd $PSScriptRoot