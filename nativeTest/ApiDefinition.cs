using System;

using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;
using CoreData;

namespace nativeTest
{
    // The first step to creating a binding is to add your native library ("libNativeLibrary.a")
    // to the project by right-clicking (or Control-clicking) the folder containing this source
    // file and clicking "Add files..." and then simply select the native library (or libraries)
    // that you want to bind.
    //
    // When you do that, you'll notice that MonoDevelop generates a code-behind file for each
    // native library which will contain a [LinkWith] attribute. MonoDevelop auto-detects the
    // architectures that the native library supports and fills in that information for you,
    // however, it cannot auto-detect any Frameworks or other system libraries that the
    // native library may depend on, so you'll need to fill in that information yourself.
    //
    // Once you've done that, you're ready to move on to binding the API...
    //
    //
    // Here is where you'd define your API definition for the native Objective-C library.
    //
    // For example, to bind the following Objective-C class:
    //
    //     @interface Widget : NSObject {
    //     }
    //
    // The C# binding would look like this:
    //
    //     [BaseType (typeof (NSObject))]
    //     interface Widget {
    //     }
    //
    // To bind Objective-C properties, such as:
    //
    //     @property (nonatomic, readwrite, assign) CGPoint center;
    //
    // You would add a property definition in the C# interface like so:
    //
    //     [Export ("center")]
    //     CGPoint Center { get; set; }
    //
    // To bind an Objective-C method, such as:
    //
    //     -(void) doSomething:(NSObject *)object atIndex:(NSInteger)index;
    //
    // You would add a method definition to the C# interface like so:
    //
    //     [Export ("doSomething:atIndex:")]
    //     void DoSomething (NSObject object, int index);
    //
    // Objective-C "constructors" such as:
    //
    //     -(id)initWithElmo:(ElmoMuppet *)elmo;
    //
    // Can be bound as:
    //
    //     [Export ("initWithElmo:")]
    //     IntPtr Constructor (ElmoMuppet elmo);
    //
    // For more information, see http://developer.xamarin.com/guides/ios/advanced_topics/binding_objective-c/
    //

    [BaseType(typeof(NSObject))]
    interface GnManager
    {

        [Export("initWithLicense:")]
        IntPtr Constructor(string license);

        [Export("initWithLicense:licenseInputMode:")]
        IntPtr Constructor(string license, GnLicenseInputMode licenseInputMode);

        //+(NSString*) productVersion;
        [Static, Export("productVersion")]
        string ProductVersion { get; }
    }

    [BaseType(typeof(NSObject))]
    [Model]
    [Protocol]
    interface GnUserStoreDelegate
    {
        [Abstract]
        [Export("loadSerializedUser:")]
        string loadSerializedUser(string clientId);


        [Export("storeSerializedUser:")]
        bool storeSerializedUser(string clientId);

        [Export("storeSerializedUser:userData:")]
        bool storeSerializedUser(string clientId, string userData);
    }

    interface IGnUserStoreDelegate { }

    [BaseType(typeof(NSObject))]
    [Model]
    [Protocol]
    interface GnStatusEventsDelegate
    {
        [Abstract]
        [Export("statusEvent:percentComplete:bytesTotalSent:bytesTotalReceived:cancellableDelegate:")]
        void StatusEvent(GnStatus status, int percentComplete, int bytesTotalSent, int bytesTotalReceived, GnCancellableDelegate canceller);
    }


    interface IGnStatusEventsDelegate { }

    [BaseType(typeof(NSObject))]
    [Model]
    [Protocol]
    interface GnCancellableDelegate
    {
        [Abstract]
        [Export("setCancel:")]
        void SetCancel(bool bCancel);
    }

    interface IGnCancellableDelegate { }

    [BaseType(typeof(NSObject))]
    interface GnUserStore : GnUserStoreDelegate
    {
    }


    [BaseType(typeof(NSObject))]
    interface GnUser
    {

        [Export("initWithSerializedUser:")]
        IntPtr Constructor(string serializedUser);


        [Export("initWithSerializedUser:clientIdTest:")]
        IntPtr Constructor(string serializedUser, string clientIdTest);


        [Export("initWithGnUserStoreDelegate:")]
        IntPtr Constructor(IGnUserStoreDelegate userStore);

        [Export("initWithGnUserStoreDelegate:clientId:clientTag:applicationVersion:")]
        IntPtr Constructor(IGnUserStoreDelegate userStore, string clientId, string clientTag, string applicationVersion);
    }

    [BaseType(typeof(NSObject))]
    interface GnLocalInfo
    {

        [Export("initWithGnLocaleGroup:")]
        IntPtr Constructor(GnLocaleGroup group);

        [Export("initWithGnLocaleGroup:language:region:descriptor:")]
        IntPtr Constructor(GnLocaleGroup group, GnLanguage language, GnRegion region, GnDescriptor descriptor);
    }

    [BaseType(typeof(NSObject))]
    interface GnLocale
    {

        [Export("initWithGnLocaleGroup:language:region:descriptor:user:statusEventsDelegate:")]
        IntPtr Constructor(GnLocaleGroup group, GnLanguage language, GnRegion region, GnDescriptor descriptor, GnUser user, [NullAllowed] GnStatusEventsDelegate pEventHandler);


        [Export("revision:")]
        string Revision(out NSError error);


        [Export("setGroupDefault:")]
        void SetGroupDefault([NullAllowed] out NSError error);


        [Export("update:statusEventsDelegate:error:")]
        bool Update(GnUser user, GnStatusEventsDelegate pEventHandler, out NSError error);

        [Export("updateCheck:statusEventsDelegate:error:")]
        bool UpdateCheck(GnUser user, GnStatusEventsDelegate pEventHandler, out NSError error);

        [Export("serialize:")]
        string Serialize(out NSError error);
    }

    [BaseType(typeof(NSObject))]
    [Model]
    [Protocol]
    interface GnAudioSourceDelegate
    {
        [Export("getData:dataSize:")]
        string GetData(IntPtr dataBuffer, int dataSize);
    }

    interface IGnAudioSourceDelegate { }

    [BaseType(typeof(NSObject))]
    interface GnDataObject
    {
    }

    [BaseType(typeof(GnDataObject))]
    interface GnResponseDataMatches
    {
    }

    [BaseType(typeof(GnDataObject))]
    interface GnResponseAlbums
    {
        [Export("from:error:")]
        GnResponseAlbums From(GnDataObject obj, out NSError error);
    }

    [BaseType(typeof(NSObject))]
    interface GnMusicId
    {

        [Export("initWithGnUser:statusEventsDelegate:")]
        IntPtr Constructor(GnUser user, IGnStatusEventsDelegate pEventHandler);


        [Export("fingerprintDataGet:")]
        string FingerprintDataGet(out NSError error);

        [Export("fingerprintBegin:audioSampleRate:audioSampleSize:audioChannels:error:")]
        void FingerprintBegin(GnFingerprintType fpType, int audioSampleRate, int audioSampleSize, int audioChannels, out NSError error);

        [Export("fingerprintWrite:audioDataSize:error:")]
        bool FingerprintWrite(IntPtr audioData, int audioDataSize, out NSError error);

        [Export("fingerprintEnd:")]
        void FingerprintEnd(out NSError error);

        [Export("fingerprintFromSource:fpType:error:")]
        void FingerprintFromSource(GnAudioSourceDelegate audioSource, GnFingerprintType fpType, out NSError error);

        [Export("findAlbumsWithAlbumTitle:trackTitle:albumArtistName:trackArtistName:composerName:error:")]
        GnResponseAlbums FindAlbumsWithAlbumTitle(string albumTitle, string trackTitle, string albumArtistName, string trackArtistName, string composeName, out NSError error);

        [Export("findAlbumsWithCDTOC:error:")]
        GnResponseAlbums FindAlbumsWithCDTOC(string CDTOC, out NSError error);

        [Export("findAlbumsWithCDTOC:strFingerprintData:fpType:error:")]
        GnResponseAlbums FindAlbumsWithCDTOC(string CDTOC, string strFingerprintData, GnFingerprintType fpType, out NSError error);

        [Export("findAlbumsWithFingerprintData:fpType:error:")]
        GnResponseAlbums FindAlbumsWithFingerprintData(string fingerprintData, GnFingerprintType fpType, out NSError error);

