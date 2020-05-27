using Aspose.CAD;
using System;
using System.Reflection;

namespace WasmApp1
{
    internal class Program
    {
        private static void Main()
        {
            // Uncomment to workaround missing WebAssembly.Bindings assembly when disabling linker
            //WebAssembly.JSObject x = null;

            var assembly = Assembly.GetExecutingAssembly();

            // DWG taken from: https://www.bloquesautocad.com/mapa-provincia-madrid/
            using (var stream = assembly.GetManifestResourceStream("WasmApp1.comunidad_madrid.dwg"))
            {
                var loadOptions = new LoadOptions 
                { 
                    // Uncomment to workaround the 1252 encoding error; however, another will surface
                    //SpecifiedEncoding = CodePages.Utf8 
                };
                var image = Image.Load(stream, loadOptions);
            }
            
            Console.WriteLine("Success!");
        }
    }
}
