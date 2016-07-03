using System;

using UIKit;
using nativeTest;

namespace NativeBindingTest
{
	public partial class ViewController : UIViewController
	{
		protected ViewController (IntPtr handle) : base (handle) {
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad () {
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib
			var a = new GnAssetFetch ();
			var b =a.Size;
		}

		public override void DidReceiveMemoryWarning () {
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

