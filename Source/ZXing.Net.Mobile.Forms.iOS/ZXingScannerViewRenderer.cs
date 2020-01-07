using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using System.Reflection;
using Foundation;
using ZXing.Net.Mobile.Forms.iOS;
using UIKit;
using ZXing.Mobile;
using ZXingScannerView = ZXing.Net.Mobile.Forms.ZXingScannerView;

[assembly:ExportRenderer(typeof(ZXingScannerView), typeof(ZXingScannerViewRenderer))]
namespace ZXing.Net.Mobile.Forms.iOS
{
    [Preserve(AllMembers = true)]
    public class ZXingScannerViewRenderer : ViewRenderer<ZXingScannerView, BaseiOSScannerView>
    {   
        // No-op to be called from app to prevent linker from stripping this out    
        public static void Init ()
        {
            var temp = DateTime.Now;
        }

        protected ZXingScannerView formsView;
        protected BaseiOSScannerView zxingView;

		protected override void OnElementChanged(ElementChangedEventArgs<ZXingScannerView> e)
        {
            AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

            formsView = Element;

            if (zxingView == null) {

                // Process requests for autofocus
                formsView.AutoFocusRequested += (x, y) => {
                    if (zxingView != null) {
                        if (x < 0 && y < 0)
                            zxingView.AutoFocus ();
                        else
                            zxingView.AutoFocus (x, y);
                    }
                };

                if (formsView.Options.UseNativeScanning)
                {
					Version sv = new Version(0, 0, 0);
					Version.TryParse(UIDevice.CurrentDevice.SystemVersion, out sv);

					var is7orgreater = sv.Major >= 7;
					var allRequestedFormatsSupported = true;
					allRequestedFormatsSupported = AVCaptureScannerView.SupportsAllRequestedBarcodeFormats(formsView.Options.PossibleFormats);

					if (is7orgreater && allRequestedFormatsSupported)
					{
						Console.WriteLine("Using AVCapture for barcode decoding");
						zxingView = new AVCaptureScannerView();
					}
					else
					{
						if (!is7orgreater)
						{
							Console.WriteLine("Not iOS 7 or greater, cannot use AVCapture for barcode decoding, using ZXing instead");
						}						
						else
						{
							Console.WriteLine("Not all requested barcode formats were supported by AVCapture, using ZXing instead");
						}

						zxingView = new ZXing.Mobile.ZXingScannerView();
					}
				}
                else
                {
	                Console.WriteLine("Using ZXing for barcode decoding");
					zxingView = new ZXing.Mobile.ZXingScannerView();
				}

                
                zxingView.UseCustomOverlayView = true;
                zxingView.CustomOverlayView = new UIView();
                zxingView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

                base.SetNativeControl (zxingView);

                if (formsView.IsScanning)
                    zxingView.StartScanning (formsView.RaiseScanResult, formsView.Options);

                if (!formsView.IsAnalyzing)
                    zxingView.PauseAnalysis ();

                if (formsView.IsTorchOn)
                    zxingView.Torch (formsView.IsTorchOn);
            }

            base.OnElementChanged (e);
        }

        protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged (sender, e);

            if (zxingView == null)
                return;

            switch (e.PropertyName) {
            case nameof (ZXingScannerView.IsTorchOn):
                zxingView.Torch (formsView.IsTorchOn);
                break;
            case nameof (ZXingScannerView.IsScanning):
                if (formsView.IsScanning)
                    zxingView.StartScanning (formsView.RaiseScanResult, formsView.Options);
                else
                    zxingView.StopScanning ();
                break;
            case nameof (ZXingScannerView.IsAnalyzing):
                if (formsView.IsAnalyzing)
                    zxingView.ResumeAnalysis ();
                else
                    zxingView.PauseAnalysis ();
                break;
            } 
        }

        public override void TouchesEnded (NSSet touches, UIKit.UIEvent evt)
        {
            base.TouchesEnded (touches, evt);

            zxingView.AutoFocus ();
        }

        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();

            // Find the best guess at current orientation
            var o = UIApplication.SharedApplication.StatusBarOrientation;
            if (ViewController != null)
                o = ViewController.InterfaceOrientation;

            // Tell the native view to rotate
            zxingView.DidRotate (o);
        }
    }
}

