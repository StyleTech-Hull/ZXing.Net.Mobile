using System;
using CoreGraphics;
using UIKit;

namespace ZXing.Mobile
{
	public interface IiOSScannerView : IZXingScanner<UIView>, IScannerSessionHost
	{
		event Action OnCancelButtonPressed;
		string CancelButtonText { get; set; }
		string FlashButtonText { get; set; }
		bool SetupCaptureSession ();
		void DidRotate(UIInterfaceOrientation orientation);
		void ResizePreview (UIInterfaceOrientation orientation);
	}
}
