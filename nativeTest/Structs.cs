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
}

