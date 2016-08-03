using System;

namespace nativeTest
{
	public enum GnLocaleGroup: int
	{
	    /**
	    * Invalid locale group
	    */
			kLocaleGroupInvalid = 0,
	    
	    /**
	    * Locale group for the GNSDK music products. Set this when creating a locale used with the MusicID and MusicID -File libraries.
	    */
			kLocaleGroupMusic =1,
	    
	    /**
	    * Locale group for the GNSDK video products. Set this when creating a locale used with the VideoID or Video Explore libraries (or both).
	    */
			kLocaleGroupVideo = 2,
	    
	    /**
	    * Locale group for the GNSDK Playlist product. Set this when creating a locale used with the Playlist library.
	    */
			kLocaleGroupPlaylist = 3,
	    
	    /**
	    * Locale group for the GNSDK EPG product. Set this when creating a locale used with the EPG library.
	    */
			kLocaleGroupEpg = 4, 
	    
	    /**
	    * Locale group for the GNSDK ACR products. Set this when creating a locale used with the ACR library.
	    * Additionally, this will set the locale value for the MusicID, VideoID, VideoExplore and EPG libraries
	    * since those libraries are used by the ACR product.
	    *
	    * <p><b>Remarks:</b></p>
	    * If kLocaleGroupAcr is set <i>after</i> kLocaleGroupMusic, kLocaleGroupVideo or kLocaleGroupEpg is set, then kLocaleGroupAcr
	    * will override any previous locale settings used with the MusicID, VideoID/VideoExplore and EPG libraries, and set the locale
	    * for all of those libraries to the same locale value.
	    *
	    * If kLocaleGroupMusic, kLocaleGroupVideo or kLocaleGroupEpg is set <i>after</i> kLocaleGroupAcr is set, then it will override
	    * the locale value previously set by kLocaleGroupAcr for the MusicID, VideoID/VideoExplore or EPG libraries, respectively.
	    */
			kLocaleGroupAcr = 5,
	}

	public enum GnDescriptor : int
	{
		kDescriptorDefault = 0,
	    
		kDescriptorSimplified = 1,
		kDescriptorDetailed =2,
	}


	public enum GnLanguage: int
	{
		kLanguageInvalid = 0,	    
		kLanguageArabic=1,
		kLanguageBulgarian=2,
		kLanguageChineseSimplified=3,
		kLanguageChineseTraditional=4,
		kLanguageCroatian=5,
		kLanguageCzech=6,
		kLanguageDanish=7,
		kLanguageDutch=8,
		kLanguageEnglish=9,
		kLanguageFarsi=10,
		kLanguageFinnish=11,
		kLanguageFrench=12,
		kLanguageGerman=13,
		kLanguageGreek=14,
		kLanguageHungarian=15,
		kLanguageIndonesian=16,
		kLanguageItalian=17,
		kLanguageJapanese=18,
		kLanguageKorean = 19,
		kLanguageNorwegian = 20,
	    kLanguagePolish = 21,
		kLanguagePortuguese = 22,
		kLanguageRomanian = 23,
		kLanguageRussian = 24,
		kLanguageSerbian = 25,
		kLanguageSlovak = 26,
		kLanguageSpanish = 27,
		kLanguageSwedish = 28,
		kLanguageThai = 29,
		kLanguageTurkish = 30,
		kLanguageVietnamese = 31,
	}


	public enum GnRegion: int
	{
	    /** Default region. A region will be selected based on what's available. */
		kRegionDefault = 0,
	    
	    /** Global region */
		kRegionGlobal = 1,
	    
	    /** United States region */
		kRegionUS = 2,
	    
	    /** Japan region */
		kRegionJapan = 3,
	    
	    /** China region */
		kRegionChina = 4,
	    
	    /** Taiwan region */
		kRegionTaiwan = 5,
	    
	    /** Korea region */
	    kRegionKorea = 6,
	    
	    /** Europe region */
	    kRegionEurope = 7,
	    
	    /** Deprecated, will be removed in a future release, use kRegionUS. */
	    kRegionNorthAmerica = 8,
	    
	    /** Latin America region */
	    kRegionLatinAmerica = 9,
	    