        [Export("findAlbumsWithGnDataObject:error:")]
        GnResponseAlbums FindAlbumsWithGnDataObject(GnDataObject gnDataObject, out NSError error);

        [Export("findAlbumsWithAudioSource:fpType:error:")]
        GnResponseAlbums FindAlbumsWithAudioSource(GnAudioSourceDelegate audioSource, GnFingerprintType fpType, out NSError error);

        [Export("findMatches:trackTitle:albumArtistName:trackArtistName:composerName:error:")]
        GnResponseDataMatches FindMatches(string albumTitle, string trackTitle, string albumArtistName, string trackArtistName, string composerName, out NSError error);

        [Export("setCancel:")]
        void SetCancel(bool bCancel);

        [Export("options")]
        GnMusicIdOptions Options { get; }
    }

    [BaseType(typeof(NSObject))]
    interface GnAssetFetch
    {

        [Export("initWithGnUser:url:statusEventsDelegate:")]
        IntPtr Constructor(GnUser user, string url, GnStatusEventsDelegate pEventHandler);

        //[Field ("size", "GnSDKObjC")]
        [Export("size")]
        int Size { get; }
    }
    [BaseType(typeof(NSObject))]
    [Model]
    [Protocol]
    interface GnMusicIdStreamEventsDelegate : GnStatusEventsDelegate
    {

        //-(void) musicIdStreamProcessingStatusEvent: (GnMusicIdStreamProcessingStatus)status cancellableDelegate: (id<GnCancellableDelegate>)canceller;
        [Export("musicIdStreamProcessingStatusEvent:cancellableDelegate:")]
        void MusicIdStreamProcessingStatusEvent(GnMusicIdStreamProcessingStatus status, GnCancellableDelegate canceller);

        //-(void) musicIdStreamIdentifyingStatusEvent: (GnMusicIdStreamIdentifyingStatus)status cancellableDelegate: (id<GnCancellableDelegate>)canceller;
        [Export("musicIdStreamIdentifyingStatusEvent:cancellableDelegate:")]
        void MusicIdStreamIdentifyingStatusEvent(GnMusicIdStreamIdentifyingStatus status, GnCancellableDelegate canceller);

        //-(void) musicIdStreamAlbumResult: (GnResponseAlbums*)result cancellableDelegate: (id<GnCancellableDelegate>)canceller;
        [Export("musicIdStreamAlbumResult:cancellableDelegate:")]
        void MusicIdStreamAlbumResult(GnResponseAlbums result, GnCancellableDelegate canceller);

        //-(void) musicIdStreamIdentifyCompletedWithError: (NSError*)completeError;
        [Export("musicIdStreamIdentifyCompletedWithError:")]
        void MusicIdStreamIdentifyCompletedWithError(NSError completeError);
    }

    interface IGnMusicIdStreamEventsDelegate { }



    [BaseType(typeof(NSObject))]
    interface GnMusicIdStream
    {
        //-(INSTANCE_RETURN_TYPE) initWithGnUser: (GnUser*)user preset: (GnMusicIdStreamPreset)preset locale: (GnLocale*)locale musicIdStreamEventsDelegate: (id<GnMusicIdStreamEventsDelegate>)pEventDelegate;
        [Export("initWithGnUser:preset:locale:musicIdStreamEventsDelegate:")]
        IntPtr Constructor(GnUser user, GnMusicIdStreamPreset preset, GnLocale locale, IGnMusicIdStreamEventsDelegate pEventDelegate);

        //      -(INSTANCE_RETURN_TYPE) initWithGnUser: (GnUser*)user preset: (GnMusicIdStreamPreset)preset musicIdStreamEventsDelegate: (id<GnMusicIdStreamEventsDelegate>)pEventDelegate;
        [Export("initWithGnUser:preset:musicIdStreamEventsDelegate:")]
        IntPtr Constructor(GnUser user, GnMusicIdStreamPreset preset, GnMusicIdStreamEventsDelegate pEventDelegate);

        //-(void) audioProcessStartWithAudioSource: (id<GnAudioSourceDelegate>)audioSource error: (NSError**)error;
        [Export("audioProcessStartWithAudioSource:error:")]
        void AudioProcessStartWithAudioSource(GnAudioSourceDelegate audioSource, out NSError error);

        //-(void) audioProcessStartWithSamplesPerSecond: (NSUInteger)samplesPerSecond bitsPerSample: (NSUInteger)bitsPerSample numberOfChannels: (NSUInteger)numberOfChannels error: (NSError**)error;
        [Export("audioProcessStartWithSamplesPerSecond:bitsPerSample:numberOfChannels:error:")]
        void AudioProcessStartWithSamplesPerSecond(nuint samplesPerSecond, nuint bitsPerSample, nuint numberOfChannels, out NSError error);

        //-(void) audioProcessStop:(NSError**) error;
        [Export("audioProcessStop:")]
        void AudioProcessStop(out NSError error);

        //-(void) audioProcess: (unsigned char*)audioData audioDataLength: (NSUInteger)audioDataLength error: (NSError**)error;
        [Export("audioProcess:audioDataLength:error:")]
        void AudioProcess([PlainString] string audioData, nuint audioDataLength, out NSError error);

        //-(void) identifyAlbum:(NSError**) error;
        [Export("identifyAlbum:")]
        void IdentifyAlbum(out NSError error);

        //-(void) identifyAlbumAsync:(NSError**) error;
        [Export("identifyAlbumAsync:")]
        void IdentifyAlbumAsync(out NSError error);

        //-(BOOL) waitForIdentify: (NSUInteger)timeout_ms error: (NSError**)error;
        [Export("waitForIdentify:error:")]
        bool WaitForIdentify(nint timeout_ms, out NSError error);

        //-(void) identifyCancel:(NSError**) error;
        [Export("identifyCancel:")]
        void IdentifyCancel(out NSError error);

        //-(void) broadcastMetadata: (NSString*)broadcastMetadataKey broadcastMetadataValue: (NSString*)broadcastMetadataValue error: (NSError**)error;
        [Export("broadcastMetadata:broadcastMetadataValue:error:")]
        void BroadcastMetadata(string broadcastMetadataKey, string broadcastMetadataValue, out NSError error);


        //-(GnMusicIdStreamOptions*) options;
        [Export("options")]
        GnMusicIdStreamOptions Options { get; }
    }


    [BaseType(typeof(NSObject))]
    interface GnMusicIdStreamOptions
    {
        //-(void) lookupMode: (GnLookupMode)lookupMode error: (NSError**)error;
        [Export("lookupMode:error:")]
        void LookupMode(GnLookupMode lookupMode, out NSError error);

        //-(void) lookupData: (GnLookupData)val enable: (BOOL)enable error: (NSError**)error;
        [Export("lookupData:enable:error:")]
        void LookupData(GnLookupData val, bool enable, out NSError error);

        //-(void) preferResultLanguage: (GnLanguage)preferredLanguage error: (NSError**)error;
        [Export("preferResultLanguage:error:")]
        void PreferResultLanguage(GnLanguage preferredLanguage, out NSError error);

        //-(void) preferResultExternalId: (NSString*)preferredExternalId error: (NSError**)error;
        [Export("preferResultExternalId:error:")]
        void PreferResultExternalId(string preferredExternalId, out NSError error);

        //-(void) preferResultCoverart: (BOOL)bEnable error: (NSError**)error;
        [Export("preferResultCoverart:error:")]
        void PreferResultCoverart(bool enable, out NSError error);

        //-(void) resultSingle: (BOOL)bEnable error: (NSError**)error;
        [Export("resultSingle:error:")]
        void ResultSingle(bool enable, out NSError error);

        //-(void) resultRangeStart: (NSUInteger)resultStart error: (NSError**)error;
        [Export("resultRangeStart:error:")]
        void ResultRangeStart(nint resultStart, out NSError error);

        //-(void) resultCount: (NSUInteger)resultCount error: (NSError**)error;
        [Export("resultCount:error:")]
        void ResultCount(nint resultCount, out NSError error);

        //-(void) networkInterfaceWithIntfName: (NSString*)intfName error: (NSError**)error;
        [Export("networkInterfaceWithIntfName:error:")]
        void NetworkInterfaceWithIntfName(string intfName, out NSError error);

        //-(NSString*) networkInterface:(NSError**) error;
        [Export("networkInterface:")]
        string NetworkInterface(out NSError error);

