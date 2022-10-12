﻿
function GetTargetFile(){
    Get-ChildItem "$PSScriptRoot\..\Directory.Build.props"
    #C:\ws\git\Nav.Language.Extensions\Directory.Build.props
}

function IncreaseMajor(){
    [CmdletBinding()]
    Param(

        [Parameter(Position=0, Mandatory=$true)]
        [string] $file
    )

    UpdateVersion $file { param($oldVersion) New-Object System.Version -ArgumentList ($oldVersion.Major+1), 0, 0 }
}

function IncreaseMinor(){
    [CmdletBinding()]
    Param(

        [Parameter(Position=0, Mandatory=$true)]
        [string] $file
    )

    UpdateVersion $file { param($oldVersion) New-Object System.Version -ArgumentList $oldVersion.Major, ($oldVersion.Minor+1), 0 }
}

function IncreaseBuild(){
    [CmdletBinding()]
    Param(

        [Parameter(Position=0, Mandatory=$true)]
        [string] $file
    )

    UpdateVersion $file { param($oldVersion) New-Object System.Version -ArgumentList $oldVersion.Major, $oldVersion.Minor, ($oldVersion.Build+1) }

}


function UpdateVersion(){
    [CmdletBinding(DefaultParametersetName='InvokeBuild')]
    Param(

        [Parameter(Position=0, Mandatory=$true)]
        [string] $file,
        [Parameter(Position=1, Mandatory=$true)]
        [ScriptBlock] $updateScript
    )

    $file=Convert-Path $file

    Write-Verbose "Opening file '$file'"

    $xml=[xml](cat $file)

    $productVersionNode=$xml.Project.PropertyGroup.ChildNodes | ? Name -eq ProductVersion

    $oldVersion=[Version]::Parse($productVersionNode.InnerText)
    Write-Verbose "Current version number is '$oldVersion'"

    $newVersion= & $updateScript $oldVersion
    Write-Verbose "New version number is '$newVersion'"

    $productVersionNode.InnerText=$newVersion.ToString()

    Write-Verbose "Saving file '$file'"
	$utf8WithBom = New-Object System.Text.UTF8Encoding($true)
	$sw = New-Object System.IO.StreamWriter($file, $false, $utf8WithBom)
    $xml.Save($sw)
	$sw.Close()
}