	    /** India region */
	    kRegionIndia = 10
	}

	public enum GnStatus : int
	{
	    /** @internal kStatusUnknown @endinternal
	    * Status unknown.
	    * @ingroup StatusCallbacks_TypesEnums
	    */
	    kStatusUnknown = 0,
	    
	    /* Basic messages */
	    
	    /** @internal kStatusBegin @endinternal
	    * Issued once per application function call, at the beginning of the call; percent_complete = 0.
	    * @ingroup StatusCallbacks_TypesEnums
	    */
	    kStatusBegin =1,
	    
	    /** @internal kStatusProgress @endinternal
	    * Issued roughly 10 times per application function call; percent_complete values between 1-100.
	    * @ingroup StatusCallbacks_TypesEnums
	    */
	    kStatusProgress =2,
	    
	    /** @internal kStatusComplete @endinternal
	    * Issued once per application function call, at the end of the call; percent_complete = 100.
	    * @ingroup StatusCallbacks_TypesEnums
	    */
	    kStatusComplete = 3,
	    
	    /** @internal kStatusErrorInfo @endinternal
	    * Issued when an error is encountered. If sent, call #gnsdk_manager_error_info().
	    * @ingroup StatusCallbacks_TypesEnums
	    */
	    kStatusErrorInfo = 4,
	    
	    /* Advanced messages */
	    
	    /** @internal kStatusConnecting @endinternal
	    * Issued when connecting to network.
	    * @ingroup StatusCallbacks_TypesEnums
	    */
	    kStatusConnecting = 5,
	    
	    /** @internal kStatusSending @endinternal
	    * Issued when uploading.
	    * @ingroup StatusCallbacks_TypesEnums
	    */
	    kStatusSending = 6,
	    
	    /** @internal kStatusReceiving @endinternal
	    * Issued when downloading.
	    * @ingroup StatusCallbacks_TypesEnums
	    */
	    kStatusReceiving = 7,
	    
	    /** @internal kStatusDisconnected @endinternal
	    * Issued when disconnected from network.
	    * @ingroup StatusCallbacks_TypesEnums
	    */
	    kStatusDisconnected = 8,
	    
	    /** @internal kStatusReading @endinternal
	    * Issued when reading from storage.
	    * @ingroup StatusCallbacks_TypesEnums
	    */
	    kStatusReading = 9,
	    
	    /** @internal kStatusWriting @endinternal
	    * Issued when writing to storage.
	    * @ingroup StatusCallbacks_TypesEnums
	    */
	    kStatusWriting = 10,            /* issued whenever writing to storage. */
	    
	    /** @internal gnsdk_status_cancelled @endinternal
	    * Issued when transaction/query is cancelled
	    * @ingroup StatusCallbacks_TypesEnums
	    */
	    kStatusCancelled = 11
	    
	}

	public enum GnFingerprintType: int
	{
	    /**
	    * Invalid fingerprint type
	    */
	    kFingerprintTypeInvalid = 0,
	    
	    /**
	    * Specifies a fingerprint data type for generating fingerprints used with MusicID-File.
	    * <p><b>Remarks:</b></p>
	    * A MusicID-File fingerprint is a fingerprint of the beginning 16 seconds of the file.
	    * <p><b>Note:</b></p>
	    * Do not design your application to submit only 16 seconds of a file; the
	    * application must submit data until GNSDK indicates it has received enough input.
	    * Use this fingerprint type when identifying audio from a file source (MusicID-File).
	    */
	    kFingerprintTypeFile = 1,
	    
	    /**
	    *  Specifies a fingerprint used for identifying an ~3-second excerpt from an audio stream.
	    *  Use this fingerprint type when identifying a continuous stream of audio data and when retrieving
	    *  Track Match Position values. The fingerprint represents a
	    *  specific point in time of the audio stream as denoted by the audio provided when the fingerprint
	    *  is generated.
	    *  <p><b>Note:</b></p>
	    *  Do not design your application to submit only 3 seconds of audio data; the
	    *  application must submit audio data until GNSDK indicates it has received enough input.
	    *  You must use this fingerprint or its 6-second counterpart when generating results where match
	    *  position is required.
	    *  The usage of this type of fingerprint must be configured to your specific User ID, otherwise queries
	    *  of this type will not succeed.
	    */
	    kFingerprintTypeStream3 = 2,
	    
