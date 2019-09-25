﻿using System;
using Foundation;

namespace MacXibless.XPCService
{
    [Register("XpcServiceMain")]
    public class Program : NSObject, INSXpcListenerDelegate, IXpcProtocol
    {
        static void Main(string[] args)
        {
            AppKit.NSApplication.Init();
            ObjCRuntime.Runtime.RegisterAssembly(typeof(Program).Assembly);

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
            newConnection.ExportedInterface = NSXpcInterface.CreateForType(typeof(XpcProtocol));
            newConnection.ExportedObject = this;
            return true;
        }

        [Export("getHelloString:returnBlock:")]
        public void GetHelloString(string toWhom, Action<string> returnBlock)
        {
            returnBlock($"Hello, {toWhom}!");
        }
    }
}
