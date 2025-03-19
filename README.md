**IMPORTANT:** It is 2025 and the are much better, and more updated, approaches to .NET in Wasm: see [Uno.Wasm.Bootstrap](https://github.com/unoplatform/uno.wasm.bootstrap) or [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor), for instance.

# Your first Wasm app with C#

__Originally posted on [Marcos Cobeña](https://github.com/MarcosCobena)'s [blog, 6/7/2019](https://marcoscobena.com/?i=wasmapp1)__

As part of our work with WebGL.NET have learned how to be aligned with Mono's latest Wasm progress. It's taking the shape of something which could make users File > New and start creating apps with such but, still needs some progress. However, I ask my-self whether everyone out there would like to start writing C# to target Wasm but still not worrying on configuring their environment in any way.

Frank Krueger and Jérôme Laban have made great works with their NuGets Ouui.Wasm and Uno.Wasm.Bootstrap, respectively, but as soon as you want to go deeper in JavaScript & .NET communication (consuming the DOM in a richer way, for instance), you'll surely enjoy having the peeper Mono is working on.

For all this, I've set up WasmApp1 repo, the minimum stuff needed I guess, to give all the power for targeting Wasm. You just can go and clone or download it, and start working. I'd like to keep it updated with the latest changes from Mono, but what I dream with is deleting such in the upcoming months because finally it's all integrated into stable, and File > New > Wasm App would be just there :-)

In the meantime, this is a session for start running your app. The last command (xsp4) serves the output folder at http://localhost:9000/, and it's Mono's integrated ASP.NET server for CLI, which comes with regular Mono downloads; you can use anything for serving such actually, or just upload it to your own web server:

```cmd
C:\Users\Marcos\source\repos\WasmApp1>dotnet build
Microsoft (R) Build Engine versión 15.8.169+g1ccb72aefa para .NET Core
Copyright (C) Microsoft Corporation. Todos los derechos reservados.

  Restauración realizada en 54,5 ms para C:\Users\Marcos\source\repos\WasmApp1\WasmApp1\WasmApp1.csproj.

Compilación correcta.
    0 Advertencia(s)
    0 Errores

Tiempo transcurrido 00:00:01.63

C:\Users\Marcos\source\repos\WasmApp1>xsp4 WasmApp1\bin\Debug\netstandard2.0\
xsp4
Listening on address: 0.0.0.0
Root directory: C:\Users\Marcos\source\repos\WasmApp1\WasmApp1\bin\Debug\netstandard2.0
Listening on port: 9000 (non-secure)
Hit Return to stop the server.
```

Now browse http://localhost:9000/ in your prefered app (Wasm is widely supported) and enjoy!
