A Simple Snake Game Written in C#

## 🔨 To Compile it Yourself

#### For Self Included Builds.
```
dotnet publish -c Release -p:PublishReadyToRun=true -p:PublishTrimmed=true -p:PublishSingleFile=true --self-contained true --runtime <TargetPlatform>
```

#### For Framework Dependent Builds.
```
dotnet publish -c Release -p:PublishReadyToRun=true -p:PublishSingleFile=true --self-contained false --runtime <TargetPlatform>
```

Built-in available platforms (<TargetPlatform>) : 
- win-x86 / win-x64 / win-arm / win-arm64
- linux-86 / linux-x64 / linux-musl-x64 / linux-arm / linux-arm64
- osx-x64 / osx-arm64

Dependencies to compile:
- .Net 7.0
