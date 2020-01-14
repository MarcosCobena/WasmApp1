using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using WebAssembly;
using WebAssembly.Core;

namespace WasmApp1
{
    internal class Program
    {
        private const string ImageFilename = "FlightHelmet_normal1.png";

        private static async void Main()
        {
            var httpClient = new HttpClient();
            var bytes = await httpClient.GetByteArrayAsync("http://127.0.0.1:9000/" + ImageFilename);
            File.WriteAllBytes(ImageFilename, bytes);

            // Load image with different approaches: ImageSharp, Skia, Rust, etc.
            var approaches = new Dictionary<string, Action<FileStream>>
            {
                // Doesn't support Wasm
                //{ nameof(SystemDrawing), SystemDrawing },
                // Currently unsupported with Mono's Wasm:
                // https://gitter.im/uno-platform/Lobby?at=5df7895cdbf24d0becfc8dc7
                //{ nameof(Skia), Skia },
                // Takes too long surely because of lacking SIMD, and IL being interpreted
                { nameof(ImageSharp), ImageSharp },
                { nameof(CanvasKit), CanvasKit },
            };

            var stream = File.OpenRead(ImageFilename);
            var stopwatch = new Stopwatch();

            foreach (var item in approaches)
            {
                stream.Position = 0;
                Trace.WriteLine($"Starting '{item.Key}'...");
                stopwatch.Restart();
                item.Value(stream);
                stopwatch.Stop();
                Trace.WriteLine($"Stopped! {stopwatch.ElapsedMilliseconds} ms");
            }
        }

        private static void SystemDrawing(FileStream stream) => System.Drawing.Image.FromStream(stream);

        private static void Canvas(FileStream stream)
        {
            //var image = Runtime.NewJSObject((JSObject)Runtime.GetGlobalObject("Image"));
        }

        private static void CanvasKit(FileStream stream)
        {
            // https://github.com/google/skia/blob/master/modules/canvaskit/interface.js#L842
            var canvasKit = (JSObject)Runtime.GetGlobalObject("CanvasKitHandle");
            JSObject image;

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);

                using (var array = Uint8Array.From(memoryStream.ToArray()))
                {
                    image = (JSObject)canvasKit.Invoke("MakeImageFromEncoded", array);
                }
            }
            
            Debug.WriteLine($"Image size: {image.Invoke("width")}x{image.Invoke("height")}");

            var imageInfo = Runtime.NewJSObject();
            imageInfo.SetObjectProperty("width", image.Invoke("width"));
            imageInfo.SetObjectProperty("height", image.Invoke("height"));
            imageInfo.SetObjectProperty(
                "alphaType",
                ((JSObject)canvasKit.GetObjectProperty("AlphaType")).GetObjectProperty("Unpremul"));
            var colorType = ((JSObject)canvasKit.GetObjectProperty("ColorType")).GetObjectProperty("RGBA_8888");
            // canvaskit.js minified expects "Dm" instead of "colorType", although passes imageInfo later to 
            // WebAssembly code and "colorType" needs to still be there
            imageInfo.SetObjectProperty("colorType", colorType);
            imageInfo.SetObjectProperty("Dm", colorType);

            var pixels = image.Invoke("readPixels", imageInfo, 0, 0);
            Debug.WriteLine($"Pixels: {pixels?.GetType()}");
        }

        private static void Skia(FileStream stream) => SkiaSharp.SKBitmap.Decode(stream);

        private static void ImageSharp(FileStream stream)
        {
            SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(
                stream, 
                new SixLabors.ImageSharp.Formats.Png.PngDecoder());
        }
    }
}