	    /**
	    *  Specifies a fingerprint used for identifying an ~6-second excerpt from and audio stream.
	    *  This is the same as kFingerprintTypeStream3 but requires more audio data to generate
	    *  but could be more accurate.
	    *  For additional notes see kFingerprintTypeStream3.
	    */
	    kFingerprintTypeStream6 = 3,
	    
	    /**
	    * @deprecated NB: This key has been marked as deprecated and will be removed from the next major release.
	    *      Use kFingerprintTypeFile instead.
	    */
	    kFingerprintTypeCMX = 4,
	    
	    /**
	    * @deprecated NB: This key has been marked as deprecated and will be removed from the next major release.
	    *      Use kFingerprintTypeStream3 or kFingerprintTypeStream6 instead.
	    */
	    kFingerprintTypeGNFPX =5
	    
	}

	public enum GnLicenseInputMode: int
	{
	    kLicenseInputModeInvalid = 0,
	    
	    /**
	    * Submit license content as string
	    */
	    kLicenseInputModeString = 1,
	    
	    /**
	    * Submit license content in file
	    */
	    kLicenseInputModeFilename = 2,
	    
	    /**
	    * Submit license content from stdin
	    */
	    kLicenseInputModeStandardIn = 3
	} 

	public enum GnMusicIdStreamPreset: int
	{
	    kPresetInvalid = 0,
	    
	    /** Application type mobile, i.e. audio is captured by microphone
	    * @ingroup Music_MusicIDStream_TypesEnums
	    */
	    kPresetMicrophone = 1,
	    
	    /** Application type radio, i.e. audio is captured at source (e.g. speaker)
	    * @ingroup Music_MusicIDStream_TypesEnums
	    */
	    kPresetRadio = 2
	}


	public enum GnMusicIdStreamProcessingStatus: int
	{
	    kStatusProcessingInvalid = 0,
	    
	    kStatusProcessingAudioNone = 1,
	    kStatusProcessingAudioSilence = 2,
	    kStatusProcessingAudioNoise = 3,
	    kStatusProcessingAudioSpeech = 4,
	    kStatusProcessingAudioMusic = 5,
	    
	    kStatusProcessingTransitionNone = 6,
	    kStatusProcessingTransitionChannelChange = 7,
	    kStatusProcessingTransitionContentToContent = 8,
	    
	    kStatusProcessingErrorNoClassifier = 9,
	    
	    kStatusProcessingAudioStarted = 10,
	    kStatusProcessingAudioEnded = 11
	}

	public enum GnLookupMode : int
	{
	    /**
	    * Invalid lookup mode
	    */
	    kLookupModeInvalid = 0,
	    
	    /**
	    * This mode forces the lookup to be done against the local database only. Local caches created from (online) query
	    * results are not queried in this mode.
	    * If no local database exists, the query will fail.
	    */
	    kLookupModeLocal = 1,
	    
	    /**
	    * This is the default lookup mode. If a cache exists, the query checks it first for a match.
	    * If a no match is found in the cache, then an online query is performed against Gracenote Service.
	    * If a result is found there, it is stored in the local cache.  If no online provider exists, the query will fail.
	    * The length of time before cache lookup query expires can be set via the user object.
	    */
	    kLookupModeOnline = 2,
	    
	    /**
	    * This mode forces the query to be done online only and will not perform a local cache lookup first.
	    * If no online provider exists, the query will fail. In this mode online queries and lists are not
	    * written to local storage, even if a storage provider has been initialize.
	    */
	    kLookupModeOnlineNoCache = 3,
	    
	    /**
	    * This mode forces the query to be done online only and will not perform a local cache lookup first.
	    * If no online provider exists, the query will fail. If a storage provider has been initialized,
	    * queries and lists are immediately written to local storage, but are never read unless the lookup mode is changed.
	    */
	    kLookupModeOnlineNoCacheRead = 4,
	    