        //-(void) custom: (NSString*)optionKey value: (NSString*)value error: (NSError**)error;
        [Export("custom:value:error:")]
        string NetworkInterface(string optionKey, string value, out NSError error);

    }



    [BaseType(typeof(NSObject))]
    interface GnAudioVisualizeAdapter : GnAudioSourceDelegate
    {

        //-(instancetype) initWithAudioSource:(id<GnAudioSourceDelegate>) audioSource audioVisualizerDelegate:(id<GnAudioVisualizerDelegate> )audioVisualizerDelegate;
        [Export("initWithAudioSource:audioVisualizerDelegate:")]
        IntPtr Constructor(GnAudioSourceDelegate audioSource, GnAudioVisualizerDelegate audioVisualizerDelegate);

    }


    [BaseType(typeof(NSObject))]
    [Model]
    [Protocol]
    interface GnAudioVisualizerDelegate
    {
        //-(void) RMSDidUpdateByValue:(float) value;
        [Export("RMSDidUpdateByValue:")]
        void RMSDidUpdateByValue(float value);

    }


    [BaseType(typeof(NSObject))]
    interface GnMusicIdOptions
    {


        //-(void) lookupMode: (GnLookupMode)lookupMode error: (NSError**)error;
        [Export("lookupMode:error:")]
        void LookupMode(GnLookupMode lookupMode, out NSError error);

        //-(void) lookupData: (GnLookupData)lookupData bEnable: (BOOL)bEnable error: (NSError**)error;
        [Export("lookupData:bEnable:error:")]
        void LookupData(GnLookupData lookupData, bool bEnable, out NSError error);

        //-(void) preferResultLanguage: (GnLanguage)preferredLanguage error: (NSError**)error;
        [Export("preferResultLanguage:error:")]
        void PreferResultLanguage(GnLanguage preferredLanguage, out NSError error);

        //-(void) preferResultExternalId: (NSString*)strExternalId error: (NSError**)error;
        [Export("preferResultExternalId:error:")]
        void PreferResultExternalId(string strExternalId, out NSError error);

        //-(void) preferResultCoverart: (BOOL)bEnable error: (NSError**)error;
        [Export("preferResultCoverart:error:")]
        void PreferResultCoverart(bool bEnable, out NSError error);

        //-(void) resultSingle: (BOOL)bEnable error: (NSError**)error;
        [Export("resultSingle:error:")]
        void ResultSingle(bool bEnable, out NSError error);

        //-(void) revisionCheck: (BOOL)bEnable error: (NSError**)error;
        [Export("revisionCheck:error:")]
        void RevisionCheck(bool bEnable, out NSError error);

        //-(void) resultRangeStart: (NSUInteger)resultStart error: (NSError**)error;
        [Export("resultRangeStart:error:")]
        void ResultRangeStart(nuint resultStart, out NSError error);

        //-(void) resultCount: (NSUInteger)resultCount error: (NSError**)error;
        [Export("resultCount:error:")]
        void ResultCount(nuint resultCount, out NSError error);

        //-(void) networkInterfaceWithIntfName: (NSString*)intfName error: (NSError**)error;
        [Export("networkInterfaceWithIntfName:error:")]
        void NetworkInterfaceWithIntfName(string intfName, out NSError error);

        //-(NSString*) networkInterface:(NSError**) error;
        [Export("networkInterface:")]
        string NetworkInterface(out NSError error);

        //-(void) customWithOption: (NSString*)option value: (NSString*)value error: (NSError**)error;
        [Export("customWithOption:value:error:")]
        void CustomWithOption(string option, string value, out NSError error);

        //-(void) customWithOption: (NSString*)option bEnable: (BOOL)bEnable error: (NSError**)error;
        [Export("customWithOption:bEnable:error:")]
        void CustomWithOption(string option, bool bEnable, out NSError error);
    }

    [BaseType(typeof(NSEnumerator), Name = "GnAlbumEnumerator")]
    interface GnAlbumEnumerator
    {

        //[Export ("enumerateObjectsUsingBlock:")]
        //void EnumerateObjectsUsingBlock (GnAlbumBlock handler);

        [Export("count")]
        nuint Count { get; }

        [Export("nextObject")]
        GnAlbum NextObject { get; }

        [Export("objectAtIndex:")]
        GnAlbum RowAtIndex(nuint index);

        [Export("allObjects")]
        NSArray AllObjects { get; }
    }

    [BaseType(typeof(NSObject))]
    interface GnAlbum
    {
        //	-(INSTANCE_RETURN_TYPE) initWithId: (NSString*)id idTag: (NSString*)idTag;
        [Export("initWithId:idTag:")]
        IntPtr Constructor(string id, string idTag);

        //-(BOOL) isFullResult;
        [Export("isFullResult")]
        bool IsFullResult { get; }

        //-(GnTitle*) title;
        [Export("title")]
        GnTitle Title { get; }

        //-(GnArtist*) artist;
        [Export("artist")]
        GnArtist Artist { get; }

        //-(NSString*) genre: (GnDataLevel)level;
        [Export("genre:")]
        string Genre(GnDataLevel level);

        //-(NSString*) label;
        [Export("label")]
        string Label { get; }

        //-(NSString*) language;
        [Export("language")]
        string Language { get; }


        //-(NSString*) languageCode;
        [Export("languageCode")]
        string LanguageCode { get; }

        //-(NSString*) tui;
        [Export("tui")]
        string Tui { get; }

        //-(NSString*) tuiTag;
        [Export("tuiTag")]
        string TuiTag { get; }

        //-(NSString*) tagId;
        [Export("tagId")]
        string TagId { get; }

        //-(NSString*) gnId;
        [Export("gnId")]
        string GnId { get; }

        //-(NSString*) gnUId;
        [Export("gnUId")]
        string GnUId { get; }


        //-(NSString*) globalId;
        [Export("globalId")]
        string GlobalId { get; }

        //-(NSUInteger) discInSet;
        [Export("discInSet")]
        nuint DiscInSet { get; }

        //-(NSUInteger) totalInSet;
        [Export("totalInSet")]
        nuint TotalInSet { get; }

        //-(NSString*) year;
        [Export("year")]
        string Year { get; }

        //-(BOOL) isClassical;
        [Export("isClassical")]
        bool IsClassical { get; }

        //-(NSUInteger) trackCount;
        [Export("trackCount")]
        nuint TrackCount { get; }

        //-(NSString*) compilation;
        [Export("compilation")]
        string Compilation { get; }

        //-(NSUInteger) matchScore;
        [Export("matchScore")]
        nuint MatchScore { get; }

        //-(GnTrack*) track: (NSUInteger)trackNumber;
        [Export("track:")]
        GnTrack Track(nuint trackNumber);

        //-(GnTrack*) trackMatched: (NSUInteger)ordinal;
        [Export("trackMatched:")]
        GnTrack TrackMatched(nuint ordinal);

        //-(NSUInteger) trackMatchNumber: (NSUInteger)ordinal;
        [Export("trackMatchNumber:")]
        nuint trackMatchNumber(nuint ordinal);

        //-(GnContent*) content: (GnContentType)contentType;
        [Export("content:")]
        GnContent Content(GnContentType contentType);

        //-(GnContent*) coverArt;
        [Export("coverArt")]
        GnContent CoverArt { get; }

        //-(GnContent*) review;
        [Export("review")]
        GnContent Review { get; }

        //-(GnTitle*) titleClassical;
        [Export("titleClassical")]
        GnTitle TitleClassical { get; }

        //-(GnTitle*) titleRegional;
        [Export("titleRegional")]
        GnTitle TitleRegional { get; }

        //-(GnTitle*) titleRegionalLocale;
        [Export("titleRegionalLocale")]
        GnTitle TitleRegionalLocale { get; }

        //-(NSString*) script;
        [Export("script")]
        string Script { get; }

        //+(NSString*) gnType;
        [Export("gnType")]
        string GnType { get; }
    }

    [BaseType(typeof(GnDataObject))]
    interface GnTrack
    {
        //	-(INSTANCE_RETURN_TYPE) initWithId: (NSString*)id idTag: (NSString*)idTag;
        [Export("initWithId:idTag:")]
        IntPtr Constructor(string id, string idTag);

        //-(BOOL) isFullResult;
        [Export("isFullResult")]
        bool IsFullResult { get; }

        //-(GnTitle*) title;
        [Export("title")]
        GnTitle Title { get; }

