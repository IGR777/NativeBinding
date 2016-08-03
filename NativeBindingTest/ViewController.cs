using System;
using UIKit;
using nativeTest;
using System.Collections.Generic;
using CoreGraphics;
using System.Linq;
using Foundation;
using System.Threading.Tasks;
using AVFoundation;
using ObjCRuntime;
using CoreAnimation;
using MediaPlayer;
using CoreData;

namespace NativeBindingTest
{
    enum Mode
    {
        UnknownMode,
        TextSearchMode,
        SettingsMode,
        HistoryMode,
        DebugMode,
        AdditionalContentMode
    };


    public partial class ViewController : UIViewController, IGnAudioSourceDelegate, IGnStatusEventsDelegate
    , IGnLookupLocalStreamIngestEventsDelegate, IGnMusicIdStreamEventsDelegate, IUIActionSheetDelegate
    {

        string CLIENTID = "25023801";
        string CLIENTIDTAG = "67B35D9B271F886AA419B63C3A607615";
        string License = @"-- BEGIN LICENSE v1.0 70068011 --\r\nname: \r\nnotes: Gracenote Open Developer Program\r\nstart_date: 0000-00-00\r\nclient_id: 25023801\r\nmusicid_file: enabled\r\nmusicid_text: enabled\r\nmusicid_stream: enabled\r\nmusicid_cd: enabled\r\nplaylist: enabled\r\nvideoid: enabled\r\nvideo_explore: enabled\r\nlocal_images: enabled\r\nlocal_mood: enabled\r\nvideoid_explore: enabled\r\nacr: enabled\r\nepg: enabled\r\n-- SIGNATURE 70068011 --\r\nlAADAgAeN5CVkNnCMV8kxrLwa+qHthpxFk796sX+KA2HLCgxAB5N2kiNfbIirf/vrn4A3IJW70xZwM2socwMiA9gNeA=\r\n-- END LICENSE 70068011 --\r\n";
        readonly nint ALBUMTITLELABELTAG = 7000;
        readonly nint TRACKTITLELABELTAG = 7001;
        readonly nint ARTISTLABELTAG = 7002;
        readonly nint TRACKDURATIONLABELTAG = 7003;
        readonly nint CAPTIONLABELTAG = 7004;
        readonly nint TEXTFIELDTAG = 7005;
        readonly nint SETTINGSSWITCHTAG = 7006;
        readonly nint ALBUMIDACTIONSHEETTAG = 7007;
        readonly nint ALBUMCOVERARTIMAGETAG = 7008;
        readonly nint ADDITIONALMETADATAVIEWTAG = 7009;
        readonly nint TRACKMATCHPOSITIONLABELTAG = 7010;
        readonly nint ADDITIONALCONTENTVIEWTAG = 7011;
        readonly nint ADDITIONALCONTENTIMAGEVIEWTAG = 7012;
        readonly nint ADDITIONALCONTENTALBUMLABELTAG = 7013;
        readonly nint ADDITIONALCONTENTARTISTLABELTAG = 7014;
        readonly nint ADDITIONALCONTENTTEXTVIEWTAG = 7015;
        readonly nint ADDITIONALCONTENTTITLELABELTAG = 7016;

        readonly nint LABELWIDTHIPHONE = 150;
        readonly nint LABELWIDTHIPAD = 420;

        readonly nint kHeightOfAdditionalMetadataCell = 260;
        readonly nint kHeightOfAdditionalMetadataCellPad = 360;

        string DEBUGMODEKEY = "debug-mode-on";
        string LOCALLOOKUPOPTIONONLY = "local-lookupoption-only";
        bool _recordingIsPaused;
        bool _microphoneIsInitialized;
        Mode _currentMode;
        bool _lookupSourceIsLocal;
        bool _audioProcessingStarted;
        bool _visualizationIsVisible;
        bool _enableDebugRefreshTimer;
        double _queryBeginTimeInterval = -1;
        double _queryEndTimeInterval = -1;
        List<object> _cancellableObjects = new List<object>();
        List<object> _albumDataMatches = new List<object>();
        List<object> _results = new List<object>();
        List<object> _arrayOfLogStrings = new List<object>();
        List<object> _history = new List<object>();
        NSIndexPath _currentlySelectedIndexPath;

        UIDynamicAnimator _dynamicAnimator;
        UISegmentedControl _searchSegmentedControl;
        UISegmentedControl _cancelSegmentedControl;

        GnMusicIdStream gnMusicIdStream;
        public GnUser gnUser;
        public GnManager gnManager;
        public GnUserStore gnUserStore;
        public GnLocale locale;
        public GnLog gnLog;
        public GnStorageSqlite gnStorageSqlite;
        GnLookupLocalStream gnLookupLocalStream;
        GnMic gnMic;

        string ApplicationDocumentsDirectory
        {
            get
            {
                NSError error = null;
                return NSFileManager.DefaultManager.GetUrl(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User, null, false, out error).Path;
            }
        }

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib
            NSError error = null;

            _currentMode = Mode.UnknownMode;

            SetupInterface();

            showOrHideVisualizationButton.TouchUpInside += ShowVisualization;
            idNowButton.TouchUpInside += IdNow;
            cancelOperationsButton.TouchUpInside += CancelAllOperations;
            doAlbumIdButton.TouchUpInside += DoAlbumID;
            doRecognizeButton.TouchUpInside += DoRecognizeMedia;

            var session = AVAudioSession.SharedInstance();
            session.SetPreferredSampleRate(44100, out error);
            session.SetInputGain(0.5f, out error);
            session.SetActive(true);

            //    [[NSNotificationCenter defaultCenter]
            //		addObserver:self
            //		   selector:@selector (applicationResignedActive:)

            //												 name:UIApplicationWillResignActiveNotification
            //											   object:nil];

            //    [[NSNotificationCenter defaultCenter]
            //		addObserver:self
            //		   selector:@selector (applicationDidBecomeActive:)

            //												 name:UIApplicationDidBecomeActiveNotification
            //											   object:nil];

            //    // Check if both ClientID and ClientIDTag have been set.
            if (string.IsNullOrEmpty(CLIENTID) || string.IsNullOrEmpty(CLIENTIDTAG))
            {
                statusLabel.Text = "Please set Client ID and Client Tag.";
                return;
            }
            //    // -------------------------------------------------------------------------------
            //    // Initialize GNSDK.
            //    // -------------------------------------------------------------------------------
            InitializeGnSDK();
            titleLabel.Text = string.Format("Gracenote SDK {0}", GnManager.ProductVersion);
            //InitializeDebugLogging ();

            try
            {
                gnStorageSqlite = GnStorageSqlite.Enable(out error);
                gnStorageSqlite.StorageLocationWithFolderPath(ApplicationDocumentsDirectory, out error);

                SetupLocalLookup();

                DownloadLatestBundle();

                if (session.RespondsToSelector(new Selector("requestRecordPermission:")))
                {
                    session.RequestRecordPermission((granted) =>
                    {
                        if (granted)
                        {
                            MicrophoneInitialize();
                        }
                        else {
                            UpdateStatus("Michrophone update status");
                        }
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
                return;
            }
        }

        void SetupInterface()
        {
            textSearchView.Alpha = 0.0f;
            arrowImageView.Alpha = 0.0f;
            textSearchView.Layer.CornerRadius = 5.0f;
            searchFieldsTableView.Layer.CornerRadius = 5.0f;

            //		//Setup Dynamic Animator.
            _dynamicAnimator = new UIDynamicAnimator(visualizationView);

            //		//Resize Visualization
            var visualizationRect = visualizationView.Frame;
            visualizationRect.Y -= visualizationRect.Size.Height - (showOrHideVisualizationButtonView.Frame.Size.Height + 10);
            visualizationView.Frame = visualizationRect;
            showOrHideVisualizationButton.TitleLabel.Text = "Show Visualization";
            showOrHideVisualizationButtonView.Layer.CornerRadius = 10.0f;
            showOrHideVisualizationButtonView.Layer.BorderWidth = 1.0f;
            showOrHideVisualizationButtonView.Layer.BorderColor = UIColor.White.CGColor;
            visualizationView.Layer.CornerRadius = 5.0f;
            visualizationView.BackgroundColor = UIColor.FromRGBA(0.2f, 0.2f, 0.2f, 0.2f);

            //		//Add Search and Cancel Buttons.
            _searchSegmentedControl = new UISegmentedControl(new[] { "Search" });

            int offset = 20;
            int width = 100;
            int height = 35;


            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
            {
                offset = 100;
                width = 200;
                height = 50;
            }

            _searchSegmentedControl.Frame = new CGRect(searchFieldsTableView.Frame.X + offset,
                                                        searchFieldsTableView.Frame.Y + searchFieldsTableView.Frame.Size.Height + 10,
                                                        width, height);
            _searchSegmentedControl.Layer.MasksToBounds = true;
            _searchSegmentedControl.Momentary = true;
            _searchSegmentedControl.BackgroundColor = UIColor.Red;
            _searchSegmentedControl.TintColor = UIColor.White;
            _searchSegmentedControl.Layer.CornerRadius = 5.0f;
            _searchSegmentedControl.Layer.BorderColor = UIColor.Green.CGColor;
            _searchSegmentedControl.AddTarget((sender, args) => DoTextSearch(), UIControlEvent.ValueChanged);

            textSearchView.AddSubview(_searchSegmentedControl);

            _cancelSegmentedControl = new UISegmentedControl(new[] { "Cancel" });

            var interfaceWidth = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 80 : 40;
            _cancelSegmentedControl.Frame = new CGRect(_searchSegmentedControl.Frame.X + _searchSegmentedControl.Frame.Size.Width + interfaceWidth, _searchSegmentedControl.Frame.Y, width, height);

            _cancelSegmentedControl.Momentary = true;
            _cancelSegmentedControl.BackgroundColor = UIColor.Black;
            _cancelSegmentedControl.TintColor = UIColor.White;
            _cancelSegmentedControl.Layer.CornerRadius = 5.0f;
            _cancelSegmentedControl.Layer.BorderColor = UIColor.Green.CGColor;
            _cancelSegmentedControl.AddTarget((sender, args) => CloseSearchTextView(), UIControlEvent.ValueChanged);

            textSearchView.AddSubview(_cancelSegmentedControl);

            idNowButton.Layer.ShadowColor = UIColor.DarkGray.CGColor;
            idNowButton.Layer.ShadowOffset = new CGSize(0, 0);
            idNowButton.Layer.ShadowRadius = 10.0f;
            idNowButton.Layer.ShadowOpacity = 1;
            idNowButton.Layer.MasksToBounds = false;

            textSearchView.Layer.ShadowColor = UIColor.DarkGray.CGColor;
            textSearchView.Layer.ShadowOffset = new CGSize(0, 0);
            textSearchView.Layer.ShadowRadius = 25.0f;
            textSearchView.Layer.ShadowOpacity = 1;
            textSearchView.Layer.MasksToBounds = false;

            _currentlySelectedIndexPath = null;


            //   //Setup Debug View.

            debugView.Layer.CornerRadius = 5.0f;
            debugViewTitleLabel.Layer.CornerRadius = 5.0f;
            debugView.Alpha = 0.0f;
            debugTextView.TextColor = UIColor.Black;
            debugTextView.BackgroundColor = UIColor.White;
        }

        void SetupLocalLookup()
        {

            NSError error = null;

            gnLookupLocalStream = GnLookupLocalStream.Enable(out error);
            if (error == null)
            {
                var docDir = ApplicationDocumentsDirectory;
                gnLookupLocalStream.StorageLocation(docDir, out error);
            }
        }

        void DownloadLatestBundle()
        {
            NSError error = null;
            var bundlePath = NSBundle.MainBundle.PathForResource("1557.b", null);

            if (bundlePath == null)
            {
                gnLookupLocalStream.StorageClear(out error);
                if (error == null)
                {
                    GnLookupLocalStreamIngest lookupLocalStreamIngest = new GnLookupLocalStreamIngest(this);

                    Task.Run(() =>
                    {
                        nint bytesRead = 0;
                        double totalBytesRead = 0;
                        byte[] buffer = new byte[1024];
                        var fileInputStream = new NSInputStream(bundlePath);

                        fileInputStream.Open();
                        do
                        {
                            bytesRead = fileInputStream.Read(buffer, 1024);
                            lookupLocalStreamIngest.Write(NSData.FromArray(buffer), (nuint)buffer.Length, out error);
                        } while (bytesRead > 0);
                        lookupLocalStreamIngest.Flush(out error);
                        fileInputStream.Close();
                    });
                }
            }
        }

        void MicrophoneInitialize()
        {
            gnMic = new GnMic(44100, 16, 1);

            if (gnMic != null)
            {
                _microphoneIsInitialized = true;
                SetupMusicIdStream();
            }
        }

        void SetupMusicIdStream()
        {
            if (gnUser == null)
                return;

            _recordingIsPaused = false;

            NSError musicIDStreamError = null;

            try
            {
                gnMusicIdStream = new GnMusicIdStream(gnUser, GnMusicIdStreamPreset.kPresetMicrophone, locale, this);

                musicIDStreamError = null;
            }
            catch (Exception e)
            {
            }

            //			@try
            //	{
            //				self.gnMusicIDStream = [[GnMusicIdStream alloc] initWithGnUser: self.gnUser preset:kPresetMicrophone locale:self.locale musicIdStreamEventsDelegate: self];

            //				musicIDStreamError = nil;
            //				GnMusicIdStreamOptions* options = [self.gnMusicIDStream options];
            //		[options resultSingle:YES error:&musicIDStreamError];
            //		[options lookupData:kLookupDataSonicData enable:YES error:&musicIDStreamError];
            //		[options lookupData:kLookupDataContent enable:YES error:&musicIDStreamError];
            //		[options preferResultCoverart:YES error:&musicIDStreamError];

            //		musicIDStreamError = nil;
            //		dispatch_async (self.internalQueue, ^

            //		{
            //			self.gnAudioVisualizeAdapter = [[GnAudioVisualizeAdapter alloc] initWithAudioSource: self.gnMic audioVisualizerDelegate:self];

            //			self.idNowButton.enabled = NO; //disable stream-ID until audio-processing-started callback is received


            //			[self.gnMusicIDStream audioProcessStartWithAudioSource:(id<GnAudioSourceDelegate>)self.gnAudioVisualizeAdapter error:&musicIDStreamError];

            //			if (musicIDStreamError)
            //			{

            //				dispatch_async (dispatch_get_main_queue(), ^
            //                {

            //					NSLog (@"Error while starting Audio Process With AudioSource - %@", [musicIDStreamError localizedDescription]);
            //	});
            //            }
            //        });
            //	}
            //	@catch (NSException* exception) {
            //	NSLog (@"Error: %@ - %@ - %@", [exception name], [exception reason], [exception userInfo]);
            //}
        }

        void DoTextSearch()
        {
            string artistName = null;
            string albumTitle = null;
            string trackTitle = null;
            GnMusicId musicId = null;
            NSError error = null;

            var indexPaths = searchFieldsTableView.IndexPathsForVisibleRows.ToArray();
            cancelOperationsButton.Enabled = true;
            _results.Clear();
            _currentlySelectedIndexPath = null;
            foreach (var indexPath in indexPaths)
            {
                var textfield = (UITextField)searchFieldsTableView.CellAt(indexPath).ContentView.ViewWithTag(TEXTFIELDTAG);
                if (indexPath.Row == 0)
                    artistName = textfield.Text;
                else if (indexPath.Row == 1)
                    albumTitle = textfield.Text;
                else if (indexPath.Row == 2)
                    trackTitle = textfield.Text;
            }
            try
            {
                musicId = new GnMusicId(gnUser, this);
                _cancellableObjects.Add(musicId);
                musicId.Options.LookupData(GnLookupData.kLookupDataContent, true, out error);
                _queryBeginTimeInterval = DateTime.Now.Ticks;
                EnableOrDisableControls(false);

                Task.Run(() =>
                {
                    NSError textSearchOperationError = null;
                    var responseAlbums = musicId.FindAlbumsWithAlbumTitle(albumTitle, trackTitle, artistName, artistName, null, out textSearchOperationError);
                    InvokeOnMainThread(() =>
                    {
                        _queryEndTimeInterval = DateTime.Now.Ticks;
                        _cancellableObjects.Remove(musicId);
                        EnableOrDisableControls(true);
                        ProcessAlbumResponseAndUpdateResultsTable(responseAlbums);
                        CloseSearchTextView();
                    });
                });
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public void StatusEvent(GnStatus status, int percentComplete, int bytesTotalSent, int bytesTotalReceived, GnCancellableDelegate canceller)
        {
            string statusString = null;

            switch (status)
            {
                case GnStatus.kStatusUnknown:
                    statusString = "Status Unknown";
                    break;

                case GnStatus.kStatusBegin:
                    statusString = "Status Begin";
                    break;

                case GnStatus.kStatusProgress:
                    break;

                case GnStatus.kStatusComplete:
                    statusString = "Status Complete";
                    break;

                case GnStatus.kStatusErrorInfo:
                    statusString = "No Match";
                    StopBusyIndicator();

                    break;

                case GnStatus.kStatusConnecting:
                    statusString = "Status Connecting";
                    break;

                case GnStatus.kStatusSending:
                    statusString = "Status Sending";
                    break;

                case GnStatus.kStatusReceiving:
                    statusString = "Status Receiving";
                    break;

                case GnStatus.kStatusDisconnected:
                    statusString = "Status Disconnected";
                    break;

                case GnStatus.kStatusReading:
                    statusString = "Status Reading";
                    break;

                case GnStatus.kStatusWriting:
                    statusString = "Status Writing";
                    break;

                case GnStatus.kStatusCancelled:
                    statusString = "Status Cancelled";
                    break;
            }

            UpdateStatus(statusString);
        }

        void StopBusyIndicator()
        {
            this.InvokeOnMainThread(() =>
            {
                EnableOrDisableControls(true);
                busyIndicator.StopAnimating();
            });
        }

        void EnableOrDisableControls(bool enable)
        {
            doAlbumIdButton.Enabled = enable;
            doRecognizeButton.Enabled = enable;
            showHistoryButton.Enabled = enable;
            showTextSearchButton.Enabled = enable;
            idNowButton.Enabled = enable && _audioProcessingStarted;
            settingsButton.Enabled = enable;
            cancelOperationsButton.Enabled = !enable;

            resultsTableView.UserInteractionEnabled = enable;
            resultsTableView.ScrollEnabled = enable;
        }

        void UpdateStatus(string status)
        {
            this.InvokeOnMainThread(() =>
            {
                statusLabel.Text = status;
            });
        }

        void ScrollPlaybackViewToVisibleRect()
        {
            var cell = resultsTableView.CellAt(_currentlySelectedIndexPath);
            resultsTableView.ScrollRectToVisible(cell.Frame, true);
        }


        void ProcessAlbumResponseAndUpdateResultsTable(GnResponseAlbums albums)
        {
            //-(void)processAlbumResponseAndUpdateResultsTable:(id)responseAlbums
            //{
            //    id albums = nil;
            //		__block const unsigned char* Imgdata;
            //		__block NSUInteger size;
            //    __block NSData* img;

            //    if([responseAlbums isKindOfClass:[GnResponseAlbums class]])
            //        albums = [responseAlbums albums];
            //    else
            //        albums = responseAlbums;

            //    for(GnAlbum* album in albums)
            //    {
            //		GnTrackEnumerator* tracksMatched = [album tracksMatched];
            //		NSString* albumArtist = [[[album artist] name] display];
            //        NSString* albumTitle = [[album title] display];
            //        NSString* albumGenre = [album genre: kDataLevel_1];
            //		NSString* albumID = [NSString stringWithFormat: @"%@-%@", [album tui], [album tuiTag]];
            //		GnExternalId* externalID = nil;
            //        if ([album externalIds] && [[album externalIds]
            //		allObjects].count)
            //            externalID = (GnExternalId*) [[album externalIds]
            //		nextObject];

            //        NSString* albumXID = [externalID source];
            //		NSString* albumYear = [album year];
            //		NSString* albumTrackCount = [NSString stringWithFormat: @"%lu", (unsigned long)[album trackCount]];
            //        NSString* albumLanguage = [album language];

            //		/* Get CoverArt */
            //		GnContent* coverArtContent = [album coverArt];
            //		GnAsset* coverArtAsset = [coverArtContent asset: kImageSizeSmall];
            //		NSString* URLString = [NSString stringWithFormat: @"http://%@", [coverArtAsset url]];

            //		GnContent* artistImageContent = [[[album artist] contributor] image];
            //        GnAsset* artistImageAsset = [artistImageContent asset: kImageSizeSmall];
            //		NSString* artistImageURLString = [NSString stringWithFormat: @"http://%@", [artistImageAsset url]];

            //		GnContent* artistBiographyContent = [[[album artist] contributor] biography];
            //        NSString* artistBiographyURLString = [NSString stringWithFormat: @"http://%@", [[[artistBiographyContent assets] nextObject] url]];

            //        GnContent* albumReviewContent = [album review];
            //		NSString* albumReviewURLString = [NSString stringWithFormat: @"http://%@", [[[albumReviewContent assets] nextObject] url]];

            //        __block GnDataModel *gnDataModelObject = [[GnDataModel alloc]
            //		init];
            //        gnDataModelObject.albumArtist = albumArtist;
            //        gnDataModelObject.albumGenre = albumGenre;
            //        gnDataModelObject.albumID = albumID;
            //        gnDataModelObject.albumXID = albumXID;
            //        gnDataModelObject.albumYear = albumYear;
            //        gnDataModelObject.albumTitle = albumTitle;
            //        gnDataModelObject.albumTrackCount = albumTrackCount;
            //        gnDataModelObject.albumLanguage = albumLanguage;

            //        __weak GnViewController *weakSelf = self;

            //        if (nil != [coverArtAsset url])
            //        {

            //			dispatch_async (dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), ^{
            //                    GnAssetFetch* gnAssetFetch = [[GnAssetFetch alloc] initWithGnUser:self.gnUser url:URLString statusEventsDelegate:self];
            //                    Imgdata = [gnAssetFetch data];
            //                    size = [gnAssetFetch size];
            //                    img = [NSData dataWithBytes: Imgdata length:size];
            //                    gnDataModelObject.albumImageData = img;
            //                    [weakSelf.resultsTableView reloadData];
            //            });
            //        }

            //       if (nil != [artistImageAsset url])
            //       {

            //		   dispatch_async (dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), ^{
            //                   GnAssetFetch* gnAssetFetch = [[GnAssetFetch alloc] initWithGnUser:self.gnUser url:artistImageURLString statusEventsDelegate:self];
            //                   Imgdata = [gnAssetFetch data];
            //                   size = [gnAssetFetch size];
            //                   img = [NSData dataWithBytes: Imgdata length:size];
            //                   gnDataModelObject.artistImageData = img;
            //                   [weakSelf.resultsTableView reloadData];
            //                   [self refreshArtistImage];
            //           });
            //       }

            //       if (nil != [[[artistBiographyContent assets]
            //nextObject] url])
            //       {

            //		   dispatch_async (dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), ^{
            //                  GnAssetFetch* gnAssetFetch = [[GnAssetFetch alloc] initWithGnUser:self.gnUser url:artistBiographyURLString statusEventsDelegate:self];
            //                  Imgdata = [gnAssetFetch data];
            //                  size = [gnAssetFetch size];
            //                  img = [NSData dataWithBytes: Imgdata length:size];
            //                  gnDataModelObject.artistBiography = [[NSString alloc]
            //initWithBytes:img.bytes length:img.length encoding:NSUTF8StringEncoding];

            //           });
            //        }

            //        if (nil != [[[albumReviewContent assets]
            //nextObject] url])
            //        {

            //			dispatch_async (dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), ^{
            //                  GnAssetFetch* gnAssetFetch = [[GnAssetFetch alloc] initWithGnUser:self.gnUser url:albumReviewURLString statusEventsDelegate:self];
            //                  Imgdata = [gnAssetFetch data];
            //                  size = [gnAssetFetch size];
            //                  img = [NSData dataWithBytes: Imgdata length:size];
            //                   gnDataModelObject.albumReview = [[NSString alloc]
            //initWithBytes:img.bytes length:img.length encoding:NSUTF8StringEncoding];

            //            });
            //        }



            //		NSLog (@"Matched Album = %@", [[album title]display]);

            //        for(GnTrack* track in tracksMatched)
            //        {


            //			NSLog (@"  Matched Track = %@", [[track title]display]);

            //            NSString* trackArtist =  [[[track artist] name] display];
            //            NSString* trackMood = [track mood: kDataLevel_1];
            //NSString* trackOrigin = [[[track artist] contributor] origin:kDataLevel_1];
            //            NSString* trackTempo = [track tempo: kDataLevel_1];
            //NSString* trackGenre =  [track genre: kDataLevel_1];
            //NSString* trackID =[NSString stringWithFormat: @"%@-%@", [track tui], [track tuiTag]];
            //NSString* trackDuration = [NSString stringWithFormat: @"%lu", (unsigned long) ( [track duration]/1000)];
            //            NSString* currentPosition = [NSString stringWithFormat: @"%zu", (NSUInteger) [track currentPosition] / 1000];
            //NSString* matchPosition = [NSString stringWithFormat: @"%zu", (NSUInteger) [track matchPosition] / 1000];


            //            if ([track externalIds] && [[track externalIds]
            //allObjects].count)
            //                externalID = (GnExternalId*) [[track externalIds]
            //nextObject];

            //            NSString* trackXID = [externalID source];
            //NSString* trackNumber = [track trackNumber];
            //NSString* trackTitle = [[track title] display];
            //            NSString* trackArtistType = [[[track artist] contributor] artistType:kDataLevel_1];

            //            //Allocate GnDataModel.
            //            gnDataModelObject.trackArtist = trackArtist;
            //            gnDataModelObject.trackMood = trackMood;
            //            gnDataModelObject.trackTempo = trackTempo;
            //            gnDataModelObject.trackOrigin = trackOrigin;
            //            gnDataModelObject.trackGenre = trackGenre;
            //            gnDataModelObject.trackID = trackID;
            //            gnDataModelObject.trackXID = trackXID;
            //            gnDataModelObject.trackNumber = trackNumber;
            //            gnDataModelObject.trackTitle = trackTitle;
            //            gnDataModelObject.trackArtistType = trackArtistType;
            //            gnDataModelObject.trackMatchPosition = matchPosition;
            //            gnDataModelObject.trackDuration = trackDuration;
            //            gnDataModelObject.currentPosition = currentPosition;
            //        }

            //        [self.results addObject:gnDataModelObject];

            //    }

            //    [self  performSelectorOnMainThread:@selector (refreshResults) withObject:nil waitUntilDone:YES];

            //    if ([[albums allObjects]
            //count])
            //    {
            //        [self performSelectorOnMainThread:@selector (saveResultsForHistory:) withObject:responseAlbums waitUntilDone:YES];
            //    }

            //}
        }

        void CloseSearchTextView()
        {

            foreach (var cell in searchFieldsTableView.VisibleCells)
            {
                var textField = (UITextField)cell.ContentView.ViewWithTag(TEXTFIELDTAG);
                if (textField.IsEditing)
                    textField.ResignFirstResponder();
            }

            _currentMode = Mode.UnknownMode;
            textSearchView.Alpha = 0.0f;
            arrowImageView.Alpha = 0.0f;
        }

        void InitializeGnSDK()
        {
            NSError error = null;
            try
            {
                gnManager = new GnManager(License, GnLicenseInputMode.kLicenseInputModeString);
                gnUserStore = new GnUserStore();
                gnUser = new GnUser(gnUserStore, CLIENTID, CLIENTIDTAG, "1.0.0.0");
                locale = new GnLocale(GnLocaleGroup.kLocaleGroupMusic, GnLanguage.kLanguageEnglish, GnRegion.kRegionGlobal, GnDescriptor.kDescriptorSimplified, gnUser, null);
                locale.SetGroupDefault(out error);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        void CloseAdditionalContentView()
        {
            if (_currentlySelectedIndexPath != null)
            {
                var cell = resultsTableView.CellAt(_currentlySelectedIndexPath);

                UIView.Animate(0.5, () =>
                {
                    var additionalContentView = cell.ContentView.ViewWithTag(ADDITIONALCONTENTVIEWTAG);
                    var frame = additionalContentView.Frame;
                    frame.X = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 768 : 320;
                    additionalContentView.Frame = frame;
                });
            }
        }

        void SettingsSwitchValueChanged(object sender)
        {
            var settingsSwitch = (UISwitch)sender;
            NSError error = null;
            if (settingsSwitch != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    UITableViewCell cell = null;
                    if (i == 0)
                        cell = (UITableViewCell)settingsSwitch.Superview.Superview;
                    else if (i == 1)
                        cell = (UITableViewCell)settingsSwitch.Superview.Superview.Superview;

                    if (cell == null)
                        break;

                    var indexPath = searchFieldsTableView.IndexPathForCell(cell);

                    if (indexPath == null)
                        continue;

                    switch (indexPath.Row)
                    {
                        case 0:
                            NSUserDefaults.StandardUserDefaults.SetBool(settingsSwitch.On, DEBUGMODEKEY);
                            if (settingsSwitch.On)
                            {
                                gnLog.EnableWithPackage(GnLogPackageType.kLogPackageAllGNSDK, out error);
                            }
                            else {
                                gnLog.DisableWithPackage(GnLogPackageType.kLogPackageAllGNSDK, out error);
                            }
                            if (error != null)
                            {
                                Console.WriteLine("Error - {0}, {1}, {2}", error.Code, error.Domain, error.LocalizedDescription);
                            }
                            break;
                        case 1:
                            if (gnMusicIdStream != null)
                            {
                                GnMusicIdStreamOptions options = gnMusicIdStream.Options;
                                if (settingsSwitch.On)
                                {
                                    options.LookupMode(GnLookupMode.kLookupModeLocal, out error);
                                }
                                else {
                                    options.LookupMode(GnLookupMode.kLookupModeOnline, out error);
                                }
                                NSUserDefaults.StandardUserDefaults.SetBool(settingsSwitch.On, LOCALLOOKUPOPTIONONLY);
                            }
                            break;
                    }
                }
            }

        }

        void ShowArtistBiography()
        {
            var cell = resultsTableView.CellAt(_currentlySelectedIndexPath);
            var additionalContentView = cell.ContentView.ViewWithTag(ADDITIONALCONTENTVIEWTAG);
            var additionalContentImageView = (UIImageView)additionalContentView.ViewWithTag(ADDITIONALCONTENTIMAGEVIEWTAG);
            var additionalContentAlbumLabel = (UILabel)additionalContentView.ViewWithTag(ADDITIONALCONTENTALBUMLABELTAG);
            var additionalContentArtistLabel = (UILabel)additionalContentView.ViewWithTag(ADDITIONALCONTENTARTISTLABELTAG);
            var additionalContentTextView = (UITextView)additionalContentView.ViewWithTag(ADDITIONALCONTENTTEXTVIEWTAG);
            var additionalContentTitleLabel = (UILabel)additionalContentView.ViewWithTag(ADDITIONALCONTENTTITLELABELTAG);
            additionalContentTitleLabel.Text = "Artist Biography";

            cell.ContentView.BringSubviewToFront(additionalContentView);
            GnDataModel datamodelObject = null;
            if (_results != null && _results.Count > _currentlySelectedIndexPath.Row)
            {
                datamodelObject = (GnDataModel)_results[_currentlySelectedIndexPath.Row];
            }

            var artistImage = new UIImage(datamodelObject.ArtistImageData);

            if (artistImage != null)
            {
                additionalContentImageView.Image = artistImage;
            }
            else {
                additionalContentImageView.Image = new UIImage("emptyImage.png");
            }

            additionalContentAlbumLabel.Text = datamodelObject.AlbumTitle;
            additionalContentAlbumLabel.Text = datamodelObject.AlbumArtist != null ? datamodelObject.AlbumArtist : datamodelObject.TrackArtist;
            additionalContentTextView.Text = datamodelObject.ArtistBiography != null ? datamodelObject.ArtistBiography : "Artist Biography is not currently available.";
            UIView.Animate(0.5f, () =>
            {
                var frame = additionalContentView.Frame;
                frame.X = 0;
                additionalContentView.Frame = frame;
            });

        }

        void ShowAlbumReview()
        {
            var cell = resultsTableView.CellAt(_currentlySelectedIndexPath);
            var additionalContentView = cell.ContentView.ViewWithTag(ADDITIONALCONTENTVIEWTAG);
            var additionalContentImageView = (UIImageView)additionalContentView.ViewWithTag(ADDITIONALCONTENTIMAGEVIEWTAG);
            var additionalContentAlbumLabel = (UILabel)additionalContentView.ViewWithTag(ADDITIONALCONTENTALBUMLABELTAG);
            var additionalContentArtistLabel = (UILabel)additionalContentView.ViewWithTag(ADDITIONALCONTENTARTISTLABELTAG);
            var additionalContentTextView = (UITextView)additionalContentView.ViewWithTag(ADDITIONALCONTENTTEXTVIEWTAG);
            var additionalContentTitleLabel = (UILabel)additionalContentView.ViewWithTag(ADDITIONALCONTENTTITLELABELTAG);
            additionalContentTitleLabel.Text = "Album Review";

            cell.ContentView.BringSubviewToFront(additionalContentView);

            GnDataModel dataModelObject = null;

            if (_results != null && _results.Count > _currentlySelectedIndexPath.Row)
            {
                dataModelObject = (GnDataModel)_results[_currentlySelectedIndexPath.Row];
            }

            var artistImage = new UIImage(dataModelObject.AlbumImageData);
            if (artistImage != null)
            {
                additionalContentImageView.Image = artistImage;
            }
            else {
                additionalContentImageView.Image = new UIImage("emptyImage.png");
            }

            additionalContentAlbumLabel.Text = dataModelObject.AlbumTitle;
            additionalContentArtistLabel.Text = dataModelObject.AlbumArtist != null ? dataModelObject.AlbumArtist : dataModelObject.TrackArtist;
            additionalContentTextView.Text = dataModelObject.AlbumReview != null ? dataModelObject.AlbumReview : "Album Review is not currently available.";
            UIView.Animate(0.5, () =>
            {
                var frame = additionalContentView.Frame;
                frame.X = 0;
                additionalContentView.Frame = frame;
            });
        }

        void AlbumIdButtonTapped(object sender)
        {
            _results.Clear();
            RefreshResults();
            _currentlySelectedIndexPath = null;

            var albumIDButton = (UIButton)sender;
            var albumIDRect = albumIDButton.Frame;
            albumIDRect = View.ConvertRectFromView(albumIDRect, albumIDButton.Superview);

            var albumIdActionsheet = new UIActionSheet("Album Id", this, "Cancel", null, new[] { "iPod-Library", "Documents Directory" });
            albumIdActionsheet.Tag = ALBUMIDACTIONSHEETTAG;

            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
            {
                albumIdActionsheet.ShowFrom(albumIDRect, this.View, true);
            }
            else {
                albumIdActionsheet.ShowInView(this.View);
            }
        }

        void RefreshResults()
        {
            if (_results.Count == 0)
            {
                UpdateStatus("No match");
            }
            else {
                UpdateStatus(String.Format("Found - {0}", _results.Count));
            }

            resultsTableView.ReloadData();
            busyIndicator.StopAnimating();
        }

        void ActionButtonTapped(object sender)
        {
            _results.Clear();
            RefreshResults();
            _currentlySelectedIndexPath = null;

            var pickerController = new MPMediaPickerController(MPMediaType.Music);
            pickerController.Prompt = "Choose songs to identify";
            pickerController.AllowsPickingMultipleItems = true;
            pickerController.WeakDelegate = this;
            PresentViewController(pickerController, true, null);
        }

        #region Table source/delegate

        [Export("tableView:cellForRowAtIndexPath:")]
        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            string cellIdentifier = "CellID";
            var cell = tableView.DequeueReusableCell(cellIdentifier);
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
                if (tableView == resultsTableView || (tableView == searchFieldsTableView && _currentMode == Mode.HistoryMode))
                {
                    var widthHeight = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 112 : 56;
                    var imageView = new UIImageView(new CGRect(5, 10, widthHeight, widthHeight));
                    imageView.Tag = ALBUMCOVERARTIMAGETAG;
                    var width = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? LABELWIDTHIPAD : LABELWIDTHIPHONE;
                    var albumTitleLabel = new UILabel(new CGRect(imageView.Frame.X + imageView.Frame.Width + 12, 5, width, 40));
                    albumTitleLabel.Font = UIFont.BoldSystemFontOfSize(16);
                    albumTitleLabel.TextColor = UIColor.FromRGBA(0.7f, 0, 0.7f, 1);
                    albumTitleLabel.Tag = ALBUMTITLELABELTAG;
                    albumTitleLabel.Lines = 2;
                    albumTitleLabel.LineBreakMode = UILineBreakMode.CharacterWrap;
                    albumTitleLabel.BackgroundColor = UIColor.Clear;

                    var trackTitleLabel = new UILabel(new CGRect(albumTitleLabel.Frame.X + albumTitleLabel.Frame.Width + 5, albumTitleLabel.Frame.Y, 120, albumTitleLabel.Frame.Height));
                    trackTitleLabel.Font = UIFont.BoldSystemFontOfSize(12);
                    trackTitleLabel.TextColor = UIColor.DarkGray;
                    trackTitleLabel.Tag = TRACKTITLELABELTAG;
                    trackTitleLabel.Lines = 2;
                    trackTitleLabel.LineBreakMode = UILineBreakMode.CharacterWrap;
                    trackTitleLabel.BackgroundColor = UIColor.Clear;

                    var artistLabel = new UILabel(new CGRect(albumTitleLabel.Frame.X, +albumTitleLabel.Frame.Y + albumTitleLabel.Frame.Height, width, 40));
                    artistLabel.Font = UIFont.SystemFontOfSize(12);
                    artistLabel.TextColor = UIColor.DarkGray;
                    artistLabel.Tag = ARTISTLABELTAG;
                    artistLabel.Lines = 2;
                    artistLabel.LineBreakMode = UILineBreakMode.CharacterWrap;
                    artistLabel.BackgroundColor = UIColor.Clear;

                    var trackDurationLabel = new UILabel(new CGRect(artistLabel.Frame.X + artistLabel.Frame.Width + 5, artistLabel.Frame.Y, 120, 40));
                    trackDurationLabel.Font = UIFont.BoldSystemFontOfSize(12);
                    trackDurationLabel.TextColor = UIColor.Gray;
                    trackDurationLabel.Tag = TRACKDURATIONLABELTAG;
                    trackDurationLabel.BackgroundColor = UIColor.Clear;
                    var addConViewX = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 768 : 320;
                    var addConViewWidth = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 768 : cell.ContentView.Bounds.Width;
                    var addConViewHeight = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? kHeightOfAdditionalMetadataCellPad : kHeightOfAdditionalMetadataCell;
                    var additionalContentView = new UIView(new CGRect(addConViewX, 0, addConViewWidth, addConViewHeight));
                    additionalContentView.Tag = ADDITIONALCONTENTVIEWTAG;
                    additionalContentView.BackgroundColor = UIColor.FromRGBA(0.1f, 0.1f, 0.1f, 0.9f);

                    //			//Add content views to additional content view
                    var additionalContentTitleLabel = new UILabel(new CGRect(0, 0, additionalContentView.Frame.Width, 20));
                    additionalContentTitleLabel.Tag = ADDITIONALCONTENTTITLELABELTAG;
                    additionalContentTitleLabel.Layer.CornerRadius = 2.0f;
                    additionalContentTitleLabel.BackgroundColor = UIColor.DarkGray;
                    additionalContentTitleLabel.TextColor = UIColor.Black;
                    additionalContentTitleLabel.TextAlignment = UITextAlignment.Center;
                    additionalContentTitleLabel.Font = UIFont.BoldSystemFontOfSize(16);
                    additionalContentTitleLabel.UserInteractionEnabled = true;

                    var closeAdditionalSegment = new UISegmentedControl(new[] { "Close" });
                    closeAdditionalSegment.Momentary = true;
                    closeAdditionalSegment.UserInteractionEnabled = true;
                    var frame = new CGRect(additionalContentTitleLabel.Frame.X + additionalContentTitleLabel.Frame.Width - 50, 0, 50, 20);
                    closeAdditionalSegment.Frame = frame;
                    closeAdditionalSegment.BackgroundColor = UIColor.Red;
                    closeAdditionalSegment.TintColor = UIColor.White;
                    closeAdditionalSegment.AddTarget((sender, args) => CloseAdditionalContentView(), UIControlEvent.ValueChanged);
                    additionalContentTitleLabel.AddSubview(closeAdditionalSegment);

                    var additionalContentImageView = new UIImageView(new CGRect(5, 25, 56, 56));
                    additionalContentImageView.Tag = ADDITIONALCONTENTIMAGEVIEWTAG;


                    var additionalContentAlbumLabel = new UILabel(new CGRect(additionalContentImageView.Frame.X + additionalContentImageView.Frame.Width + 5, additionalContentImageView.Frame.Y, additionalContentView.Bounds.Width - (additionalContentImageView.Frame.Width + additionalContentImageView.Frame.X + 5), 40));
                    additionalContentAlbumLabel.Lines = 2;
                    additionalContentAlbumLabel.LineBreakMode = UILineBreakMode.CharacterWrap;
                    additionalContentAlbumLabel.BackgroundColor = UIColor.Clear;
                    additionalContentAlbumLabel.Tag = ADDITIONALCONTENTALBUMLABELTAG;
                    additionalContentAlbumLabel.TextColor = UIColor.White;

                    var additionalContentArtistLabel = new UILabel(new CGRect(additionalContentAlbumLabel.Frame.X, additionalContentAlbumLabel.Frame.Y + additionalContentAlbumLabel.Frame.Height + 5, additionalContentView.Bounds.Width - (additionalContentImageView.Frame.Width + additionalContentImageView.Frame.X + 5), 40));
                    additionalContentArtistLabel.Lines = 2;
                    additionalContentArtistLabel.LineBreakMode = UILineBreakMode.CharacterWrap;
                    additionalContentArtistLabel.BackgroundColor = UIColor.Clear;
                    additionalContentArtistLabel.Tag = ADDITIONALCONTENTARTISTLABELTAG;
                    additionalContentArtistLabel.TextColor = UIColor.White;

                    var additionalContentTextView = new UITextView(new CGRect(additionalContentImageView.Frame.X, additionalContentArtistLabel.Frame.Y + additionalContentArtistLabel.Frame.Height + 5, additionalContentView.Bounds.Width - 10, additionalContentView.Bounds.Height - (additionalContentArtistLabel.Frame.Y + additionalContentArtistLabel.Frame.Height + 10)));
                    additionalContentTextView.Editable = false;
                    additionalContentTextView.Text = null;
                    additionalContentTextView.Tag = ADDITIONALCONTENTTEXTVIEWTAG;
                    additionalContentTextView.Layer.CornerRadius = 5.0f;
                    additionalContentTextView.Font = UIFont.ItalicSystemFontOfSize(14.0f);
                    additionalContentTextView.TextColor = UIColor.DarkGray;
                    additionalContentTextView.ShowsVerticalScrollIndicator = true;

                    additionalContentView.AddSubview(additionalContentTitleLabel);
                    additionalContentView.AddSubview(additionalContentImageView);
                    additionalContentView.AddSubview(additionalContentAlbumLabel);
                    additionalContentView.AddSubview(additionalContentArtistLabel);
                    additionalContentView.AddSubview(additionalContentTextView);
                    cell.ContentView.AddSubview(imageView);
                    cell.ContentView.AddSubview(albumTitleLabel);
                    cell.ContentView.AddSubview(trackTitleLabel);
                    cell.ContentView.AddSubview(artistLabel);
                    cell.ContentView.AddSubview(trackDurationLabel);
                    cell.ContentView.AddSubview(additionalContentView);

                }
                else if (tableView == searchFieldsTableView && _currentMode == Mode.TextSearchMode)
                {
                    var captionLabel = new UILabel(new CGRect(5, 5, 100, 25));
                    captionLabel.Tag = CAPTIONLABELTAG;

                    var textfield = new UITextField(new CGRect(captionLabel.Frame.X + captionLabel.Frame.Width + 5, 5, cell.ContentView.Bounds.Width - (captionLabel.Frame.X + captionLabel.Frame.Width + 5), 25));
                    textfield.Tag = TEXTFIELDTAG;
                    cell.ContentView.AddSubview(captionLabel);
                    cell.ContentView.AddSubview(textfield);
                    cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                }
                else if (tableView == searchFieldsTableView && _currentMode == Mode.SettingsMode)
                {
                    var captionLabel = new UILabel(new CGRect(12, 12, tableView.Frame.Width / 2, 25));
                    captionLabel.Tag = CAPTIONLABELTAG;

                    var settingsSwitch = new UISwitch(new CGRect(tableView.Frame.Width - 62, 12, 50, 25));
                    settingsSwitch.AddTarget((sender, args) => SettingsSwitchValueChanged(sender), UIControlEvent.ValueChanged);
                }
                else if (tableView == searchFieldsTableView && _currentMode == Mode.SettingsMode)
                {
                    var captionLabel = new UILabel(new CGRect(12, 12, tableView.Frame.Width / 2, 25));
                    captionLabel.Tag = CAPTIONLABELTAG;
                    var settingSwitch = new UISwitch(new CGRect(tableView.Frame.Width - 62, 12, 50, 25));
                    settingSwitch.AddTarget((sender, args) => SettingsSwitchValueChanged(sender), UIControlEvent.ValueChanged);
                    settingSwitch.Tag = SETTINGSSWITCHTAG;
                    cell.ContentView.AddSubview(captionLabel);
                    cell.ContentView.AddSubview(settingSwitch);
                }
            }

            if (tableView == resultsTableView)
            {
                GnDataModel dataModelObject = null;
                if (_results != null && _results.Count > 0)
                {
                    dataModelObject = _results[indexPath.Row] as GnDataModel;
                }

                var albumTitleLabel = cell.ContentView.ViewWithTag(ALBUMTITLELABELTAG) as UILabel;
                var trackTitleLabel = cell.ContentView.ViewWithTag(TRACKTITLELABELTAG) as UILabel;
                var artistLabel = cell.ContentView.ViewWithTag(ARTISTLABELTAG) as UILabel;
                var trackDurationLabel = cell.ContentView.ViewWithTag(TRACKDURATIONLABELTAG) as UILabel;
                var imageView = cell.ContentView.ViewWithTag(ALBUMCOVERARTIMAGETAG) as UIImageView;

                albumTitleLabel.Text = dataModelObject.AlbumTitle;
                trackTitleLabel.Text = dataModelObject.TrackTitle;
                artistLabel.Text = dataModelObject.AlbumArtist ?? dataModelObject.TrackArtist;
                var durationText = dataModelObject.TrackDuration;
                if (String.IsNullOrEmpty(durationText) || durationText.Equals("0"))
                {
                    trackDurationLabel.Text = "";
                }
                else {
                    trackDurationLabel.Text = string.Format("Duration: {0}", durationText);
                }

                if (dataModelObject.AlbumImageData != null)
                {
                    imageView.Image = new UIImage(dataModelObject.AlbumImageData);
                }
                else {
                    imageView.Image = new UIImage("emptyImage.png");
                }

                if (_currentlySelectedIndexPath != null && indexPath.Row != _currentlySelectedIndexPath.Row)
                {
                    cell.ContentView.ViewWithTag(ADDITIONALMETADATAVIEWTAG).RemoveFromSuperview();
                    albumTitleLabel.Enabled = false;
                    trackTitleLabel.Enabled = false;
                    artistLabel.Enabled = false;
                    trackDurationLabel.Enabled = false;
                    imageView.Alpha = 0.5f;
                }
                else {
                    if (_currentlySelectedIndexPath == null)
                    {
                        cell.ContentView.BackgroundColor = UIColor.White;
                        cell.ContentView.ViewWithTag(ADDITIONALMETADATAVIEWTAG).RemoveFromSuperview();
                    }
                    else if (_currentlySelectedIndexPath != null && indexPath.Row == _currentlySelectedIndexPath.Row)
                    {
                        cell.ContentView.BackgroundColor = UIColor.White;
                        var additionalMetadataView = cell.ContentView.ViewWithTag(ADDITIONALMETADATAVIEWTAG);

                        if (additionalMetadataView == null)
                        {
                            additionalMetadataView = new UIView(new CGRect(cell.ContentView.Frame.X,
                                                                 imageView.Frame.Y + imageView.Frame.Height + 12,
                                                                 cell.ContentView.Frame.Width - 10,
                                                                            kHeightOfAdditionalMetadataCell - (artistLabel.Frame.Y + artistLabel.Frame.Height + 12)));
                            additionalMetadataView.Tag = ADDITIONALMETADATAVIEWTAG;

                            var width = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? LABELWIDTHIPAD : LABELWIDTHIPHONE;
                            var trackMatchPositionLabel = new UILabel(new CGRect(5, 5, width, 20));
                            trackMatchPositionLabel.Text = String.Format("Match pos: {0}", dataModelObject.TrackMatchPosition);
                            trackMatchPositionLabel.Font = UIFont.SystemFontOfSize(12);

                            var lookupSourceLabel = new UILabel(new CGRect(trackMatchPositionLabel.Frame.X + trackMatchPositionLabel.Frame.Width + 5, trackMatchPositionLabel.Frame.Y, width, trackMatchPositionLabel.Frame.Height));
                            lookupSourceLabel.Text = String.Format("Lookup source: {0}", _lookupSourceIsLocal ? "Local" : "Online");
                            lookupSourceLabel.Font = UIFont.SystemFontOfSize(12);

                            var currentPositionLabel = new UILabel(new CGRect(trackMatchPositionLabel.Frame.X, trackMatchPositionLabel.Frame.Y + trackMatchPositionLabel.Frame.Height + 5, width, 20));
                            currentPositionLabel.Text = string.Format("Current pos: {0}", dataModelObject.CurrentPosition);
                            currentPositionLabel.Font = UIFont.SystemFontOfSize(12);

                            var genreLabel = new UILabel(new CGRect(currentPositionLabel.Frame.X + currentPositionLabel.Frame.Width + 5, currentPositionLabel.Frame.Y, width, 20));
                            genreLabel.Text = string.Format("Genre: {0}", (dataModelObject.TrackGenre != null && dataModelObject.TrackGenre.Length > 0) ? dataModelObject.TrackGenre : dataModelObject.AlbumGenre);
                            genreLabel.Font = UIFont.SystemFontOfSize(12);

                            var diff = _queryEndTimeInterval - _queryBeginTimeInterval;

                            var timeToMatchLabel = new UILabel(new CGRect(currentPositionLabel.Frame.X, currentPositionLabel.Frame.Y + currentPositionLabel.Frame.Height + 5, width, 20));
                            timeToMatchLabel.Text = string.Format("Time to match(ms): {0}", diff);
                            timeToMatchLabel.Font = UIFont.SystemFontOfSize(12);

                            var tempoLabel = new UILabel(new CGRect(timeToMatchLabel.Frame.X, timeToMatchLabel.Frame.Bottom + 5, width, 20));
                            tempoLabel.Text = string.Format("Tempo: {0}", dataModelObject.TrackTempo);
                            tempoLabel.Font = UIFont.SystemFontOfSize(12);

                            var originLabel = new UILabel(new CGRect(timeToMatchLabel.Frame.X + timeToMatchLabel.Frame.Width + 5, timeToMatchLabel.Frame.Bottom + 5, width, 40));
                            originLabel.Text = string.Format("Origin: {0}", dataModelObject.TrackOrigin);
                            originLabel.Font = UIFont.SystemFontOfSize(12);

                            var moodLabel = new UILabel(new CGRect(tempoLabel.Frame.X, tempoLabel.Frame.Bottom + 5, width, 20));
                            moodLabel.Text = string.Format("Mood: {0}", dataModelObject.TrackMood);
                            moodLabel.Font = UIFont.SystemFontOfSize(12);

                            var biographyWidth = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 100 : 0;
                            var artistBiographyControl = new UISegmentedControl(new[] { "Artist Biography" });
                            artistBiographyControl.Frame = new CGRect(moodLabel.Frame.X + biographyWidth, moodLabel.Frame.Bottom + 10, 150, 30);
                            artistBiographyControl.Layer.MasksToBounds = true;
                            artistBiographyControl.Momentary = true;
                            artistBiographyControl.BackgroundColor = UIColor.DarkGray;
                            artistBiographyControl.TintColor = UIColor.White;
                            artistBiographyControl.Layer.CornerRadius = 5.0f;
                            artistBiographyControl.Layer.BorderColor = UIColor.Green.CGColor;
                            artistBiographyControl.AddTarget((sender, args) => ShowArtistBiography(), UIControlEvent.ValueChanged);

                            var albumReviewControl = new UISegmentedControl(new[] { "Album Review" });
                            var reviewWidth = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 150 : 10;
                            albumReviewControl.Frame = new CGRect(artistBiographyControl.Frame.X + artistBiographyControl.Frame.Width, artistBiographyControl.Frame.Y, 150, 30);
                            albumReviewControl.Layer.MasksToBounds = true;
                            albumReviewControl.Momentary = true;
                            albumReviewControl.BackgroundColor = UIColor.DarkGray;
                            albumReviewControl.TintColor = UIColor.White;
                            albumReviewControl.Layer.CornerRadius = 5.0f;
                            albumReviewControl.Layer.BorderColor = UIColor.Green.CGColor;
                            albumReviewControl.AddTarget((sender, args) => ShowAlbumReview(), UIControlEvent.ValueChanged);

                            additionalMetadataView.AddSubview(trackMatchPositionLabel);
                            additionalMetadataView.AddSubview(lookupSourceLabel);
                            additionalMetadataView.AddSubview(currentPositionLabel);
                            additionalMetadataView.AddSubview(genreLabel);
                            additionalMetadataView.AddSubview(timeToMatchLabel);
                            additionalMetadataView.AddSubview(tempoLabel);
                            additionalMetadataView.AddSubview(originLabel);
                            additionalMetadataView.AddSubview(moodLabel);
                            additionalMetadataView.AddSubview(artistBiographyControl);
                            additionalMetadataView.AddSubview(albumReviewControl);

                            cell.ContentView.AddSubview(additionalMetadataView);
                        }
                    }

                    albumTitleLabel.Enabled = true;
                    trackTitleLabel.Enabled = true;
                    artistLabel.Enabled = true;
                    trackDurationLabel.Enabled = true;
                    imageView.Alpha = 1.0f;
                }

                cell.SelectionStyle = UITableViewCellSelectionStyle.Gray;
            }
            else if (tableView == searchFieldsTableView && _currentMode == Mode.HistoryMode)
            {
                var albumTitleLabel = (UILabel)cell.ContentView.ViewWithTag(ALBUMTITLELABELTAG);
                UIImageView imageView = null;
                UILabel trackTitleLabel = null;
                UILabel artistLabel = null;
                UILabel trackMatchPositionLabel = null;

                if (albumTitleLabel == null)
                {
                    foreach (var sv in cell.ContentView.Subviews)
                    {
                        sv.RemoveFromSuperview();
                    }
                    imageView = new UIImageView(new CGRect(5, 10, 56, 56));
                    imageView.Tag = ALBUMCOVERARTIMAGETAG;

                    albumTitleLabel = new UILabel(new CGRect(70, 5, 250, 40));
                    albumTitleLabel.Font = UIFont.BoldSystemFontOfSize(16);
                    albumTitleLabel.TextColor = UIColor.FromRGBA(0.7f, 0.0f, 0.7f, 1.0f);
                    albumTitleLabel.Tag = ALBUMTITLELABELTAG;
                    albumTitleLabel.Lines = 2;
                    albumTitleLabel.LineBreakMode = UILineBreakMode.CharacterWrap;

                    trackTitleLabel = new UILabel(new CGRect(albumTitleLabel.Frame.X, albumTitleLabel.Frame.Bottom + 5, 250, albumTitleLabel.Frame.Height));
                    trackTitleLabel.Font = UIFont.SystemFontOfSize(12);
                    trackTitleLabel.TextColor = UIColor.DarkGray;
                    trackTitleLabel.Tag = TRACKTITLELABELTAG;
                    trackTitleLabel.Lines = 2;
                    trackTitleLabel.LineBreakMode = UILineBreakMode.CharacterWrap;

                    artistLabel = new UILabel(new CGRect(trackTitleLabel.Frame.X, trackTitleLabel.Frame.Bottom, 250, 40));
                    artistLabel.Font = UIFont.SystemFontOfSize(12);
                    artistLabel.TextColor = UIColor.DarkGray;
                    artistLabel.Lines = 2;
                    artistLabel.LineBreakMode = UILineBreakMode.CharacterWrap;

                    trackMatchPositionLabel = new UILabel(new CGRect(artistLabel.Frame.X, artistLabel.Frame.Bottom + 5, 250, 40));
                    trackMatchPositionLabel.Font = UIFont.BoldSystemFontOfSize(10);
                    trackMatchPositionLabel.TextColor = UIColor.Gray;
                    trackMatchPositionLabel.Tag = TRACKMATCHPOSITIONLABELTAG;
                    trackMatchPositionLabel.Lines = 2;
                    trackMatchPositionLabel.LineBreakMode = UILineBreakMode.CharacterWrap;

                    cell.ContentView.AddSubview(imageView);
                    cell.ContentView.AddSubview(albumTitleLabel);
                    cell.ContentView.AddSubview(trackTitleLabel);
                    cell.ContentView.AddSubview(artistLabel);
                    cell.ContentView.AddSubview(trackMatchPositionLabel);
                }

                imageView = (UIImageView)cell.ContentView.ViewWithTag(ALBUMCOVERARTIMAGETAG);
                trackTitleLabel = (UILabel)cell.ContentView.ViewWithTag(TRACKTITLELABELTAG);
                artistLabel = (UILabel)cell.ContentView.ViewWithTag(ARTISTLABELTAG);
                trackMatchPositionLabel = (UILabel)cell.ContentView.ViewWithTag(TRACKMATCHPOSITIONLABELTAG);

                History history = null;
                CoverArt coverArt = null;

                if (_history != null && _history.Count > 0)
                {
                    history = (History)_history[indexPath.Row];
                    coverArt = (CoverArt)history.Metadata.CoverArt;
                }

                albumTitleLabel.Text = history.Metadata.AlbumTitle;
                trackTitleLabel.Text = history.Metadata.TrackTitle;
                artistLabel.Text = history.Metadata.Artist;
                imageView.Image = new UIImage("emptyImage.png");

                if (history != null && coverArt != null && coverArt.Data != null)
                {
                    imageView.Image = new UIImage(coverArt.Data);
                }
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }
            else if (tableView == searchFieldsTableView && _currentMode == Mode.TextSearchMode)
            {
                var captionLabel = (UILabel)cell.ContentView.ViewWithTag(CAPTIONLABELTAG);
                var textField = (UITextField)cell.ContentView.ViewWithTag(TEXTFIELDTAG);
                if (textField == null)
                {
                    foreach (var sv in cell.ContentView.Subviews)
                    {
                        sv.RemoveFromSuperview();
                    }

                    captionLabel = new UILabel(new CGRect(5, 5, 100, 25));
                    captionLabel.Tag = CAPTIONLABELTAG;

                    textField = new UITextField(new CGRect(captionLabel.Frame.X + captionLabel.Frame.Width + 5, 5, cell.ContentView.Frame.Width - (captionLabel.Frame.X + captionLabel.Frame.Width + 5) - 12, 25));
                    textField.Tag = TEXTFIELDTAG;

                    cell.ContentView.AddSubview(captionLabel);
                    cell.ContentView.AddSubview(textField);
                    cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                }
                switch (indexPath.Row)
                {
                    case 0:
                        captionLabel.Text = "Artist";
                        break;
                    case 1:
                        captionLabel.Text = "Album";
                        break;
                    case 2:
                        captionLabel.Text = "Track";
                        break;
                    default:
                        break;
                }

                textField.Placeholder = captionLabel.Text;
            }
            else if (tableView == searchFieldsTableView && _currentMode == Mode.SettingsMode)
            {
                var captionLabel = (UILabel)cell.ContentView.ViewWithTag(CAPTIONLABELTAG);
                var settingsSwitch = (UISwitch)cell.ContentView.ViewWithTag(SETTINGSSWITCHTAG);

                if (settingsSwitch == null)
                {
                    foreach (var s in cell.ContentView.Subviews)
                        s.RemoveFromSuperview();
                    captionLabel = new UILabel(new CGRect(5, 5, 200, 25));
                    captionLabel.Tag = CAPTIONLABELTAG;

                    settingsSwitch = new UISwitch(new CGRect(captionLabel.Frame.X + captionLabel.Frame.Width + 5, 5, 50, 25));

                    settingsSwitch.AddTarget((sender, args) => SettingsSwitchValueChanged(sender), UIControlEvent.ValueChanged);
                    settingsSwitch.Tag = SETTINGSSWITCHTAG;
                    cell.ContentView.AddSubview(captionLabel);
                    cell.ContentView.AddSubview(settingsSwitch);
                }

                switch (indexPath.Row)
                {
                    case 0:
                        captionLabel.Text = "Debug logging";
                        settingsSwitch.On = NSUserDefaults.StandardUserDefaults.BoolForKey(DEBUGMODEKEY);
                        break;
                    case 1:
                        captionLabel.Text = "Local Search Only";
                        settingsSwitch.On = NSUserDefaults.StandardUserDefaults.BoolForKey(LOCALLOOKUPOPTIONONLY);
                        break;
                }

                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }
            return cell;
        }

        [Export("tableView:heightForRowAtIndexPath:")]
        public nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {

            if (tableView == resultsTableView && _currentlySelectedIndexPath != null && _currentlySelectedIndexPath.Row == indexPath.Row)
            {
                return (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? kHeightOfAdditionalMetadataCellPad : kHeightOfAdditionalMetadataCell;
            }

            if (tableView == resultsTableView || tableView == searchFieldsTableView && _currentMode == Mode.HistoryMode)
            {
                return 100.0f;
            }

            return 45;
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        public virtual void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {

            if (_currentlySelectedIndexPath == null)
            {
                _currentlySelectedIndexPath = indexPath;
                var cell = GetCell(tableView, indexPath);
            }
            else {
                var cell = GetCell(tableView, _currentlySelectedIndexPath);
                _currentlySelectedIndexPath = null;

                var additionalContentView = (UIView)cell.ContentView.ViewWithTag(ADDITIONALCONTENTVIEWTAG);
                var frame = additionalContentView.Frame;
                frame.X = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 768 : 320;
                additionalContentView.Frame = frame;
            }
            tableView.DeselectRow(indexPath, false);
            tableView.BeginUpdates();
            tableView.ReloadData();

            Task.Delay(100).ContinueWith(t =>
            {
                ScrollPlaybackViewToVisibleRect();
                tableView.EndUpdates();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        [Export("tableView:numberOfRowsInSection:")]
        public nint RowsInSection(UITableView tableview, nint section)
        {
            if (tableview == resultsTableView)
            {
                if (_results.Count == 0)
                    tableview.SeparatorColor = UIColor.Clear;
                else
                    tableview.SeparatorColor = UIColor.LightGray;
                return _results.Count;
            }
            else if (tableview == searchFieldsTableView && _currentMode == Mode.TextSearchMode)
            {
                return 3;
            }
            else if (tableview == searchFieldsTableView && _currentMode == Mode.SettingsMode)
            {
                return 2;
            }
            else if (tableview == searchFieldsTableView && _currentMode == Mode.HistoryMode && _history != null)
            {
                return _history.Count;
            }
            return 0;
        }

        void PositionArrowForItem(UIButton item, bool isUp)
        {
            var itemFrame = item.Frame;
            itemFrame = item.Superview.ConvertRectToView(itemFrame, View);

            var arrowFrame = arrowImageView.Frame;
            arrowFrame.X = itemFrame.GetMidX() - arrowFrame.Width / 2;
            arrowFrame.Y = isUp ? itemFrame.Y + itemFrame.Height : itemFrame.Y - arrowFrame.Height;
            arrowImageView.Frame = arrowFrame;

            if (isUp)
            {
                arrowImageView.Image = new UIImage("upArrow.png");
                var textSearchFrame = textSearchView.Frame;
                textSearchFrame.Y = arrowImageView.Frame.Y + arrowImageView.Frame.Height - 5;
                textSearchView.Frame = textSearchFrame;
            }
            else {
                arrowImageView.Image = new UIImage("downArrow.png");
                var textSearchFrame = textSearchView.Frame;
                textSearchFrame.Y = arrowImageView.Frame.Y - textSearchFrame.Height;
                textSearchView.Frame = textSearchFrame;
            }
        }
        #endregion

        #region IGnLookupLocalStreamIngestEventsDelegate implementation

        public void StatusEvent(GnLookupLocalStreamIngestStatus status, string bundleId, IGnCancellableDelegate canceller)
        {
            Console.WriteLine("status = {0}", status);
        }
        #endregion

        #region IGnMusicIdStreamEventsDelegate implementation

        [Export("musicIdStreamProcessingStatusEvent:cancellableDelegate:")]
        void MusicIdStreamProcessingStatusEvent(GnMusicIdStreamProcessingStatus status, GnCancellableDelegate canceller)
        {
            switch (status)
            {
                case GnMusicIdStreamProcessingStatus.kStatusProcessingAudioStarted:
                    InvokeOnMainThread(() =>
                    {
                        _audioProcessingStarted = true;
                        idNowButton.Enabled = true;
                    });
                    break;
            }
        }

        [Export("musicIdStreamIdentifyingStatusEvent:cancellableDelegate:")]
        void MusicIdStreamIdentifyingStatusEvent(GnMusicIdStreamIdentifyingStatus status, GnCancellableDelegate canceller)
        {
            string statusString = null;
            switch (status)
            {
                case GnMusicIdStreamIdentifyingStatus.kStatusIdentifyingInvalid:
                    statusString = "Error";
                    break;
                case GnMusicIdStreamIdentifyingStatus.kStatusIdentifyingStarted:
                    statusString = "Identyfying";
                    break;
                case GnMusicIdStreamIdentifyingStatus.kStatusIdentifyingFpGenerated:
                    statusString = "Fingerprint Generated";
                    break;
                case GnMusicIdStreamIdentifyingStatus.kStatusIdentifyingLocalQueryStarted:
                    statusString = "Local Query Started";
                    _lookupSourceIsLocal = true;
                    _queryBeginTimeInterval = DateTime.Now.Ticks;
                    break;
                case GnMusicIdStreamIdentifyingStatus.kStatusIdentifyingOnlineQueryStarted:
                    statusString = "Online Query Started";
                    _lookupSourceIsLocal = false;
                    break;
                case GnMusicIdStreamIdentifyingStatus.kStatusIdentifyingLocalQueryEnded:
                    statusString = "Local Query Ended";
                    _lookupSourceIsLocal = true;
                    _queryEndTimeInterval = DateTime.Now.Ticks;
                    break;
                case GnMusicIdStreamIdentifyingStatus.kStatusIdentifyingOnlineQueryEnded:
                    statusString = "Online Query Ended";
                    _queryEndTimeInterval = DateTime.Now.Ticks;
                    break;
                case GnMusicIdStreamIdentifyingStatus.kStatusIdentifyingEnded:
                    statusString = "Identification Ended";
                    break;
            }

            if (statusString != null)
            {
                UpdateStatus(statusString);
            }
        }

        [Export("musicIdStreamAlbumResult:cancellableDelegate:")]
        void MusicIdStreamAlbumResult(GnResponseAlbums result, GnCancellableDelegate canceller)
        {
            _cancellableObjects.Remove(gnMusicIdStream);

            if (_cancellableObjects.Count == 0)
            {
                cancelOperationsButton.Enabled = false;
            }

            StopBusyIndicator();
            ProcessAlbumResponseAndUpdateResultsTable(result);
        }

        [Export("musicIdStreamIdentifyCompletedWithError:")]
        void MusicIdStreamIdentifyCompletedWithError(NSError completeError)
        {
            _cancellableObjects.Remove(gnMusicIdStream);

            if (_cancellableObjects.Count == 0)
            {
                cancelOperationsButton.Enabled = false;
            }

            UpdateStatus(completeError.ToString());
            StopBusyIndicator();
        }
        #endregion

        #region event handlers

        void ShowVisualization(object sender, EventArgs e)
        {

            var visualizationFrame = visualizationView.Frame;

            if (!_visualizationIsVisible)
            {
                visualizationFrame.Y += visualizationFrame.Height - (showOrHideVisualizationButtonView.Frame.Height + 10);
            }
            else {
                visualizationFrame.Y -= visualizationFrame.Height - (showOrHideVisualizationButtonView.Frame.Height + 10);
            }

            UIView.Animate(0.5, () =>
            {
                visualizationView.Frame = visualizationFrame;
            }, () =>
            {
                _visualizationIsVisible = !_visualizationIsVisible;
                showOrHideVisualizationButton.TitleLabel.Text = _visualizationIsVisible ? "Close" : "Show Visualization";

                UIView.Animate(0.5, () =>
                 {
                     if (_visualizationIsVisible)
                     {
                         var spinBehavior = new UIDynamicItemBehavior(new[] { gracenoteLogoImageView });
                         spinBehavior.AddAngularVelocityForItem(5.0f, gracenoteLogoImageView);
                         spinBehavior.AngularResistance = 0;
                     }
                     else {
                         float scale = 1.0f;
                         var sscale = CGAffineTransform.MakeScale(scale, scale);
                         coloredRingImageView.Transform = sscale;
                         _dynamicAnimator.RemoveAllBehaviors();
                         var rotTransform = CATransform3D.MakeRotation(0, 0, 0, 1);
                         gracenoteLogoImageView.Layer.Transform = rotTransform;
                     }
                 });
            });
        }


        void IdNow(object sender, EventArgs e)
        {
            if (gnMusicIdStream != null)
            {
                cancelOperationsButton.Enabled = true;
                EnableOrDisableControls(false);
                _results.Clear();

                _currentlySelectedIndexPath = null;

                NSError error = null;

                _cancellableObjects.Add(gnMusicIdStream);
                gnMusicIdStream.IdentifyAlbumAsync(out error);
                UpdateStatus("Identifying");
                busyIndicator.StartAnimating();

                if (error != null)
                {
                    Console.WriteLine("Identify error - {0}", error.LocalizedDescription);
                    _queryBeginTimeInterval = -1;
                }
                else {
                    _queryBeginTimeInterval = DateTime.Now.Ticks;
                }
            }
        }

        void CancelAllOperations(object sender, EventArgs e)
        {
            foreach (var obj in _cancellableObjects)
            {
                var o = obj as GnMusicIdStream;
                if (o != null)
                {
                    NSError error = null;
                    o.IdentifyCancel(out error);
                    if (error != null)
                    {
                        Console.WriteLine("MusicIDStream Cancel Error  - {0}", error.LocalizedDescription);
                    }
                    var o1 = obj as GnMusicIdFile;
                    if (o1 != null)
                        o1.Cancel();
                    //        else
                    //        {
                    //            [obj setCancel:YES];
                    //        }
                }
            }
            StopBusyIndicator();
        }

        void DoAlbumID(object sender, EventArgs e)
        {
            AlbumIdButtonTapped(sender);
        }

        void DoRecognizeMedia(object sender, EventArgs e)
        {
            ActionButtonTapped(sender);
        }

        void ShowDebugConsole(object sender, EventArgs e)
        {
            if (_currentMode != Mode.DebugMode)
            {
                var frame = debugView.Frame;
                frame.Y = UIScreen.MainScreen.Bounds.Height + 20;
                debugView.Frame = frame;
                debugView.Alpha = 1.0f;
                _currentMode = Mode.DebugMode;

                UIView.Animate(0.5f, () =>
                {
                    frame = debugView.Frame;
                    frame.Y = (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) ? 120 : 80;
                    debugView.Frame = frame;
                }, () =>
                {
                    _enableDebugRefreshTimer = true;
                });
            }
            else {
                UIView.Animate(0.5f, () =>
                {
                    var frame = debugView.Frame;
                    frame.Y = UIScreen.MainScreen.Bounds.Height + 20;
                    debugView.Frame = frame;
                }, () =>
                {
                    debugView.Alpha = 0.0f;
                    _currentMode = Mode.UnknownMode;
                    _enableDebugRefreshTimer = false;
                });
            }
        }


        void ShowHistory(object sender, EventArgs e)
        {
            if (_currentMode == Mode.HistoryMode)
            {
                CloseSearchTextView();
                _currentMode = Mode.UnknownMode;
                return;
            }

            _currentMode = Mode.HistoryMode;
            NSError error = null;
            var context = AppDelegate.SharedContext;
            var fetchRequest = new NSFetchRequest();
            var entity = NSEntityDescription.EntityForName("History", context);
            fetchRequest.Entity = entity;

            var dateSortDescriptor = new NSSortDescriptor("current_date", false);
            fetchRequest.SortDescriptors = new[] { dateSortDescriptor };
            var fetchedObjects = context.ExecuteFetchRequest(fetchRequest, out error);
            var list = new List<object>(fetchedObjects);
            _history = list;
            PositionArrowForItem(sender as UIButton, false);
            _searchSegmentedControl.Hidden = true;
            _cancelSegmentedControl.Hidden = true;
            searchFieldsTableView.ReloadData();
            searchFieldsTableView.Bounces = true;
            searchFieldsTableView.AlwaysBounceVertical = true;
            searchFieldsTableView.ScrollEnabled = true;
            var searchFieldsTableViewFrame = searchFieldsTableView.Frame;
            searchFieldsTableViewFrame.Height = textSearchView.Frame.Height - searchFieldsTableViewFrame.Y * 2;
            searchFieldsTableView.Frame = searchFieldsTableViewFrame;

            ((UIButton)sender).TintColor = UIColor.Blue;
            UIView.Animate(0.5, () =>
            {
                textSearchView.Alpha = 1.0f;
                arrowImageView.Alpha = 1.0f;
            }, null);
        }
        #endregion
    }
}

