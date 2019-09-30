using System;

using AppKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

namespace MacXibless
{
    delegate void GetHelloStringReturnBlock(NSString message);

    [Preserve, XpcInterface]
    [Protocol(Name = "XamarinXpcProtocol")]
    [BaseType(typeof(NSObject))]
    interface XpcProtocol
	{
        [Export("getHelloString:returnBlock:")]
		void GetHelloString(NSString toWhom, [BlockCallback] GetHelloStringReturnBlock returnBlock);
	}
}
