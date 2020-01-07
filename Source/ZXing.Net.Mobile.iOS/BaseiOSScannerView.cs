using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace ZXing.Mobile
{
	public abstract class BaseiOSScannerView : UIView, IiOSScannerView
	{
		protected BaseiOSScannerView()
		{
			
		}

		protected BaseiOSScannerView(IntPtr handle) : base(handle)
		{
		}

		protected BaseiOSScannerView(CGRect frame) : base(frame)
		{
		}

		/// <inheritdoc />
		public abstract void StartScanning(Action<Result> scanResultHandler, MobileBarcodeScanningOptions options = null);

		/// <inheritdoc />
		public abstract void StopScanning();

		/// <inheritdoc />
		public abstract void PauseAnalysis();

		/// <inheritdoc />
		public abstract void ResumeAnalysis();

		/// <inheritdoc />
		public abstract void Torch(bool @on);

		/// <inheritdoc />
		public abstract void AutoFocus();

		/// <inheritdoc />
		public abstract void AutoFocus(int x, int y);

		/// <inheritdoc />
		public abstract void ToggleTorch();

		/// <inheritdoc />
		public abstract bool IsTorchOn { get; }

		/// <inheritdoc />
		public abstract bool IsAnalyzing { get; }

		/// <inheritdoc />
		public abstract bool HasTorch { get; }

		/// <inheritdoc />
		public abstract UIView CustomOverlayView { get; set; }

		/// <inheritdoc />
		public abstract bool UseCustomOverlayView { get; set; }

		/// <inheritdoc />
		public abstract string TopText { get; set; }

		/// <inheritdoc />
		public abstract string BottomText { get; set; }

		/// <inheritdoc />
		public abstract MobileBarcodeScanningOptions ScanningOptions { get; set; }

		/// <inheritdoc />
		public abstract event Action OnCancelButtonPressed;

		/// <inheritdoc />
		public abstract string CancelButtonText { get; set; }

		/// <inheritdoc />
		public abstract string FlashButtonText { get; set; }

		/// <inheritdoc />
		public abstract bool SetupCaptureSession();

		/// <inheritdoc />
		public abstract void DidRotate(UIInterfaceOrientation orientation);

		/// <inheritdoc />
		public abstract void ResizePreview(UIInterfaceOrientation orientation);
	}
}
