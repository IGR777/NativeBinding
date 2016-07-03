/*
 *
 *  GRACENOTE, INC. PROPRIETARY INFORMATION
 *  This software is supplied under the terms of a license agreement or
 *  nondisclosure agreement with Gracenote, Inc. and may not be copied
 *  or disclosed except in accordance with the terms of that agreement.
 *  Copyright(c) 2000-2015. Gracenote, Inc. All Rights Reserved.
 *
 */
 
#ifndef _GnMusicIdOptions_h_
#define _GnMusicIdOptions_h_


#import <Foundation/Foundation.h>
#import "GnDefines.h"

#import "GnEnums.h"




/**
* \class GnMusicIdOptions
* Configures options for GnMusicId
*/ 

@interface GnMusicIdOptions : NSObject

/**
*  Indicates whether the MusicID query should be performed against local embedded databases or online.
*  @param lookupMode  [in] One of the GnLookupMode values
*  @ingroup MusicId_Options
*/ 

-(void) lookupMode: (GnLookupMode)lookupMode error: (NSError**)error;

/**
*  Indicates the lookup data value for the MusicID query.
*  @param lookupData [in] One of the GnLookupData values
*  @param bEnable    [in] Set lookup data
*  @ingroup MusicId_Options
*/ 

-(void) lookupData: (GnLookupData)lookupData bEnable: (BOOL)bEnable error: (NSError**)error;

/**
*  Indicates the preferred language of the returned results.
*  @param preferredLanguage [in] One of the GNSDK language values
*  @ingroup MusicId_Options
*/ 

-(void) preferResultLanguage: (GnLanguage)preferredLanguage error: (NSError**)error;

/**
*  Indicates the preferred external ID of the returned results.
*  Only available where single result is also requested.
*  @param strExternalId [in] Gracenote external ID source name
*  @ingroup MusicId_Options
*/ 

-(void) preferResultExternalId: (NSString*)strExternalId error: (NSError**)error;

/**
*  Indicates using cover art to prefer the returned results.
*  @param bEnable [in] Set prefer cover art
*  @ingroup MusicId_Options
*/ 

-(void) preferResultCoverart: (BOOL)bEnable error: (NSError**)error;

/**
*  Indicates whether a response must return only the single best result.
*  When enabled a single full result is returned, when disabled multiple partial results may be returned.
*  @param bEnable [in] Set single result
*  <p><b>Remarks:</b></p>
*  If enabled, the MusicID library selects the single best result based on the query type and input.
*  @ingroup MusicId_Options
*/ 

-(void) resultSingle: (BOOL)bEnable error: (NSError**)error;

/**
*  Enables or disables revision check option.
*  @param bEnable [in] Set revision check
*
* Long Running Potential: Network I/O, File system I/O (for online query cache or local lookup)
*  @ingroup MusicId_Options
*/ 

-(void) revisionCheck: (BOOL)bEnable error: (NSError**)error;

/**
*  Specfies whether a response must return a range of results that begin and count at a specified values.
*  @param resultStart  [in] Result range start value
*  <p><b>Remarks:</b></p>
*  This Option is useful for paging through results.
*  <p><b>Note:</b></p>
*  Gracenote Service enforces that the range start value must be less than or equal to the total
*  number of results. If you specify a range start value that is greater than the total number of
*  results, no results are returned.
*  @ingroup MusicId_Options
*/ 

-(void) resultRangeStart: (NSUInteger)resultStart error: (NSError**)error;

/**
*  Specfies maximum number of results to return.
*  @param resultCount  [in] Result maximum value
*  <p><b>Note:</b></p>
*  Gracenote Service enforces its own maximum number of results that can vary over time and query types. 
*  This setting sets a maximum that is up to the Service maximums.
*  @ingroup MusicId_Options
*/ 

-(void) resultCount: (NSUInteger)resultCount error: (NSError**)error;

/**
* Set a specific network interface to use with this object's connections. This can be useful for
* systems with multiple network interaces. Otherwise, the operating system determines the interface to use.
*  @param intfName [in] Local IP address or system name for the desired network interface
*  @ingroup MusicId_Options
*/ 

-(void) networkInterfaceWithIntfName: (NSString*)intfName error: (NSError**)error;

/**
* Return network interface being use with this object's connections if one has been set. 
* If no specific network interface has been set this option will return an empty string.
*  @ingroup MusicId_Options
*/ 

-(NSString*) networkInterface:(NSError**) error;

/**
*  Set option using option name
*  @param option   [in] Option name
*  @param value	[in] Option value
*  @ingroup MusicId_Options
*/ 

-(void) customWithOption: (NSString*)option value: (NSString*)value error: (NSError**)error;

/**
*  Set option using option name
*  @param option   [in] Option name
*  @param bEnable	[in] Option enable true/false
*  @ingroup MusicId_Options
*/ 

-(void) customWithOption: (NSString*)option bEnable: (BOOL)bEnable error: (NSError**)error;


@end



#endif /* _GnMusicIdOptions_h_ */

