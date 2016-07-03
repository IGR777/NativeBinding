using System;

using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

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

    [BaseType (typeof (NSObject))]
    interface GnManager {

	     [Export ("initWithLicense:")]
	     IntPtr Constructor (string license);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface GnUserStoreDelegate
	{
		[Abstract]
		[Export ("loadSerializedUser:")]
		string loadSerializedUser (string clientId);


		[Export ("storeSerializedUser:")]
		bool storeSerializedUser (string clientId);

		[Export ("storeSerializedUser:userData:")]
		bool storeSerializedUser (string clientId, string userData);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface GnStatusEventsDelegate
	{
		[Abstract]
		[Export ("statusEvent:percentComplete:bytesTotalSent:bytesTotalReceived:cancellableDelegate: ")]
		string loadSerializedUser (GnStatus status, int percentComplete, int bytesTotalSent, int bytesTotalReceived, GnCancellableDelegate canceller);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface GnCancellableDelegate
	{
		[Abstract]
		[Export ("setCancel:")]
		void SetCancel (bool bCancel);
	}

	[BaseType (typeof (NSObject))]
	interface GnUserStore : GnUserStoreDelegate
	{
	}


	[BaseType (typeof (NSObject))]
	interface GnUser
	{

		[Export ("initWithSerializedUser:")]
		IntPtr Constructor (string serializedUser);


		[Export ("initWithSerializedUser:clientIdTest:")]
		IntPtr Constructor (string serializedUser, string clientIdTest);


		[Export ("initWithGnUserStoreDelegate:")]
		IntPtr Constructor (GnUserStoreDelegate userStore);

		[Export ("initWithGnUserStoreDelegate:clientId:clientTag:applicationVersion:")]
		IntPtr Constructor (GnUserStoreDelegate userStore, string clientId, string clientTag, string applicationVersion);
	}

	[BaseType (typeof (NSObject))]
	interface GnLocalInfo
	{

		[Export ("initWithGnLocaleGroup:")]
		IntPtr Constructor (GnLocaleGroup group);

		[Export ("initWithGnLocaleGroup:language:region:descriptor:")]
		IntPtr Constructor (GnLocaleGroup group, GnLanguage language, GnRegion region, GnDescriptor descriptor);
	}

	[BaseType (typeof (NSObject))]
	interface GnLocale
	{

		[Export ("initWithGnLocaleGroup:language:region:descriptor:user:statusEventsDelegate:")]
		IntPtr Constructor (GnLocaleGroup group, GnLanguage language, GnRegion region, GnDescriptor descriptor, GnUser user, GnStatusEventsDelegate pEventHandler);


		[Export ("revision:")]
		string Revision (NSError error);


		[Export ("setGroupDefault:")]
		void SetGroupDefault (NSError error);


		[Export ("update:statusEventsDelegate:error:")]
		bool Update (GnUser user, GnStatusEventsDelegate pEventHandler, NSError error);

		[Export ("updateCheck:statusEventsDelegate:error:")]
		bool UpdateCheck (GnUser user, GnStatusEventsDelegate pEventHandler, NSError error);

		[Export ("serialize:")]
		string Serialize (NSError error);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface GnAudioSourceDelegate
	{
		[Export ("getData:dataSize:")]
		string GetData (IntPtr dataBuffer, int dataSize);
	}

	[BaseType (typeof (NSObject))]
	interface GnDataObject
	{
	}

	[BaseType (typeof (GnDataObject))]
	interface GnResponseDataMatches
	{
	}

	[BaseType (typeof (GnDataObject))]
	interface GnResponseAlbums
	{
		[Export ("from:error:")]
		GnResponseAlbums From (GnDataObject obj, NSError error);
	}

	[BaseType (typeof (NSObject))]
	interface GnMusicId
	{

		[Export ("initWithGnUser:statusEventsDelegate:")]
		IntPtr Constructor (GnUser user, GnStatusEventsDelegate pEventHandler);


		[Export ("fingerprintDataGet:")]
		string FingerprintDataGet (NSError error);

		[Export ("fingerprintBegin:audioSampleRate:audioSampleSize:audioChannels:error:")]
		void FingerprintBegin (GnFingerprintType fpType, int audioSampleRate, int audioSampleSize, int audioChannels, NSError error);

		[Export ("fingerprintWrite:audioDataSize:error:")]
		bool FingerprintWrite (IntPtr audioData, int audioDataSize,NSError error);

		[Export ("fingerprintEnd:")]
		void FingerprintEnd (NSError error);

		[Export ("fingerprintFromSource:fpType:error:")]
		void FingerprintFromSource (GnAudioSourceDelegate audioSource, GnFingerprintType fpType, NSError error);

		[Export ("findAlbumsWithAlbumTitle:trackTitle:albumArtistName:trackArtistName:composerName:error:")]
		GnResponseAlbums FindAlbumsWithAlbumTitle (string albumTitle, string trackTitle, string albumArtistName, string trackArtistName, string composeName, NSError error);

		[Export ("findAlbumsWithCDTOC:error:")]
		GnResponseAlbums FindAlbumsWithCDTOC (string CDTOC, NSError error);

		[Export ("findAlbumsWithCDTOC:strFingerprintData:fpType:error:")]
		GnResponseAlbums FindAlbumsWithCDTOC (string CDTOC, string strFingerprintData, GnFingerprintType fpType, NSError error);

		[Export ("findAlbumsWithFingerprintData:fpType:error:")]
		GnResponseAlbums FindAlbumsWithFingerprintData (string fingerprintData, GnFingerprintType fpType, NSError error);

		[Export ("findAlbumsWithGnDataObject:error:")]
		GnResponseAlbums FindAlbumsWithGnDataObject (GnDataObject gnDataObject, NSError error);

		[Export ("findAlbumsWithAudioSource:fpType:error:")]
		GnResponseAlbums FindAlbumsWithAudioSource (GnAudioSourceDelegate audioSource, GnFingerprintType fpType, NSError error);

		[Export ("findMatches:trackTitle:albumArtistName:trackArtistName:composerName:error:")]
		GnResponseDataMatches FindMatches (string albumTitle, string trackTitle, string albumArtistName, string trackArtistName, string composerName, NSError error);

		[Export ("setCancel:")]
		void SetCancel (bool bCancel);
	}

	[BaseType (typeof (NSObject))]
	interface GnAssetFetch
	{

		[Export ("initWithGnUser:url:statusEventsDelegate:")]
		IntPtr Constructor (GnUser user, string url, GnStatusEventsDelegate pEventHandler);

		//[Field ("size", "GnSDKObjC")]
		[Export ("size")]
		int Size{ get; }
	}
}

