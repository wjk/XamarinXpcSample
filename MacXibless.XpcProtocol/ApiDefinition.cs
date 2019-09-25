using System;

using AppKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

namespace MacXibless
{
    [Protocol]
    [BaseType(typeof(NSObject))]
    interface XpcProtocol
	{
        [Export("getHelloString:returnBlock:")]
		void GetHelloString(string toWhom, Action<string> returnBlock);
	}
}