        //-(GnArtist*) artist;
        [Export("artist")]
        GnArtist Artist { get; }


        //-(NSString*) mood: (GnDataLevel)level;
        [Export("mood:")]
        string Mood(GnDataLevel level);

        //-(NSString*) tempo: (GnDataLevel)level;
        [Export("tempo:")]
        string Tempo(GnDataLevel level);

        //-(NSString*) genre: (GnDataLevel)level;
        [Export("genre:")]
        string Genre(GnDataLevel level);

        //-(GnContent*) content: (GnContentType)contentType;genre
        [Export("content:")]
        GnContent Content(GnContentType contentType);

        //-(GnContent*) review;
        [Export("review")]
        GnContent Review { get; }

        //-(BOOL) matched;
        [Export("matched")]
        bool Matched { get; }

        //-(NSUInteger) matchPosition;
        [Export("matchPosition")]
        nuint MatchPosition { get; }

        //-(NSUInteger) matchDuration;
        [Export("matchDuration")]
        nuint MatchDuration { get; }

        //-(NSUInteger) currentPosition;
        [Export("currentPosition")]
        nuint CurrentPosition { get; }

        //-(NSUInteger) duration;
        [Export("duration")]
        nuint Duration { get; }

        //-(NSString*) tui;
        [Export("tui")]
        string Tui { get; }

        //-(NSString*) tuiTag;
        [Export("tuiTag")]
        string TuiTag { get; }

        //-(NSString*) tagId;
        [Export("tagId")]
        string TagId { get; }

        //-(NSString*) gnId;
        [Export("gnId")]
        string GnId { get; }

        //-(NSString*) gnUId;
        [Export("gnUId")]
        string GnUId { get; }

        //-(NSString*) trackNumber;
        [Export("trackNumber")]
        string TrackNumber { get; }

        //-(NSString*) year;
        [Export("year")]
        string Year { get; }

        //-(NSString*) matchLookupType;
        [Export("matchLookupType")]
        string MatchLookupType { get; }

        //-(NSString*) matchConfidence;
        [Export("matchConfidence")]
        string MatchConfidence { get; }

        //-(NSUInteger) matchScore;
        [Export("matchScore")]
        nuint MatchScore { get; }

        //-(GnTitle*) titleClassical;
        [Export("titleClassical")]
        GnTitle TitleClassical { get; }

        //-(GnTitle*) titleRegional;
        [Export("titleRegional")]
        GnTitle TitleRegional { get; }
    }

    [BaseType(typeof(GnDataObject))]
    interface GnContent
    {

        //-(NSString*) id;
        [Export("id")]
        string Id { get; }

        //-(GnContentType) contentType;
        [Export("contentType")]
        GnContentType ContentType { get; }

        //-(NSString*) mimeType;
        [Export("mimeType")]
        string MimeType { get; }
    }

    [BaseType(typeof(GnDataObject))]
    interface GnTitle
    {

        //-(NSString*) language;
        [Export("language")]
        string Language { get; }

        //-(NSString*) languageCode;
        [Export("languageCode")]
        string LanguageCode { get; }

        //-(NSString*) display;
        [Export("display")]
        string Display { get; }

        //-(NSString*) prefix;
        [Export("prefix")]
        string Prefix { get; }

        //-(NSString*) sortable;
        [Export("sortable")]
        string Sortable { get; }

        //-(NSString*) sortableScheme;
        [Export("sortableScheme")]
        string SortableScheme { get; }

        //-(NSString*) mainTitle;
        [Export("mainTitle")]
        string MainTitle { get; }

        //-(NSString*) edition;
        [Export("edition")]
        string Edition { get; }

    }

    [BaseType(typeof(GnDataObject))]
    interface GnArtist
    {

        //-(GnName*) name;
        [Export("name")]
        GnName Name { get; }
    }

    [BaseType(typeof(GnDataObject))]
    interface GnName
    {

        //-(NSString*) language;
        [Export("language")]
        string Language { get; }

        //-(NSString*) languageCode;
        [Export("languageCode")]
        string LanguageCode { get; }

        //-(NSString*) display;
        [Export("display")]
        string Display { get; }

        //-(NSString*) sortable;
        [Export("sortable")]
        string Sortable { get; }

        //-(NSString*) sortableScheme;
        [Export("sortableScheme")]
        string SortableScheme { get; }

        //-(NSString*) prefix;
        [Export("prefix")]
        string Prefix { get; }

        //-(NSString*) family;
        [Export("family")]
        string Family { get; }

        //-(NSString*) given;
        [Export("given")]
        string Given { get; }

        //-(NSString*) globalId;
        [Export("globalId")]
        string GlobalId { get; }

    }

    [BaseType(typeof(NSObject))]
    interface GnLog
    {
        //-(INSTANCE_RETURN_TYPE) initWithLogFilePath: (NSString*)logFilePath logEventsDelegate: (id<GnLogEventsDelegate>)pLoggingDelegate;
        [Export("initWithLogFilePath:logEventsDelegate:")]
        IntPtr Constructor(string logFilePath, IGnLogEventsDelegate pLoggingDelegate);

        //-(INSTANCE_RETURN_TYPE) initWithLogFilePath: (NSString*)logFilePath filters: (GnLogFilters*)filters columns: (GnLogColumns*)columns options: (GnLogOptions*)options logEventsDelegate: (id<GnLogEventsDelegate>)pLoggingDelegate;
        [Export("initWithLogFilePath:filters:columns:options:logEventsDelegate:")]
        IntPtr Constructor(string logFilePath, GnLogFilters filters, GnLogColumns columns, GnLogOptions options, IGnLogEventsDelegate pLoggingDelegate);

        //-(void) options: (GnLogOptions*)options;
        [Export("options:")]
        void Options(GnLogOptions options);

        //-(void) filters: (GnLogFilters*)filters;
        [Export("filters:")]
        void Filters(GnLogFilters filters);

        //-(void) columns: (GnLogColumns*)columns;
        [Export("columns:")]
        void Columns(GnLogColumns columns);

        //-(GnLog*) enableWithPackage: (GnLogPackageType)package error: (NSError**)error;
        [Export("enableWithPackage:error:")]
        GnLog EnableWithPackage(GnLogPackageType package, out NSError error);

        //-(GnLog*) enableWithCustomPackageId: (NSUInteger)customPackageId error: (NSError**)error;
        [Export("enableWithCustomPackageId:error:")]
        GnLog EnableWithCustomPackageId(nuint customPackageId, out NSError error);

        //-(GnLog*) disableWithPackage: (GnLogPackageType)package error: (NSError**)error;
        [Export("disableWithPackage:error:")]
        GnLog DisableWithPackage(GnLogPackageType package, out NSError error);

        //-(GnLog*) disableWithCustomPackageId: (NSUInteger)customPackageId error: (NSError**)error;
        [Export("disableWithCustomPackageId:error:")]
        GnLog DisableWithCustomPackageId(nuint customPackageId, out NSError error);

        //-(GnLog*) register: (NSUInteger)customPackageId customPackageName: (NSString*)customPackageName error: (NSError**)error;
        [Export("register:customPackageName:error:")]
        GnLog Register(nuint customPackageId, string customPAckageName, out NSError error);
    }

    [BaseType(typeof(NSObject))]
    [Model]
    [Protocol]
    interface GnLogEventsDelegate
    {
        [Abstract]
        [Export("logMessage:filterMask:errorCode:")]
        void LogMessage(nuint packageId, nuint filterMask, nuint errorCode);
    }


    interface IGnLogEventsDelegate { }

    [BaseType(typeof(NSObject))]
    interface GnLogFilters
    {

        //-(NSString*) language;
        [Export("clear")]
        GnLogFilters Clear { get; }

        //-(NSString*) languageCode;
        [Export("error")]
        GnLogFilters Error { get; }

        //-(NSString*) display;
        [Export("warning")]
        GnLogFilters Warning { get; }

        //-(NSString*) sortable;
        [Export("info")]
        GnLogFilters Info { get; }

        //-(NSString*) sortableScheme;
        [Export("debug")]
        GnLogFilters Debug { get; }

        //-(NSString*) prefix;
        [Export("all")]
        GnLogFilters All { get; }
    }

    [BaseType(typeof(NSObject))]
    interface GnLogColumns
    {
        //-(GnLogColumns*) timeStamp;
        [Export("timeStamp")]
        GnLogColumns TimeStamp { get; }

