# Fails when the aggregated Cobertura coverage in the results directory is below the threshold
# or does not contain every required package.
# Usage: ./eng/Assert-CodeCoverage.ps1 -ResultsDirectory ./TestResults -Threshold 100

[CmdletBinding()]
param(
  [Parameter(Mandatory)]
  [string] $ResultsDirectory,

  [ValidateRange(0, 100)]
  [double] $Threshold = 100,

  [ValidateNotNullOrEmpty()]
  [string[]] $RequiredPackages = @(
    'Unio',
    'Unio.Types',
    'Unio.Extensions',
    'Unio.AspNetCore',
    'Unio.SourceGenerator'
  )
)

$ErrorActionPreference = 'Stop'

$reports = @(Get-ChildItem -Path $ResultsDirectory -Recurse -Filter '*.cobertura.xml' -ErrorAction SilentlyContinue)
if ($reports.Count -eq 0) {
  throw "Expected at least one Cobertura report in '$ResultsDirectory', found 0 reports."
}

$invariant = [cultureinfo]::InvariantCulture
$packageLines = @{}
$packageBranches = @{}

foreach ($report in $reports) {
  [xml] $coverage = Get-Content -Path $report.FullName -Raw
  foreach ($pkg in @($coverage.coverage.packages.package)) {
    if (-not $pkg) { continue }
    $pkgName = [string]$pkg.name
    if (-not $packageLines.ContainsKey($pkgName)) {
      $packageLines[$pkgName] = @{}
      $packageBranches[$pkgName] = @{}
    }

    $linesMap = $packageLines[$pkgName]
    $branchesMap = $packageBranches[$pkgName]

    foreach ($cls in @($pkg.classes.class)) {
      if (-not $cls) { continue }
      $clsName = [string]$cls.name
      foreach ($method in @($cls.methods.method)) {
        if (-not $method) { continue }
        $methodKey = "$clsName::$($method.name)$($method.signature)"
        foreach ($line in @($method.lines.line)) {
          if (-not $line) { continue }
          $key = "$methodKey`:$($line.number)"
          $hits = [long]::Parse([string]$line.hits, $invariant)
          if (-not $linesMap.ContainsKey($key) -or $hits -gt $linesMap[$key]) {
            $linesMap[$key] = $hits
          }

          if ([string]$line.branch -eq 'True' -and [string]$line.'condition-coverage' -match '\((\d+)/(\d+)\)') {
            $covered = [int]::Parse($Matches[1], $invariant)
            $total = [int]::Parse($Matches[2], $invariant)
            if (-not $branchesMap.ContainsKey($key) -or $covered -gt $branchesMap[$key][0]) {
              $branchesMap[$key] = @($covered, $total)
            }
          }
        }
      }
    }
  }
}

$totalLines = 0
$coveredLines = 0
$totalBranches = 0
$coveredBranches = 0

foreach ($pkgName in $packageLines.Keys) {
  foreach ($h in $packageLines[$pkgName].Values) {
    $totalLines++
    if ($h -gt 0) { $coveredLines++ }
  }
  foreach ($b in $packageBranches[$pkgName].Values) {
    $coveredBranches += $b[0]
    $totalBranches += $b[1]
  }
}

$overallLineRate = if ($totalLines -gt 0) { ($coveredLines / [double]$totalLines) * 100.0 } else { 100.0 }
$overallBranchRate = if ($totalBranches -gt 0) { ($coveredBranches / [double]$totalBranches) * 100.0 } else { 100.0 }

Write-Host ([string]::Format($invariant, 'Coverage reports merged: {0}', $reports.Count))
Write-Host ([string]::Format($invariant, 'Line coverage:   {0:F2} % ({1}/{2}, minimum {3} %)', $overallLineRate, $coveredLines, $totalLines, $Threshold))
Write-Host ([string]::Format($invariant, 'Branch coverage: {0:F2} % ({1}/{2}, minimum {3} %)', $overallBranchRate, $coveredBranches, $totalBranches, $Threshold))

$failed = $overallLineRate -lt $Threshold -or $overallBranchRate -lt $Threshold

foreach ($name in $RequiredPackages) {
  if (-not $packageLines.ContainsKey($name) -or $packageLines[$name].Count -eq 0) {
    Write-Host "Package $name`: missing from the coverage reports"
    $failed = $true
    continue
  }

  $pkgTotalLines = $packageLines[$name].Count
  $pkgCoveredLines = @($packageLines[$name].Values | Where-Object { $_ -gt 0 }).Count
  $pkgLineRate = ($pkgCoveredLines / [double]$pkgTotalLines) * 100.0

  $pkgTotalBranches = 0
  $pkgCoveredBranches = 0
  foreach ($b in $packageBranches[$name].Values) {
    $pkgCoveredBranches += $b[0]
    $pkgTotalBranches += $b[1]
  }
  $pkgBranchRate = if ($pkgTotalBranches -gt 0) { ($pkgCoveredBranches / [double]$pkgTotalBranches) * 100.0 } else { 100.0 }

  Write-Host ([string]::Format($invariant, 'Package {0}: line {1:F2} % ({2}/{3}), branch {4:F2} % ({5}/{6})', $name, $pkgLineRate, $pkgCoveredLines, $pkgTotalLines, $pkgBranchRate, $pkgCoveredBranches, $pkgTotalBranches))

  if ($pkgLineRate -lt $Threshold -or $pkgBranchRate -lt $Threshold) {
    $failed = $true
  }
}

if ($failed) {
  Write-Error ([string]::Format($invariant, 'Code coverage is below the minimum of {0} % or a required package is missing.', $Threshold))
  exit 1
}
