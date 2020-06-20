# McRename ![.NET Core](https://github.com/Nuss9/McRename/workflows/.NET%20Core/badge.svg?branch=master&event=push)

A small console application to batch rename files in a desktop directory.\
Works cross-platform, Windows, macOS and Linux.\
\
Currently one folder can be chosen to select all files within to rename, excluding directories and hidden files.\
Files will be ordered by file name, then by the timestamp of their creation.

The following **options** are available to rename the files:
1. Numerical      *Ascending numbers, starting at 1.*
2. Date           *Date of creation, in the format YYYYMMDD.*
3. Date time      *Date and time of creation, in the format YYYYMMDD_HHMMSS.*
4. Custom text    *The custom text provided by the user will be used for file names.*
5. Trucation      *The custom text provided by the user will be used to trim from the file names.*
6. Extension      *Modify the file extensions only.*

**Note** that it's possible to create duplicate file names. In these cases, the program will automatically append a number between brackets. *E.g. fileA_(1).png, fileA_(2).png*

![Program example](https://github.com/[Nuss9]/[McRename]/blob/[master]/Screenshot.png?raw=true)