        //-(GnLogColumns*) category;
        [Export("category")]
        GnLogColumns Category { get; }

        //-(GnLogColumns*) packageName;
        [Export("packageName")]
        GnLogColumns PackageName { get; }

        //-(GnLogColumns*) thread;
        [Export("thread")]
        GnLogColumns Thread { get; }

        //-(GnLogColumns*) sourceInfo;
        [Export("sourceInfo")]
        GnLogColumns SourceInfo { get; }

        //-(GnLogColumns*) newLine;
        [Export("newLine")]
        GnLogColumns NewLine { get; }

        //-(GnLogColumns*) all;
        [Export("all")]
        GnLogColumns All { get; }
    }

    [BaseType(typeof(NSObject))]
    interface GnLogOptions
    {
        //-(GnLogOptions*) synchronous: (BOOL)bSyncWrite;
        [Export("synchronous:")]
        GnLogOptions Synchronous(bool bSyncWrite);

        //-(GnLogOptions*) archive: (BOOL)bArchive;
        [Export("archive:")]
        GnLogOptions Archive(bool bArchive);

        //-(GnLogOptions*) archiveDaily;
        [Export("archiveDaily")]
        GnLogOptions ArchiveDaily { get; }

        //-(GnLogOptions*) maxSize: (NSUInteger)maxSize;
        [Export("maxSize:")]
        GnLogOptions MaxSize(nuint maxSize);
    }

    [BaseType(typeof(NSObject))]
    interface GnDataModel
    {
        //@property (strong) NSString* albumArtist;
        [Export("albumArtist")]
        string AlbumArtist { get; }
        //@property (strong) NSString* albumGenre;
        [Export("albumGenre")]
        string AlbumGenre { get; }
        //@property (strong) NSString* albumID;
        [Export("albumID")]
        string AlbumID { get; }
        //@property (strong) NSString* albumXID;
        [Export("albumXID")]
        string AlbumXID { get; }
        //@property (strong) NSString* albumYear;
        [Export("albumYear")]
        string AlbumYear { get; }
        //@property (strong) NSString* albumTitle;
        [Export("albumTitle")]
        string AlbumTitle { get; }
        //@property (strong) NSString* albumTrackCount;
        [Export("albumTrackCount")]
        string AlbumTrackCount { get; }
        //@property (strong) NSString* albumLanguage;
        [Export("albumLanguage")]
        string AlbumLanguage { get; }
        //@property (strong) NSString* albumReview;
        [Export("albumReview")]
        string AlbumReview { get; }
        //@property (strong) NSData* albumImageData;
        [Export("albumImageData")]
        NSData AlbumImageData { get; }
        //@property (strong) NSString* albumImageURLString;
        [Export("albumImageURLString")]
        string AlbumImageURLString { get; }

        //@property (strong) NSString* trackArtist;
        [Export("trackArtist")]
        string TrackArtist { get; }

        //@property (strong) NSString* trackMood;
        [Export("trackMood")]
        string TrackMood { get; }
        //@property (strong) NSData* artistImageData;
        [Export("artistImageData")]
        NSData ArtistImageData { get; }
        //@property (strong) NSString* artistImageURLString;
        [Export("artistImageURLString")]
        string ArtistImageURLString { get; }
        //@property (strong) NSString* artistBiography;
        [Export("artistBiography")]
        string ArtistBiography { get; }

        //@property (strong) NSString* currentPosition;
        [Export("currentPosition")]
        string CurrentPosition { get; }
        //@property (strong) NSString* trackMatchPosition;
        [Export("trackMatchPosition")]
        string TrackMatchPosition { get; }
        //@property (strong) NSString* trackDuration;
        [Export("trackDuration")]
        string TrackDuration { get; }
        //@property (strong) NSString* trackTempo;
        [Export("trackTempo")]
        string TrackTempo { get; }
        //@property (strong) NSString* trackOrigin;
        [Export("trackOrigin")]
        string TrackOrigin { get; }
        //@property (strong) NSString* trackGenre;
        [Export("trackGenre")]
        string TrackGenre { get; }
        //@property (strong) NSString* trackID;
        [Export("trackID")]
        string TrackID { get; }
        //@property (strong) NSString* trackXID;
        [Export("trackXID")]
        string TrackXID { get; }
        //@property (strong) NSString* trackNumber;
        [Export("trackNumber")]
        string TrackNumber { get; }
        //@property (strong) NSString* trackTitle;
        [Export("trackTitle")]
        string TrackTitle { get; }
        //@property (strong) NSString* trackArtistType;
        [Export("trackArtistType")]
        string TrackArtistType { get; }

        //-(void) startDownloadingImageFromURL:(NSURL*) imageURL;
        [Export("startDownloadingImageFromURL:")]
        void startDownloadingImageFromURL(NSUrl imageURL);
    }


    [BaseType(typeof(NSManagedObject))]
    interface History
    {
        //@property (nonatomic, retain) NSNumber* auto_id;
        [Export("auto_id")]
        int AutoID { get; }

        //@property (nonatomic, retain) NSDate* current_date;
        [Export("current_date")]
        NSDate CurrentDate { get; }
        //@property (nonatomic, retain) NSString* fingerprint;
        [Export("fingerprint")]
        string Fingerprint { get; }
        //@property (nonatomic, retain) NSNumber* latitude;
        [Export("latitude")]
        int Latitude { get; }
        //@property (nonatomic, retain) NSNumber* longitude;
        [Export("longitude")]
        int Longitude { get; }
        //@property (nonatomic, retain) Metadata* metadata;
        [Export("metadata")]
        Metadata Metadata { get; }
    }


    [BaseType(typeof(NSManagedObject))]
    interface Metadata
    {

        //@property (nonatomic, retain) NSString* albumId;
        [Export("metadata")]
        string AlbumId { get; }
        //@property (nonatomic, retain) NSString* albumTitle;
        [Export("albumTitle")]
        string AlbumTitle { get; }
        //@property (nonatomic, retain) NSString* albumTitleYomi;
        [Export("albumTitleYomi")]
        string AlbumTitleYomi { get; }
        //@property (nonatomic, retain) NSNumber* albumTrackCount;
        [Export("albumTrackCount")]
        int AlbumTrackCount { get; }
        //@property (nonatomic, retain) NSString* artist;
        [Export("artist")]
        string Artist { get; }
        //@property (nonatomic, retain) NSString* artistBetsumei;
        [Export("artistBetsumei")]
        string ArtistBetsumei { get; }
        //@property (nonatomic, retain) NSString* artistYomi;
        [Export("artistYomi")]
        string ArtistYomi { get; }
        //@property (nonatomic, retain) NSString* genre;
        [Export("genre")]
        string Genre { get; }
        //@property (nonatomic, retain) NSNumber* trackNumber;
        [Export("trackNumber")]
        int TrackNumber { get; }
        //@property (nonatomic, retain) NSString* trackTitle;
        [Export("trackTitle")]
        string TrackTitle { get; }
        //@property (nonatomic, retain) NSString* trackTitleYomi;
        [Export("trackTitleYomi")]
        string TrackTitleYomi { get; }
        //@property (nonatomic, retain) NSManagedObject* history;
        [Export("history")]
        NSManagedObject History { get; }
        //@property (nonatomic, retain) NSManagedObject* coverArt;
        [Export("coverArt")]
        NSManagedObject CoverArt { get; }
    }

    [BaseType(typeof(NSManagedObject))]
    interface CoverArt
    {
        //@property (nonatomic, retain) NSData* data;
        [Export("data")]
        NSData Data { get; }
        //@property (nonatomic, retain) NSString* mimeType;
        [Export("mimeType")]
        string MimeType { get; }
        //@property (nonatomic, retain) NSString* size;
        [Export("size")]
        string Size { get; }
        //@property (nonatomic, retain) Metadata* metaData;
        [Export("metaData")]
        Metadata MetaData { get; }
    }

    [BaseType(typeof(NSObject))]
    interface GnStorageSqlite
    {

        //+(GnStorageSqlite*) enable:(NSError**) error;
        [Static, Export("enable:")]
        GnStorageSqlite Enable(out NSError error);

        //+(NSString*) version;
        [Static, Export("version")]
        string Version { get; }

        //+(NSString*) buildDate;
        [Static, Export("buildDate")]
        string BuildDate { get; }

        //+(NSString*) sqliteVersion;
        [Static, Export("sqliteVersion")]
        string SqliteVersion { get; }

