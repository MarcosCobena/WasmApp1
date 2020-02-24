using System;
using System.Buffers;
using System.Diagnostics;
using WebAssembly;
using WebAssembly.Core;

namespace WasmApp1
{
    internal class Program
    {
        private static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();

            using (var document = (JSObject)Runtime.GetGlobalObject("document"))
            using (var body = (JSObject)document.GetObjectProperty("body"))
            using (var button = (JSObject)document.Invoke("createElement", "button"))
            {
                button.SetObjectProperty("innerHTML", "Rent array");
                button.SetObjectProperty(
                    "onclick", 
                    new Action<JSObject>(_ => 
                    {
                        const int imageSide = 8 * 1024;
                        
                        stopWatch.Restart();

                        var pixelsComingFromCanvasKit = new Uint8Array(imageSide * imageSide * 4);
                        var array = ArrayPool<byte>.Shared.Rent(pixelsComingFromCanvasKit.Length);
                        pixelsComingFromCanvasKit.CopyTo(array);

                        stopWatch.Stop();

                        Trace.WriteLine($"{stopWatch.ElapsedMilliseconds} ms");
                    }));
                body.Invoke("appendChild", button);
            }
        }
    }
}
