/*
 *
 *  GRACENOTE, INC. PROPRIETARY INFORMATION
 *  This software is supplied under the terms of a license agreement or
 *  nondisclosure agreement with Gracenote, Inc. and may not be copied
 *  or disclosed except in accordance with the terms of that agreement.
 *  Copyright(c) 2000-2015. Gracenote, Inc. All Rights Reserved.
 *
 */
 
#ifndef _GnMusicIdStreamOptions_h_
#define _GnMusicIdStreamOptions_h_


#import <Foundation/Foundation.h>
#import "GnDefines.h"

#import "GnEnums.h"




/**
* Configures options for GnMusicIdStream
*/ 

@interface GnMusicIdStreamOptions : NSObject

/**
*  Specifies whether identification should be performed against local embedded databases or online.
*  @param lookupMode  [in] One of the GnLookupMode values
*  @ingroup MusicIdStream_Options
*/ 

-(void) lookupMode: (GnLookupMode)lookupMode error: (NSError**)error;

/**
*  Specifies which data should be included in the response
*  @param val 		[in] Set One of the GnLookupData values
*  @param enable 	[in] True or false to enable or disable
*  @ingroup MusicIdStream_Options
*/ 

-(void) lookupData: (GnLookupData)val enable: (BOOL)enable error: (NSError**)error;

/**
*  @deprecated To set language provide appropriate GnLocale object with GnMusicIdStream constructor
*   @param preferredLanguage		[in] preferred language for result
*  @ingroup MusicIdStream_Options
*/ 

-(void) preferResultLanguage: (GnLanguage)preferredLanguage error: (NSError**)error;

/**
* Specifies preference for results that contain the provided external identifier
* <p><b>Remarks:</b></p>
* This option is currently only supported when online processing is enabled and single
* result is specified.
*  @param preferredExternalId 	[in] The name of an external identifier that should be preferred when selecting matches
*  @ingroup MusicIdStream_Options
*/ 

-(void) preferResultExternalId: (NSString*)preferredExternalId error: (NSError**)error;

/**
*  Specifies preference for results that have cover art associated
*  @param bEnable 	[in] Set prefer cover art.
*  @ingroup MusicIdStream_Options
*/ 

-(void) preferResultCoverart: (BOOL)bEnable error: (NSError**)error;

/**
*  Specifies whether a response must return only the single best result.
*  When enabled a single full result is returned, when disabled multiple partial results may be returned.
*  @param bEnable 	[in] Option, default is true. True to enable, false to disable.
*  @ingroup MusicIdStream_Options
*/ 

-(void) resultSingle: (BOOL)bEnable error: (NSError**)error;

/**
*  Specifies whether a response must return a range of results that begin at the specified values
*  @param resultStart  [in] Result range start value
*  <p><b>Remarks:</b></p>
*  This Option is useful for paging through results.
*  <p><b>Note:</b></p>
*  Gracenote Service enforces that the range start value must be less than or equal to the total
*  number of results. If you specify a range start value that is greater than the total number of
*  results, no results are returned.
*  @ingroup MusicIdStream_Options
*/ 

-(void) resultRangeStart: (NSUInteger)resultStart error: (NSError**)error;

/**
* Specifies the number of results to return in the response
*  @param resultCount 	[in] Number of results
*  @ingroup MusicIdStream_Options
*/ 

-(void) resultCount: (NSUInteger)resultCount error: (NSError**)error;

/**
* Set a specific network interface to use with this object's connections. This can be useful for
* systems with multiple network interaces. Otherwise, the operating system determines the interface to use.
*  @param intfName [in] Local IP address or system name for the desired network interface
*  @ingroup MusicIdStream_Options
*/ 

-(void) networkInterfaceWithIntfName: (NSString*)intfName error: (NSError**)error;

/**
* Return network interface being use with this object's connections if one has been set. 
* If no specific network interface has been set this option will return an empty string.
*  @ingroup MusicIdStream_Options
*/ 

-(NSString*) networkInterface:(NSError**) error;

/**
*  General option setting for custom string options
*  @param optionKey   [in] Option name
*  @param value	   [in] Option value
*  @ingroup MusicIdStream_Options
*/ 

-(void) custom: (NSString*)optionKey value: (NSString*)value error: (NSError**)error;


@end



#endif /* _GnMusicIdStreamOptions_h_ */

