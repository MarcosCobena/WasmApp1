using System;
using System.IO;
using System.Threading.Tasks;
using WebAssembly;

namespace WasmApp1
{
    internal class Program
    {
        private static TaskCompletionSource<bool> taskCompletionSource;

        private static async void Main()
        {
            const string root = "/persistent_data";

            // IndexedDB doesn't work in private mode under Firefox
            var fs = (JSObject)Runtime.GetGlobalObject("FS");
            fs.Invoke("mkdir", root);
            fs.Invoke("mount", Runtime.GetGlobalObject("IDBFS"), new WebAssembly.Core.Array(), root);
            var onSyncfsCallback = new Action<JSObject>(OnSyncfs);
            taskCompletionSource = new TaskCompletionSource<bool>();
            fs.Invoke("syncfs", true, onSyncfsCallback);
            var isSyncSuccess = await taskCompletionSource.Task;

            if (!isSyncSuccess)
            {
                return;
            }

            var path = $"{root}/foo.txt";

            if (File.Exists(path))
            {
                var content = File.ReadAllText(path);
                Console.WriteLine(content);
            }

            File.WriteAllText(path, DateTime.UtcNow.ToString());

            taskCompletionSource = new TaskCompletionSource<bool>();
            fs.Invoke("syncfs", false, onSyncfsCallback);
        }

        public static void OnSyncfs(JSObject value)
        {
            if (value != null)
            {
                taskCompletionSource.SetResult(false);
                return;
            }

            taskCompletionSource.SetResult(true);
        }
    }
}
