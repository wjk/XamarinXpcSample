using System;

using AppKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

namespace XamarinXpcTemplate
{
    [Preserve, XpcInterface]
    [Protocol(Name = "XamarinXpcTemplateInterfaceProtocol")]
    [BaseType(typeof(NSObject))]
    interface IXpcInterfaceProtocol
    {
    }
}