        //-(void) storageLocationWithFolderPath: (NSString*)folderPath error: (NSError**)error;
        [Export("storageLocationWithFolderPath:error:")]
        void StorageLocationWithFolderPath(string folderPath, out NSError error);

        //-(NSString*) storageLocation:(NSError**) error;
        [Export("storageLocation:")]
        string StorageLocation(out NSError error);

        //-(NSString*) temporaryStorageLocation:(NSError**) error;
        [Export("temporaryStorageLocation:")]
        string TemporaryStorageLocation(out NSError error);

        //-(void) temporaryStorageLocationWithFolderPath: (NSString*)folderPath error: (NSError**)error;
        [Export("temporaryStorageLocationWithFolderPath:error:")]
        void TemporaryStorageLocationWithFolderPath(string folderPath, out NSError error);

        //-(void) maximumCacheFileSizeWithMaxCacheSize: (NSUInteger)maxCacheSize error: (NSError**)error;
        [Export("maximumCacheFileSizeWithMaxCacheSize:error:")]
        void MaximumCacheFileSizeWithMaxCacheSize(nint maxCacheSize, out NSError error);

        //-(NSUInteger) maximumCacheFileSize:(NSError**) error;
        [Export("maximumCacheFileSize:")]
        nuint MaximumCacheFileSize(out NSError error);

        //-(void) maximumCacheMemoryWithMaxMemSize: (NSUInteger)maxMemSize error: (NSError**)error;
        [Export("maximumCacheMemoryWithMaxMemSize:error:")]
        nuint MaximumCacheMemoryWithMaxMemSize(nuint maxMemSize, out NSError error);


        //-(NSUInteger) maximumCacheMemory:(NSError**) error;
        [Export("maximumCacheMemory:")]
        nuint MaximumCacheMemory(out NSError error);

        //-(void) synchronousModeWithMode: (NSString*)mode error: (NSError**)error;
        [Export("synchronousModeWithMode:error:")]
        void SynchronousModeWithMode(string mode, out NSError error);

        //-(NSString*) synchronousMode:(NSError**) error;
        [Export("synchronousMode:")]
        string SynchronousMode(out NSError error);

        //-(void) journalModeWithMode: (NSString*)mode error: (NSError**)error;
        [Export("journalModeWithMode:error:")]
        void JournalModeWithMode(string mode, out NSError error);

        //-(NSString*) journalMode:(NSError**) error;
        [Export("journalMode:")]
        string JournalMode(out NSError error);

    }

    [BaseType(typeof(NSObject))]
    interface GnLookupLocalStream
    {
        //+(GnLookupLocalStream*) enable:(NSError**) error;
        [Static, Export("enable:")]
        GnLookupLocalStream Enable(out NSError error);

        //+(NSString*) version;
        [Static, Export("version")]
        string Version { get; }

        //+(NSString*) buildDate;
        [Static, Export("buildDate")]
        string BuildDate { get; }

        //-(void) storageLocation: (NSString*)location error: (NSError**)error;
        [Export("storageLocation:error:")]
        void StorageLocation(string location, out NSError error);

        //-(void) engineTypeWithEngineType: (GnLocalStreamEngineType)engineType error: (NSError**)error;
        [Export("engineTypeWithEngineType:error:")]
        void EngineTypeWithEngineType(GnLocalStreamEngineType engineType, out NSError error);

        //-(GnLocalStreamEngineType) engineType:(NSError**) error;
        [Export("engineType:")]
        GnLocalStreamEngineType EngineType(out NSError error);

        //-(void) storageClear:(NSError**) error;
        [Export("storageClear:")]
        void StorageClear(out NSError error);

        //-(void) storageRemove: (NSString*)bundleItemId error: (NSError**)error;
        [Export("storageRemove:error:")]
        void StorageRemove(string bundleItemId, out NSError error);
    }

    [BaseType(typeof(NSObject))]
    interface GnLookupLocalStreamIngest
    {
        //	-(INSTANCE_RETURN_TYPE) initWithGnLookupLocalStreamIngestEventsDelegate: (id<GnLookupLocalStreamIngestEventsDelegate>)pEventDelegate;
        [Export("initWithGnLookupLocalStreamIngestEventsDelegate:")]
        IntPtr Constructor(IGnLookupLocalStreamIngestEventsDelegate pEventDelegate);

        //-(void) write: (unsigned char*)bundleData dataSize: (NSUInteger)dataSize error: (NSError**)error;
        [Export("write:dataSize:error:")]
        void Write(NSData bundleData, nuint dataSize, out NSError error);

        //-(void) flush:(NSError**) error;
        [Export("flush:")]
        void Flush(out NSError error);
    }



    [BaseType(typeof(NSObject))]
    [Model]
    [Protocol]
    interface GnLookupLocalStreamIngestEventsDelegate
    {
        //@required
        //-(void) statusEvent: (GnLookupLocalStreamIngestStatus)status bundleId: (NSString*)bundleId cancellableDelegate: (id<GnCancellableDelegate>)canceller;
        [Abstract]
        [Export("statusEvent:bundleId:cancellableDelegate:")]
        void StatusEvent(GnLookupLocalStreamIngestStatus status, string bundleId, IGnCancellableDelegate canceller);
    }


    interface IGnLookupLocalStreamIngestEventsDelegate { }


    [BaseType(typeof(NSObject))]
    interface GnMic : GnAudioSourceDelegate
    {
        //- (instancetype) initWithSampleRate:(Float64) sampleRate bitsPerChannel:(UInt32) bitsPerChannel numberOfChannels:(UInt32)  numberOfChannels;
        [Export("initWithSampleRate:bitsPerChannel:numberOfChannels:")]
        IntPtr Constructor(float sampleRate, nuint bitsPerChannel, nuint numberOfChannels);
    }

    [BaseType(typeof(NSObject))]
    interface GnMusicIdFile
    {
        //-(INSTANCE_RETURN_TYPE) initWithGnUser: (GnUser*)user musicIdFileEventsDelegate: (id <GnMusicIdFileEventsDelegate>)pEventHandler;
        [Export("initWithGnUser:musicIdFileEventsDelegate:")]
        IntPtr Constructor(GnUser user, IGnMusicIdFileEventsDelegate pEventDelegate);

        //+(NSString*) version;
        [Static, Export("version")]
        string Version { get; }

        //+(NSString*) version;
        [Static, Export("buildDate")]
        string BuildDate { get; }

        //-(GnMusicIdFileOptions*) options;
        [Export("buildDate")]
        GnMusicIdFileOptions Options { get; }

        //-(GnMusicIdFileInfoManager*) fileInfos;
        [Export("fileInfos")]
        GnMusicIdFileInfoManager FileInfos { get; }

        //-(void) doTrackId: (GnMusicIdFileProcessType)processType responseType: (GnMusicIdFileResponseType)responseType error: (NSError**)error;
        [Export("doTrackId:responseType:error:")]
        void DoTrackId(GnMusicIdFileProcessType processType, GnMusicIdFileResponseType responseType, out NSError error);

        //-(void) doTrackIdAsync: (GnMusicIdFileProcessType)processType responseType: (GnMusicIdFileResponseType)responseType error: (NSError**)error;
        [Export("doTrackIdAsync:responseType:error:")]
        void DoTrackIdAsync(GnMusicIdFileProcessType processType, GnMusicIdFileResponseType responseType, out NSError error);

        //-(void) doAlbumId: (GnMusicIdFileProcessType)processType responseType: (GnMusicIdFileResponseType)responseType error: (NSError**)error;
        [Export("doAlbumId:responseType:error:")]
        void DoAlbumId(GnMusicIdFileProcessType processType, GnMusicIdFileResponseType responseType, out NSError error);

        //-(void) doAlbumIdAsync: (GnMusicIdFileProcessType)processType responseType: (GnMusicIdFileResponseType)responseType error: (NSError**)error;
        [Export("doAlbumIdAsync:responseType:error:")]
        void DoAlbumIdAsync(GnMusicIdFileProcessType processType, GnMusicIdFileResponseType responseType, out NSError error);

        //-(void) doLibraryId: (GnMusicIdFileResponseType)responseType error: (NSError**)error;
        [Export("doLibraryId:error:")]
        void DoLibraryId(GnMusicIdFileResponseType responseType, out NSError error);

