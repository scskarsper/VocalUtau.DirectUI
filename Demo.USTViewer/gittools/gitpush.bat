"%~dp0\git.exe" add -A
"%~dp0\git.exe" commit -m "%1£¨%DATE:~4,4%%DATE:~9,2%%DATE:~12,2%%TIME:~0,2%%TIME:~3,2%%TIME:~6,2%£©"
"%~dp0\git.exe" push