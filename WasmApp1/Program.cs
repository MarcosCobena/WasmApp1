using System;
using System.Diagnostics;
using WebAssembly;
using WebGLDotNET;

namespace WasmApp1
{
    internal class Program
    {
        private static JSObject window;
        private static WebGL2RenderingContext gl;
        private static Action<double> loopAction = new Action<double>(Loop);

        private static Stopwatch stopwatch = new Stopwatch();
        private static int iteration_counter = 0;
        private static int iteration_counter_global = 0;
        private static double do_frame_elapsed_ms = 0;
        private static double do_frame_elapsed_ms_global = 0;
        private static double iteration_elapsed_ms;
        private static double iteration_elapsed_ms_global = 0;
        private static double previousTimestampMilliseconds = 0;

        private static void Main()
        {
            window = (JSObject)Runtime.GetGlobalObject();
            var document = (JSObject)window.GetObjectProperty("document");
            var canvas = (JSObject)document.Invoke("getElementById", "canvas");
            var contextAttributes = new WebGLContextAttributes { Antialias = false };
            gl = new WebGL2RenderingContext(canvas, contextAttributes);
            gl.ClearColor(1.0f, 0.0f, 0.0f, 0.0f);
            Loop(0);
        }

        private static void Loop(double timestampMilliseconds)
        {
            stopwatch.Restart();
            gl.Clear(WebGLRenderingContextBase.COLOR_BUFFER_BIT);
            stopwatch.Stop();

            iteration_counter++;
            iteration_elapsed_ms += timestampMilliseconds - previousTimestampMilliseconds;
            previousTimestampMilliseconds = timestampMilliseconds;
            do_frame_elapsed_ms += stopwatch.Elapsed.TotalMilliseconds;
            
            if (iteration_counter >= 6000)
            {
                iteration_counter_global += iteration_counter;
                do_frame_elapsed_ms_global += do_frame_elapsed_ms;
                iteration_elapsed_ms_global += iteration_elapsed_ms;
                Console.WriteLine(
                    $"glClear: {do_frame_elapsed_ms / iteration_counter} ms, " +
                    $"mean: {do_frame_elapsed_ms_global / iteration_counter_global} ms");
                Console.WriteLine(
                    $"FPS: {(iteration_counter / iteration_elapsed_ms) * 1000}, " +
                    $"mean: {(iteration_counter_global / iteration_elapsed_ms_global) * 1000}");
                iteration_counter = 0;
                iteration_elapsed_ms = 0;
                do_frame_elapsed_ms = 0;
            }

            window.Invoke("requestAnimationFrame", loopAction);
        }
    }
}