	    /**
	    * This mode forces the query to be done against the online cache only and will not perform a network lookup.
	    * If no online provider exists, the query will fail.
	    */
	    kLookupModeOnlineCacheOnly = 5
	    
	}

	public enum GnMusicIdStreamIdentifyingStatus : int
	{
	    /** Invalid status
	    * @ingroup Music_MusicIDStream_TypesEnums
	    */
	    kStatusIdentifyingInvalid = 0,
	    
	    
	    /** Identification query started
	    * @ingroup Music_MusicIDStream_TypesEnums
	    */
	    kStatusIdentifyingStarted = 1,
	    
	    /** Fingerprint generated for sample audio
	    * @ingroup Music_MusicIDStream_TypesEnums
	    */
	    kStatusIdentifyingFpGenerated = 2,
	    
	    /** Local query started for identification
	    * @ingroup Music_MusicIDStream_TypesEnums
	    */
	    kStatusIdentifyingLocalQueryStarted = 3,
	    
	    /** Local query ended for identification
	    * @ingroup Music_MusicIDStream_TypesEnums
	    */
	    kStatusIdentifyingLocalQueryEnded = 4,
	    
	    /** Online query started for identification
	    * @ingroup Music_MusicIDStream_TypesEnums
	    */
	    kStatusIdentifyingOnlineQueryStarted = 5,
	    
	    /** Online query ended for identification
	    * @ingroup Music_MusicIDStream_TypesEnums
	    */
	    kStatusIdentifyingOnlineQueryEnded = 6,
	    
	    /** Identification query ended
	    * @ingroup Music_MusicIDStream_TypesEnums
	    */
	    kStatusIdentifyingEnded = 7,
	    
	    /** Identification query completed with existing match
	    * @ingroup Music_MusicIDStream_TypesEnums
	    */
	    kStatusIdentifyingNoNewResult = 8
	    
	}

	public enum GnLookupData : int
	{
	    /**
	    * Invalid lookup data
	    */
	    kLookupDataInvalid = 0,
	    
	    /**
	    * Indicates whether a response should include data for use in fetching content (like images).
	    * <p><b>Remarks:</b></p>
	    * An application's client ID must be entitled to retrieve this specialized data. Contact your
	    *	Gracenote representative with any questions about this enhanced
	    *	functionality.
	    */
	    kLookupDataContent = 1,
	    
	    /**
	    * Indicates whether a response should include any associated classical music data.
	    * <p><b>Remarks:</b></p>
	    * An application's license must be entitled to retrieve this specialized data. Contact your
	    * Gracenote representative with any questions about this enhanced functionality.
	    */
	    kLookupDataClassical = 2,
	    
	    /**
	    * Indicates whether a response should include any associated sonic attribute data.
	    * <p><b>Remarks:</b></p>
	    * An application's license must be entitled to retrieve this specialized data. Contact your
	    * Gracenote representative with any questions about this enhanced functionality.
	    */
	    kLookupDataSonicData = 3,
	    
	    /**
	    * Indicates whether a response should include associated attribute data for GNSDK Playlist.
	    * <p><b>Remarks:</b></p>
	    * An application's license must be entitled to retrieve this specialized data. Contact your
	    * Gracenote representative with any questions about this enhanced functionality.
	    */
	    kLookupDataPlaylist = 4,
	    
	    /**
	    * Indicates whether a response should include external IDs (third-party IDs).
	    * <p><b>Remarks:</b></p>
	    * External IDs are third-party IDs associated with the results (such as an Amazon ID),
	    *	configured specifically for your application.
	    * An application's client ID must be entitled to retrieve this specialized data. Contact your
	    * Gracenote representative with any questions about this enhanced functionality.
	    * External IDs can be retrieved from applicable query response objects.
	    */
	    kLookupDataExternalIds = 5,
	    
	    /**
	    * Indicates whether a response should include global IDs.
	    */
	    kLookupDataGlobalIds = 6,
	    
	    /**
	    * Indicates whether a response should include additional credits.
	    */
	    kLookupDataAdditionalCredits = 7,
	    
	    /**
	    * Indicates whether a response should include sortable data for names and titles
	    */
	    kLookupDataSortable = 8	    
	}