        //-(void) doLibraryIdAsync: (GnMusicIdFileResponseType)responseType error: (NSError**)error;
        [Export("doLibraryIdAsync:error:")]
        void DoLibraryIdAsync(GnMusicIdFileResponseType responseType, out NSError error);

        //-(void) waitForCompleteWithTimeoutValue: (NSUInteger)timeoutValue error: (NSError**)error;
        [Export("waitForCompleteWithTimeoutValue:error:")]
        void WaitForCompleteWithTimeoutValue(nint timeoutValue, out NSError error);

        //-(void) waitForComplete:(NSError**) error;
        [Export("waitForComplete:")]
        void WaitForComplete(out NSError error);

        //-(void) cancel;
        [Export("cancel")]
        void Cancel();
    }
    [BaseType(typeof(NSObject))]
    [Model]
    [Protocol]
    interface GnMusicIdFileEventsDelegate : GnStatusEventsDelegate
    {

        //-(void) musicIdFileStatusEvent: (GnMusicIdFileInfo*)fileInfo status: (GnMusicIdFileCallbackStatus)status currentFile: (NSUInteger)currentFile totalFiles: (NSUInteger)totalFiles cancellableDelegate: (id<GnCancellableDelegate>)canceller;
        [Abstract]
        [Export("musicIdFileStatusEvent:status:currentFile:totalFiles:cancellableDelegate:")]
        void MusicIdFileStatusEvent(GnMusicIdFileInfo fileInfo, GnMusicIdFileCallbackStatus status, nint currentFile, nint totalFiles, IGnCancellableDelegate canceller);

        //-(void) gatherFingerprint: (GnMusicIdFileInfo*)fileInfo currentFile: (NSUInteger)currentFile totalFiles: (NSUInteger)totalFiles cancellableDelegate: (id<GnCancellableDelegate>)canceller;
        [Export("gatherFingerprint:currentFile:totalFiles:cancellableDelegate:")]
        void MusicIdFileStatusEvent(GnMusicIdFileInfo fileInfo, nint currentFile, nint totalFiles, IGnCancellableDelegate canceller);

        //-(void) gatherMetadata: (GnMusicIdFileInfo*)fileInfo currentFile: (NSUInteger)currentFile totalFiles: (NSUInteger)totalFiles cancellableDelegate: (id<GnCancellableDelegate>)canceller;
        [Export("gatherMetadata:currentFile:totalFiles:cancellableDelegate:")]
        void GatherMetadata(GnMusicIdFileInfo fileInfo, nint currentFile, nint totalFiles, IGnCancellableDelegate canceller);

        //-(void) musicIdFileAlbumResult: (GnResponseAlbums*)albumResult currentAlbum: (NSUInteger)currentAlbum totalAlbums: (NSUInteger)totalAlbums cancellableDelegate: (id<GnCancellableDelegate>)canceller;
        [Export("musicIdFileAlbumResult:currentAlbum:totalAlbums:cancellableDelegate:")]
        void MusicIdFileStatusEvent(GnResponseAlbums albumResult, nint currentAlbum, nint totalAlbums, IGnCancellableDelegate canceller);

        //-(void) musicIdFileMatchResult: (GnResponseDataMatches*)matchesResult currentAlbum: (NSUInteger)currentAlbum totalAlbums: (NSUInteger)totalAlbums cancellableDelegate: (id<GnCancellableDelegate>)canceller;
        [Export("musicIdFileMatchResult:currentAlbum:totalAlbums:cancellableDelegate:")]
        void MusicIdFileMatchResult(GnResponseDataMatches matchesResult, nint currentAlbum, nint totalAlbums, IGnCancellableDelegate canceller);

        //-(void) musicIdFileResultNotFound: (GnMusicIdFileInfo*)fileInfo currentFile: (NSUInteger)currentFile totalFiles: (NSUInteger)totalFiles cancellableDelegate: (id<GnCancellableDelegate>)canceller;
        [Export("musicIdFileResultNotFound:currentFile:totalFiles:cancellableDelegate:")]
        void MusicIdFileResultNotFound(GnMusicIdFileInfo fileInfo, nint currentFile, nint totalFiles, IGnCancellableDelegate canceller);

        //-(void) musicIdFileComplete: (NSError*)completeError;
        [Export("musicIdFileComplete:")]
        void MusicIdFileResultNotFound(out NSError completeError);
    }

    interface IGnMusicIdFileEventsDelegate { }


    [BaseType(typeof(NSObject))]
    interface GnMusicIdFileInfo
    {
        //        -(NSString*) identifier:(NSError**) error;
        [Export("identifier:")]
        string Identifier(out NSError error);

        //-(NSString*) fileName:(NSError**) error;
        [Export("fileName:")]
        string FileName(out NSError error);

        //-(void) fileNameWithValue: (NSString*)value error: (NSError**)error;
        [Export("fileNameWithValue:error:")]
        void FileNameWithValue(string value, out NSError error);

        //-(NSString*) cddbId:(NSError**) error;
        [Export("cddbId:")]
        string CddbId(out NSError error);

        //-(void) cddbIdWithValue: (NSString*)value error: (NSError**)error;
        [Export("cddbIdWithValue:error:")]
        void cddbIdWithValue(string value, out NSError error);

        //-(NSString*) albumArtist:(NSError**) error;
        [Export("albumArtist:")]
        string AlbumArtist(out NSError error);

        //-(void) albumArtistWithValue: (NSString*)value error: (NSError**)error;
        [Export("albumArtistWithValue:error:")]
        string AlbumArtistWithValue(string value, out NSError error);

        //-(NSString*) albumTitle:(NSError**) error;
        [Export("albumTitle:")]
        string AlbumTitle(out NSError error);

        //-(void) albumTitleWithValue: (NSString*)value error: (NSError**)error;
        [Export("albumTitleWithValue:error:")]
        string AlbumTitleWithValue(string value, out NSError error);

        //-(NSString*) trackArtist:(NSError**) error;
        [Export("trackArtist:")]
        string TrackArtist(out NSError error);

        //-(void) trackArtistWithValue: (NSString*)value error: (NSError**)error;
        [Export("trackArtistWithValue:error:")]
        void TrackArtistWithValue(string value, out NSError error);

        //-(NSString*) trackTitle:(NSError**) error;
        [Export("trackTitle:")]
        string TrackTitle(out NSError error);

        //-(void) trackTitleWithValue: (NSString*)value error: (NSError**)error;
        [Export("trackTitleWithValue:error:")]
        void TrackTitleWithValue(string value, out NSError error);

        //-(NSUInteger) trackNumber:(NSError**) error;
        [Export("trackNumber:")]
        nint TrackNumber(out NSError error);

        //-(void) trackNumberWithTrackNumber: (NSUInteger)trackNumber error: (NSError**)error;
        [Export("trackNumberWithTrackNumber:error:")]
        void TrackNumberWithTrackNumber(nint trackNumber, out NSError error);

        //-(NSUInteger) discNumber:(NSError**) error;
        [Export("discNumber:")]
        nint DiscNumber(out NSError error);

        //-(void) discNumberWithDiscNumber: (NSUInteger)discNumber error: (NSError**)error;
        [Export("discNumberWithDiscNumber:error:")]
        void DiscNumberWithDiscNumber(nint discNumber, out NSError error);

        //-(NSString*) tagId:(NSError**) error;
        [Export("tagId:")]
        string TagId(out NSError error);

        //-(void) tagIdWithValue: (NSString*)value error: (NSError**)error;
        [Export("tagIdWithValue:error:")]
        void TagIdWithValue(string value, out NSError error);

        //-(NSString*) fingerprint:(NSError**) error;
        [Export("fingerprint:")]
        string Fingerprint(out NSError error);

        //-(void) fingerprintWithValue: (NSString*)value error: (NSError**)error;
        [Export("fingerprintWithValue:error:")]
        string FingerprintWithValue(string value, out NSError error);

        //-(NSString*) mediaId:(NSError**) error;
        [Export("mediaId:")]
        string MediaId(out NSError error);

        //-(void) mediaIdWithValue: (NSString*)value error: (NSError**)error;
        [Export("mediaIdWithValue:error:")]
        void MediaIdWithValue(string value, out NSError error);

        //-(NSString*) mui:(NSError**) error;
        [Export("mui:")]
        string Mui(out NSError error);

        //-(void) muiWithValue: (NSString*)value error: (NSError**)error;
        [Export("muiWithValue:error:")]
        void MuiWithValue(string value, out NSError error);

