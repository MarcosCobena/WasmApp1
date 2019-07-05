using System;
using WebAssembly;

namespace WasmApp1
{
    internal class Program
    {
        private static void Main()
        {
            {
                using (var document = (JSObject)Runtime.GetGlobalObject("document"))
                using (var body = (JSObject)document.GetObjectProperty("body"))
                using (var button = (JSObject)document.Invoke("createElement", "button"))
                {
                    button.SetObjectProperty("innerHTML", "Click me!");
                    button.SetObjectProperty(
                        "onclick", 
                        new Action<JSObject>(_ => 
                        {
                            using (var window = (JSObject)Runtime.GetGlobalObject())
                            {
                                window.Invoke("alert", "Hello, Wasm!");
                            }
                        }));
                    body.Invoke("appendChild", button);
                }
            }
        }
    }
}
