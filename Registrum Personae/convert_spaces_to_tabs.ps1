Get-ChildItem -Recurse -Path "Assets" -Filter "*.cs" | ForEach-Object {
    $file = $_.FullName
    $content = [System.IO.File]::ReadAllText($file)
    $pattern = '^ +'
    $replacement = {
        $match = $args[0]
        $spaces = $match.Value.Length
        $tabs = [math]::Floor($spaces / 4)
        "`t" * $tabs
    }
    $newContent = [regex]::Replace($content, $pattern, $replacement, [System.Text.RegularExpressions.RegexOptions]::Multiline)
    [System.IO.File]::WriteAllText($file, $newContent)
}