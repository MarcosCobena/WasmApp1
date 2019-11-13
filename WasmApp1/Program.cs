using System;
using System.IO;

namespace WasmApp1
{
    internal class Program
    {
        private static void Main()
        {
            // (A working Emscripten installation is needed in advance. You should get a similar output to —at tools/:
            //   $ python file_packager.py
            //   Usage: file_packager.py TARGET[--preload A[B..]] [--embed C [D..]] [--exclude E [F..]]]
            //   [--js-output=OUTPUT.js] [--no-force] [--use-preload-cache] [--indexedDB-name=EM_PRELOAD_CACHE]
            //   [--no-heap-copy] [--separate-metadata] [--lz4] [--use-preload-plugins]
            //   See the source for more details.
            // )
            // $ python $EMSCRIPTEN_PATH/tools/file_packager.py mono.dat --js-output=mono-loader.js --preload Program.cs
            // Remember to build the main file with - s FORCE_FILESYSTEM = 1  so that it includes support for loading
            // this file package
            // $ cp mono.dat bin/Debug/netstandard2.0/
            // $ cp mono-loader.js bin/Debug/netstandard2.0/
            // Add mono-loader.js into index.html:
            //   <script src="runtime.js"></script>
            //   <!-- Here: --><script src="mono-loader.js"></script>
            //   <script defer src="mono.js"></script>
            var content = File.ReadAllText("Program.cs");
            Console.WriteLine(content);
        }
    }
}
