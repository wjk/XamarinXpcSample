using System;
using Foundation;

namespace XamarinXpcTemplate.XpcService
{
    [Register("XamarinXpcTemplateMain")]
    public class Program : NSObject, INSXpcListenerDelegate, IXpcInterfaceProtocol
    {
        static void Main(string[] args)
        {
            AppKit.NSApplication.Init();

            using (Program program = new Program())
                program.Run();
        }

        void Run()
        {
            NSXpcListener listener = NSXpcListener.ServiceListener;
            listener.Delegate = this;
            listener.Resume();
            // Resume() does not return.
        }

        [Export("listener:shouldAcceptNewConnection:")]
        public bool ShouldAcceptConnection(NSXpcListener listener, NSXpcConnection newConnection)
        {
            newConnection.ExportedInterface = NSXpcInterface.Create(typeof(IXpcInterfaceProtocol));
            newConnection.ExportedObject = this;
            newConnection.Resume();
            return true;
        }
    }
}
