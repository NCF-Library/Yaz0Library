# Yaz0 Library

C# managed wrapper and implementations for sYaz0 compression and decompression.

### Usage

#### Compress

```cs
// Compress a file
Span<byte> compressed = Yaz0.Compress("path/to/uncompressed_file.bin", out Yaz0SafeHandle handle);
```

```cs
// Compress a byte[]
byte[] uncompressed = sarc["path/to/uncompressed_file.bin"];
Span<byte> compressed = Yaz0.Compress(uncompressed, out Yaz0SafeHandle handle);
```

#### Decompress

```cs
// Decompress a file
Span<byte> decompressed = Yaz0.Decmpress("path/to/compressed_file.sbin");
```

```cs
// Decompress a byte[]
byte[] compressed = sarc["path/to/compressed_file.sbin"];
Span<byte> decompressed = Yaz0.Decompress(compressed);
```

### Install

[![NuGet](https://img.shields.io/nuget/v/Yaz0Library.svg)](https://www.nuget.org/packages/Yaz0Library) [![NuGet](https://img.shields.io/nuget/dt/Yaz0Library.svg)](https://www.nuget.org/packages/Yaz0Library)

#### NuGet
```powershell
Install-Package Yaz0Library
```

#### Build From Source
```batch
git clone https://github.com/NCF-Library/Yaz0Library.git
dotnet build Yaz0Library
```

### Credit

**[ArchLeaders](https://github.com/ArchLeaders):** C# oead wrapper ([Cead](https://github.com/ArchLeaders/Cead))<br>
**[exelix](https://github.com/exelix11):** C# Managed [Yaz0 implementation](https://github.com/exelix11/EditorCore/blob/master/FileFormatPlugins/SARCLib/Sarc/Yaz0Compression.cs).<br>
**[Léo Lam](https://github.com/leoetlino):** [oead](https://github.com/zeldamods/oead)<br>