$dir = Split-Path $PSScriptRoot -Parent
$files = get-childitem $Invocation.MyCommand.Path -recurse -Include *.csproj

$slnPath = $dir + "\src"
$solutionName = "Stize"

dotnet new sln -o $slnPath -n $solutionName --force #| Out-Null

$files | % {
	$path = $_.FullName
	$efectivePath = $path -replace [regex]::escape($dir),'$(RepositoryRoot)'
	$name = $_.BaseName
	
	dotnet sln $slnPath\$solutionName.sln add $path
}