        //-(NSString*) cdToc:(NSError**) error;
        [Export("cdToc:")]
        string CdToc(out NSError error);

        //-(void) cdTocWithValue: (NSString*)value error: (NSError**)error;
        [Export("cdTocWithValue:error:")]
        void CdTocWithValue(string value, out NSError error);

        //-(NSString*) tui:(NSError**) error;
        [Export("tui:")]
        string Tui(out NSError error);

        //-(void) tuiWithValue: (NSString*)value error: (NSError**)error;
        [Export("tuiWithValue:error:")]
        void TuiWithValue(string value, out NSError error);

        //-(NSString*) tuiTag:(NSError**) error;
        [Export("tuiTag:")]
        string TuiTag(out NSError error);

        //-(void) tuiTagWithValue: (NSString*)value error: (NSError**)error;
        [Export("tuiTagWithValue:error:")]
        void TuiTagWithValue(string value, out NSError error);

        //-(void) fingerprintBegin: (NSUInteger)audioSampleRate audioSampleSize: (NSUInteger)audioSampleSize audioChannels: (NSUInteger)audioChannels error: (NSError**)error;
        [Export("fingerprintBegin:audioSampleSize:audioChannels:error:")]
        void TuiTagWithValue(nint audioSampleRate, nint audioSampleSize, nint audioChannels, out NSError error);

        //-(BOOL) fingerprintWrite: (unsigned char*)audioData audioDataSize: (NSUInteger)audioDataSize error: (NSError**)error;
        [Export("fingerprintWrite:audioDataSize:error:")]
        bool FingerprintWrite(NSData audioData, nint audioDataSize, out NSError error);

        //-(void) fingerprintEnd:(NSError**) error;
        [Export("fingerprintEnd:")]
        void FingerprintEnd(out NSError error);

        //-(void) fingerprintFromSource: (id<GnAudioSourceDelegate>)audioSource error: (NSError**)error;
        [Export("fingerprintFromSource:error:")]
        void FingerprintFromSource(IGnAudioSourceDelegate audioSource, out NSError error);

        //-(GnMusicIdFileInfoStatus) status:(NSError**) error;
        [Export("status:")]
        GnMusicIdFileInfoStatus Status(out NSError error);

        //-(NSError*) errorInformation:(NSError**) error;
        [Export("errorInformation:")]
        NSError ErrorInformation(out NSError error);

        //-(GnResponseAlbums*) albumResponse:(NSError**) error;
        [Export("albumResponse:")]
        GnResponseAlbums AlbumResponse(out NSError error);

        //-(GnResponseDataMatches*) dataMatchResponse:(NSError**) error;
        [Export("dataMatchResponse:")]
        GnResponseDataMatches DataMatchResponse(out NSError error);
    }

    [BaseType(typeof(NSObject))]
    interface GnMusicIdFileOptions
    {

        //-(void) lookupMode: (GnLookupMode)lookupMode error: (NSError**)error;
        [Export("lookupMode:error:")]
        void LookupMode(GnLookupMode lookupMode, out NSError error);

        //-(void) lookupData: (GnLookupData)val enable: (BOOL)enable error: (NSError**)error;
        [Export("lookupData:enable:error:")]
        void LookupMode(GnLookupData val, bool enable, out NSError error);

        //-(void) batchSize: (NSUInteger)size error: (NSError**)error;
        [Export("batchSize:error:")]
        void BatchSize(nint size, out NSError error);

        //-(void) onlineProcessing: (BOOL)enable error: (NSError**)error;
        [Export("onlineProcessing:error:")]
        void OnlineProcessing(bool enable, out NSError error);

        //-(void) preferResultLanguage: (GnLanguage)preferredLangauge error: (NSError**)error;
        [Export("preferResultLanguage:error:")]
        void PreferResultLanguage(GnLanguage preferredLangauge, out NSError error);

        //-(void) preferResultExternalId: (NSString*)preferredExternalId error: (NSError**)error;
        [Export("preferResultExternalId:error:")]
        void PreferResultExternalId(string preferredExternalId, out NSError error);

        //-(void) threadPriority: (GnThreadPriority)threadPriority error: (NSError**)error;
        [Export("threadPriority:error:")]
        void ThreadPriority(GnThreadPriority preferredExternalId, out NSError error);

        //-(void) networkInterfaceWithIntfName: (NSString*)intfName error: (NSError**)error;
        [Export("networkInterfaceWithIntfName:error:")]
        void NetworkInterfaceWithIntfName(string preferredExternalId, out NSError error);

        //-(NSString*) networkInterface:(NSError**) error;
        [Export("networkInterface:")]
        string NetworkInterface(out NSError error);

        //-(void) customWithOptionKey: (NSString*)optionKey enable: (BOOL)enable error: (NSError**)error;
        [Export("customWithOptionKey:enable:error:")]
        void CustomWithOptionKey(string optionKey, bool enable, out NSError error);

        //-(void) customWithOption: (NSString*)option value: (NSString*)value error: (NSError**)error;
        [Export("customWithOption:enable:error:")]
        void CustomWithOption(string option, bool enable, out NSError error);
    }

    [BaseType(typeof(NSObject))]
    interface GnMusicIdFileInfoManager
    {

        //-(GnMusicIdFileInfo*) add: (NSString*)uniqueIdentifier musicIdFileInfoEventsDelegate: (id<GnMusicIdFileInfoEventsDelegate>)pEventHandler error: (NSError**)error;
        [Export("add:musicIdFileInfoEventsDelegate:error:")]
        GnMusicIdFileInfo Add(string uniqueIdentifier, IGnMusicIdFileInfoEventsDelegate pEventHandler, out NSError error);

        //-(NSUInteger) addFromXml: (NSString*)xmlStr error: (NSError**)error;
        [Export("addFromXml:error:")]
        nint AddFromXml(string xmlStr, out NSError error);

        //-(NSString*) renderToXml:(NSError**) error;
        [Export("renderToXml:")]
        string RenderToXml(out NSError error);

        //-(void) remove: (GnMusicIdFileInfo*)fileInfo error: (NSError**)error;
        [Export("remove:error:")]
        void Remove(GnMusicIdFileInfo fileInfo, out NSError error);

        //-(NSUInteger) count;
        [Export("count")]
        nint Count { get; }

        //-(GnMusicIdFileInfo*) at: (NSUInteger)index;
        [Export("at:")]
        GnMusicIdFileInfo At(nint index);

        //-(GnMusicIdFileInfo*) getByIdentifier: (NSString*)ident error: (NSError**)error;
        [Export("getByIdentifier:error:")]
        GnMusicIdFileInfo GetByIdentifier(string ident, out NSError error);

        //-(GnMusicIdFileInfo*) getByFilename: (NSString*)filename error: (NSError**)error;
        [Export("getByFilename:error:")]
        GnMusicIdFileInfo GetByFilename(string filename, out NSError error);

        //-(GnMusicIdFileInfo*) getByFolder: (NSString*)folder index: (NSUInteger)index error: (NSError**)error;
        [Export("getByFolder:index:error:")]
        GnMusicIdFileInfo GetByFolder(string folder, nint index, out NSError error);
    }

    [BaseType(typeof(NSObject))]
    [Model]
    [Protocol]
    interface GnMusicIdFileInfoEventsDelegate
    {
        //-(void) gatherFingerprint: (GnMusicIdFileInfo*)fileinfo currentFile: (NSUInteger)currentFile totalFiles: (NSUInteger)totalFiles cancellableDelegate: (id<GnCancellableDelegate>)canceller;
        [Export("gatherFingerprint:currentFile:totalFiles:cancellableDelegate:")]
        void GatherFingerprint(GnMusicIdFileInfo fileinfo, nint currentFile, nint totalFiles, IGnCancellableDelegate canceller);

        //-(void) gatherMetadata: (GnMusicIdFileInfo*)fileinfo currentFile: (NSUInteger)currentFile totalFiles: (NSUInteger)totalFiles cancellableDelegate: (id<GnCancellableDelegate>)canceller;
        [Export("gatherMetadata:currentFile:totalFiles:cancellableDelegate:")]
        void GatherMetadata(GnMusicIdFileInfo fileinfo, nint currentFile, nint totalFiles, IGnCancellableDelegate canceller);
    }

    interface IGnMusicIdFileInfoEventsDelegate { }
}

