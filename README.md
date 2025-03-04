# Word Counter:

## Overview:
Console application to parse mulitple txt files, using parallel processing, to find unique words and record their frequency. 

## Build and Run:
dotnet build

dotnet run

## Test:
dotnet test

## Example config file:
filename: appsettings.json

{

  "TextFilesFolder": "./TextFiles",  
  
  "IgnoreCaseFlag": "true",
  
  "MaxThreadsOption": "4"
  
}

## Sample Output:
Test: 132

do: 5

in: 4

that: 3

ut: 3

so: 2

not: 2

it: 2

