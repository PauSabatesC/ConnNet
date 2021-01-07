1.- Change .csproj version
2.- 
```
dotnet pack
```
3.- 
```
dotnet nuget push SockNet/bin/Debug/SockNet.X.X.X.nupkg --api-key xxxxxxxxxxxxxxxxxxxxxx  --source https://api.nuget.org/v3/index.json
```
