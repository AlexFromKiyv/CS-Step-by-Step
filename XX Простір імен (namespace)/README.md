# Простір імен (namespace)

Простір імен (namespace) містить типи у вигляді класів згуповані по призначенню. Наприклад у випадку System.Console простір імен це System а Console це клас в ньому. Для того шоб ми могли визивати методи цього класу компілятоу потривна вказівка де цей клас може бути. При компфляції коду генерується файл obj\Debug\netX.0\<ProjectName>.GlobalUsings.g.cs в якому вказуються ти простори імен які потрібні майже в кожному файлі программи. 

Створемо просте рішення з консольним проектом.

1. Solution name: Namespaces
2. Project name: Namespaces

Якшо нажати Show All Files і подивитись obj\Debug\netX.0\Namespaces.GlobalUsings.g.cs то він буде виглядати:
```cs
global using global::System;
global using global::System.Collections.Generic;
global using global::System.IO;
global using global::System.Linq;
global using global::System.Net.Http;
global using global::System.Threading;
global using global::System.Threading.Tasks;
```
Відкриємо файл проекту двійним кліком на назві Namespaces
Замінемо його на наступний:
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net7.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <Using Remove='System.Collections.Generic;' />
    <Using Remove='System.IO' />
    <Using Remove='System.Linq' />
    <Using Remove='System.Net.Http' />
    <Using Remove='System.Threading' />
    <Using Remove='System.Threading.Tasks;' />
    <Using Include='System.Numerics' />
  </ItemGroup>
</Project>
```
Коли ми збережемо файл то побачимо в Namespaces.GlobalUsings.g.cs 
```cs
global using global::System;
global using global::System.Numerics;
```
Тобто цей файл коррегує сам компілятор.


