A Simple Snake Game Written in C#

## 🔨 To Compile it Yourself

#### For Self Included Builds.
```
dotnet publish -c Release -p:PublishReadyToRun=true -p:PublishTrimmed=true -p:PublishSingleFile=true --self-contained true --runtime {platform}
```

#### For Framework Dependent Builds.
```
dotnet publish -c Release -p:PublishReadyToRun=true -p:PublishSingleFile=true --self-contained false --runtime
```

Dependencies to compile:
- .Net 7.0
