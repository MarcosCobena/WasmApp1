using System;
using WebAssembly;

namespace WasmApp1
{
    internal class Program
    {
        private static void Main()
        {
            System.Diagnostics.Trace.TraceError("This should have crashed");
        }
    }
}