	public enum GnDataLevel: int
	{
	    kDataLevelInvalid = 0,
	    
	    kDataLevel_1      = 1,  /* least granular */
	    kDataLevel_2 = 2,
	    kDataLevel_3 = 3,
	    kDataLevel_4 =4           /* most granular */
	    
	}


	public enum GnContentType: int
	{
	    kContentTypeNull = 0,
	    kContentTypeUnknown = 1,
	    
	    kContentTypeImageCover = 2,
	    kContentTypeImageArtist = 3,
	    kContentTypeImageVideo = 4,
	    kContentTypeImageLogo = 5,
	    kContentTypeBiography =6,
	    kContentTypeReview = 7,
	    kContentTypeNews = 8,
	    kContentTypeArtistNews = 9,
	    kContentTypeListenerComments = 10,
	    kContentTypeReleaseComments = 11
	} 

	public enum GnLogPackageType: int
	{
	    kLogPackageInternal	=  1,
	    kLogPackageManager	=  2,
	    kLogPackageMusicID = 3,
	    kLogPackageMusicIDFile = 4,
	    kLogPackageLink	= 5,
	    kLogPackageVideoID = 6,
	    kLogPackageSubmit = 7,
	    kLogPackagePlaylist = 8,
	    kLogPackageStorageSqlite = 9,
	    kLogPackageDsp = 10,
	    kLogPackageMusicIdMatch	= 11,
	    kLogPackageAcr = 12,
	    kLogPackageLookupLocal = 13,
	    kLogPackageEDBInstall = 14,
	    kLogPackageEPG = 15,
	    kLogPackageMoodGrid = 16,
	    kLogPackageStorageQNX = 17,
	    kLogPackageLookupFPLocal = 18,
	    kLogPackageCorrelates = 19,
	    kLogPackageTaste = 20,
	    kLogPackageMusicIDStream = 21,
	    kLogPackageLookupLocalStream = 22,
	    kLogPackageRhythm = 23,
	    kLogPackageAllGNSDK = 24,
	    kLogPackageAll = 25
	} 

	public enum GnLocalStreamEngineType: int
	{
	    kLocalStreamEngineInvalid = 0,
	    
	    kLocalStreamEngineMMap = 1,
	    kLocalStreamEngineInMemory = 2
	}

	public enum GnLookupLocalStreamIngestStatus : int
	{
	    kIngestStatusInvalid = 0,
	    kIngestStatusItemBegin = 1,
	    kIngestStatusItemAdd = 2,
	    kIngestStatusItemDelete = 3
	} 

    public enum GnMusicIdFileInfoStatus : int
    {
        kMusicIdFileInfoStatusUnprocessed  = 0,

        kMusicIdFileInfoStatusProcessing   = 1,

        kMusicIdFileInfoStatusError      = 2,

        kMusicIdFileInfoStatusResultNone   = 3,

        kMusicIdFileInfoStatusResultSingle = 4,

        kMusicIdFileInfoStatusResultAll  = 5     
    } 

    public enum GnMusicIdFileCallbackStatus : int
    {
        kMusicIdFileCallbackStatusProcessingBegin    = 0x100,
 
        kMusicIdFileCallbackStatusFileInfoQuery      = 0x150,

        kMusicIdFileCallbackStatusProcessingComplete = 0x199,

        kMusicIdFileCallbackStatusProcessingError    = 0x299,

        kMusicIdFileCallbackStatusError              = 0x999
        
    }

    public enum GnMusicIdFileProcessType : int
    {
        kQueryReturnSingle = 1,
        kQueryReturnAll = 2
    } 

    public enum GnMusicIdFileResponseType : int
    {
        kResponseAlbums = 1,
        kResponseMatches = 2
        
    } 


    public enum GnThreadPriority : int
    {
        kThreadPriorityInvalid = 0,

        kThreadPriorityDefault = 1,
        
        kThreadPriorityIdle = 2,
        
        kThreadPriorityLow = 3,
        
        kThreadPriorityNormal = 4,
        
        kThreadPriorityHigh = 5
        
    } 


}

