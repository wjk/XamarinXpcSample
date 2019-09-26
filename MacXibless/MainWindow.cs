using System;
using Foundation;
using AppKit;
using CoreGraphics;

namespace MacXibless
{
    public class MainWindow : NSWindow
    {
        #region Computed Properties
        public NSButton ClickMeButton { get; set; }
        public NSTextField ClickMeLabel { get; set; }
        #endregion

        #region Constructors

        public MainWindow(CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation) : base(contentRect, aStyle, bufferingType, deferCreation)
        {
            // Define the User Interface of the Window here
            Title = "Window From Code";

            // Create the content view for the window and make it fill the window
            ContentView = new NSView(Frame);

            // Add UI Elements to window
            ClickMeButton = NSButton.CreateButton("Click Me!", ButtonClicked);
            ClickMeButton.TranslatesAutoresizingMaskIntoConstraints = false;
            ContentView.AddSubview(ClickMeButton);

            ClickMeLabel = NSTextField.CreateLabel("Button has not been clicked yet.");
            ClickMeLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            ContentView.AddSubview(ClickMeLabel);

            // Add constraints to window
            ContentView.AddConstraints(NSLayoutConstraint.FromVisualFormat(
                "V:|-[button]-[label]", NSLayoutFormatOptions.AlignAllLeading,
                "button", ClickMeButton, "label", ClickMeLabel));
            ContentView.AddConstraints(NSLayoutConstraint.FromVisualFormat(
                "H:|-[button]", NSLayoutFormatOptions.AlignAllFirstBaseline,
                "button", ClickMeButton));
            ContentView.AddConstraints(NSLayoutConstraint.FromVisualFormat(
                "H:|-[label]", NSLayoutFormatOptions.AlignAllFirstBaseline,
                "label", ClickMeLabel));

            var constraints = NSLayoutConstraint.FromVisualFormat(
                "H:[label]-|", NSLayoutFormatOptions.AlignAllFirstBaseline,
                "label", ClickMeLabel);
            ContentView.AddConstraints(NSLayoutConstraint.FromVisualFormat(
                "H:[button]-(>=std)-|", NSLayoutFormatOptions.AlignAllFirstBaseline,
                "button", ClickMeButton, "std", constraints[0].Constant));
            ContentView.AddConstraints(NSLayoutConstraint.FromVisualFormat(
                "H:[label]-(>=std)-|", NSLayoutFormatOptions.AlignAllFirstBaseline,
                "label", ClickMeLabel, "std", constraints[0].Constant));

            constraints = NSLayoutConstraint.FromVisualFormat(
                "V:[label]-|", NSLayoutFormatOptions.AlignAllLeft,
                "label", ClickMeLabel);
            ContentView.AddConstraints(NSLayoutConstraint.FromVisualFormat(
                "V:[label]-(>=std)-|", NSLayoutFormatOptions.AlignAllFirstBaseline,
                "label", ClickMeLabel, "std", constraints[0].Constant));
        }

        #endregion

        private void ButtonClicked()
        {
            bool connectionSuccessful = false;

            var protocol = new ObjCRuntime.Protocol(typeof(IXpcProtocol));
            NSXpcConnection connection = new NSXpcConnection("com.your-company.MacXiblessXPCService");
            connection.RemoteInterface = NSXpcInterface.Create(protocol);
            connection.InvalidationHandler = () =>
            {
                if (!connectionSuccessful)
                {
                    ClickMeLabel.StringValue = "Connection failed!";
                }
            };

            connection.Resume();
            var remoteObject = connection.CreateRemoteObjectProxy<IXpcProtocol>();
            remoteObject.GetHelloString(new NSString("World"), (retval) =>
            {
                InvokeOnMainThread(() =>
                {
                    ClickMeButton.Title = retval;
                    connectionSuccessful = true;
                    connection.Invalidate();
                });
            });
        }
    }
}
