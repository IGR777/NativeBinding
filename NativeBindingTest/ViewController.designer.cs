// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NativeBindingTest
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIImageView arrowImageView { get; set; }

		[Outlet]
		UIKit.UIProgressView bundleLoadingProgressView { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView busyIndicator { get; set; }

		[Outlet]
		UIKit.UIButton cancelOperationsButton { get; set; }

		[Outlet]
		UIKit.UIImageView coloredRingImageView { get; set; }

		[Outlet]
		UIKit.UITextView debugTextView { get; set; }

		[Outlet]
		UIKit.UIView debugView { get; set; }

		[Outlet]
		UIKit.UILabel debugViewTitleLabel { get; set; }

		[Outlet]
		UIKit.UIButton doAlbumIdButton { get; set; }

		[Outlet]
		UIKit.UIButton doRecognizeButton { get; set; }

		[Outlet]
		UIKit.UIImageView gracenoteLogoImageView { get; set; }

		[Outlet]
		UIKit.UIButton idNowButton { get; set; }

		[Outlet]
		UIKit.UITableView resultsTableView { get; set; }

		[Outlet]
		UIKit.UITableView searchFieldsTableView { get; set; }

		[Outlet]
		UIKit.UIButton settingsButton { get; set; }

		[Outlet]
		UIKit.UIButton showDebugConsoleButton { get; set; }

		[Outlet]
		UIKit.UIButton showHistoryButton { get; set; }

		[Outlet]
		UIKit.UIButton showOrHideVisualizationButton { get; set; }

		[Outlet]
		UIKit.UIView showOrHideVisualizationButtonView { get; set; }

		[Outlet]
		UIKit.UIButton showTextSearchButton { get; set; }

		[Outlet]
		UIKit.UILabel statusCaptionLabel { get; set; }

		[Outlet]
		UIKit.UILabel statusLabel { get; set; }

		[Outlet]
		UIKit.UIView textSearchView { get; set; }

		[Outlet]
		UIKit.UILabel titleLabel { get; set; }

		[Outlet]
		UIKit.UIView visualizationView { get; set; }

		[Action ("cancelAllOperations:")]
		partial void cancelAllOperations (UIKit.UIButton sender);

		[Action ("doAlbumID:")]
		partial void doAlbumID (UIKit.UIButton sender);

		[Action ("doRecognizeMedia:")]
		partial void doRecognizeMedia (UIKit.UIButton sender);

		[Action ("idNow:")]
		partial void idNow (UIKit.UIButton sender);

		[Action ("showDebugConsole:")]
		partial void showDebugConsole (UIKit.UIButton sender);

		[Action ("showHistory:")]
		partial void showHistory (UIKit.UIButton sender);

		[Action ("showSettings:")]
		partial void showSettings (UIKit.UIButton sender);

		[Action ("showTextSearch:")]
		partial void showTextSearch (UIKit.UIButton sender);

		[Action ("showVisualization:")]
		partial void showVisualization (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (arrowImageView != null) {
				arrowImageView.Dispose ();
				arrowImageView = null;
			}

			if (bundleLoadingProgressView != null) {
				bundleLoadingProgressView.Dispose ();
				bundleLoadingProgressView = null;
			}

			if (busyIndicator != null) {
				busyIndicator.Dispose ();
				busyIndicator = null;
			}

			if (cancelOperationsButton != null) {
				cancelOperationsButton.Dispose ();
				cancelOperationsButton = null;
			}

			if (coloredRingImageView != null) {
				coloredRingImageView.Dispose ();
				coloredRingImageView = null;
			}

			if (debugTextView != null) {
				debugTextView.Dispose ();
				debugTextView = null;
			}

			if (debugView != null) {
				debugView.Dispose ();
				debugView = null;
			}

			if (debugViewTitleLabel != null) {
				debugViewTitleLabel.Dispose ();
				debugViewTitleLabel = null;
			}

			if (doAlbumIdButton != null) {
				doAlbumIdButton.Dispose ();
				doAlbumIdButton = null;
			}

			if (doRecognizeButton != null) {
				doRecognizeButton.Dispose ();
				doRecognizeButton = null;
			}

			if (gracenoteLogoImageView != null) {
				gracenoteLogoImageView.Dispose ();
				gracenoteLogoImageView = null;
			}

			if (idNowButton != null) {
				idNowButton.Dispose ();
				idNowButton = null;
			}

			if (resultsTableView != null) {
				resultsTableView.Dispose ();
				resultsTableView = null;
			}

			if (searchFieldsTableView != null) {
				searchFieldsTableView.Dispose ();
				searchFieldsTableView = null;
			}

			if (settingsButton != null) {
				settingsButton.Dispose ();
				settingsButton = null;
			}

			if (showHistoryButton != null) {
				showHistoryButton.Dispose ();
				showHistoryButton = null;
			}

			if (showOrHideVisualizationButton != null) {
				showOrHideVisualizationButton.Dispose ();
				showOrHideVisualizationButton = null;
			}

			if (showOrHideVisualizationButtonView != null) {
				showOrHideVisualizationButtonView.Dispose ();
				showOrHideVisualizationButtonView = null;
			}

			if (showTextSearchButton != null) {
				showTextSearchButton.Dispose ();
				showTextSearchButton = null;
			}

			if (statusCaptionLabel != null) {
				statusCaptionLabel.Dispose ();
				statusCaptionLabel = null;
			}

			if (statusLabel != null) {
				statusLabel.Dispose ();
				statusLabel = null;
			}

			if (textSearchView != null) {
				textSearchView.Dispose ();
				textSearchView = null;
			}

			if (titleLabel != null) {
				titleLabel.Dispose ();
				titleLabel = null;
			}

			if (visualizationView != null) {
				visualizationView.Dispose ();
				visualizationView = null;
			}

			if (showDebugConsoleButton != null) {
				showDebugConsoleButton.Dispose ();
				showDebugConsoleButton = null;
			}
		}
	}
}
