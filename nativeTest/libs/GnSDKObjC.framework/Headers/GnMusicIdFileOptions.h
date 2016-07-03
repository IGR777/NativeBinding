/*
 *
 *  GRACENOTE, INC. PROPRIETARY INFORMATION
 *  This software is supplied under the terms of a license agreement or
 *  nondisclosure agreement with Gracenote, Inc. and may not be copied
 *  or disclosed except in accordance with the terms of that agreement.
 *  Copyright(c) 2000-2015. Gracenote, Inc. All Rights Reserved.
 *
 */
 
#ifndef _GnMusicIdFileOptions_h_
#define _GnMusicIdFileOptions_h_


#import <Foundation/Foundation.h>
#import "GnDefines.h"

#import "GnEnums.h"




/**
* Configures options for GnMusicIdFile
*/ 

@interface GnMusicIdFileOptions : NSObject

/**
*  Indicates whether the MusicID-File query should be performed against local embedded databases or online.
*  @param lookupMode		[in] One of the GnLookupMode values
*  @ingroup MusicIdFile_Options
*/ 

-(void) lookupMode: (GnLookupMode)lookupMode error: (NSError**)error;

/**
*  Sets the lookup data value for the MusicID-File query.
*  @param val 				[in] Set One of the GnLookupData values
*  @param enable 			[in] True or false to enable or disable
*  @ingroup MusicIdFile_Options
*/ 

-(void) lookupData: (GnLookupData)val enable: (BOOL)enable error: (NSError**)error;

/**
*  Sets the batch size for the MusicID-File query.
*  @param size				[in] set String value or one of MusicID-File Option Values that corresponds to BATCH_SIZE
*  <p><b>Remarks:</b></p>
*  The option value provided for batch size must be greater than zero (0).
*  @ingroup MusicIdFile_Options
*/ 

-(void) batchSize: (NSUInteger)size error: (NSError**)error;

/**
*  Indicates whether MusicID-File should Process the responses Online, this may reduce the amount of 
*  resources used by the client. Online processing must be allowed by your license.
*  @param enable			[in] True or false to enable or disable
*  @ingroup MusicIdFile_Options
*/ 

-(void) onlineProcessing: (BOOL)enable error: (NSError**)error;

/**
*  Sets the preferred language for the MusicID-File query.
*  @param preferredLangauge	[in] One of the GNSDK language values
*  @ingroup MusicIdFile_Options
*/ 

-(void) preferResultLanguage: (GnLanguage)preferredLangauge error: (NSError**)error;

/**
* Use this option to specify an external identifier which MusicID-File should try to include in any responses that are returned.
*  <p><b>Remarks:</b></p>
* This option is currently only supported when online processing is enabled.
*  @param preferredExternalId	[in] The name of an external identifier that should be preferred when selecting matches
*  @ingroup MusicIdFile_Options
*/ 

-(void) preferResultExternalId: (NSString*)preferredExternalId error: (NSError**)error;

/**
*  Sets the thread priority for a given MusicID-File query.
*  @param threadPriority 	[in] Set one of GnThreadPriority values that corresponds to thread priority
*  <p><b>Remarks:</b></p>
*  The option value provided for thread priority must be one of the defined
*  GnThreadPriority values.
*  @ingroup MusicIdFile_Options
*/ 

-(void) threadPriority: (GnThreadPriority)threadPriority error: (NSError**)error;

/**
* Set a specific network interface to use with this object's connections. This can be useful for
* systems with multiple network interaces. Otherwise, the operating system determines the interface to use.
*  @param intfName [in] Local IP address or system name for the desired network interface
*  @ingroup MusicIdFile_Options
*/ 

-(void) networkInterfaceWithIntfName: (NSString*)intfName error: (NSError**)error;

/**
* Return network interface being use with this object's connections if one has been set. 
* If no specific network interface has been set this option will return an empty string.
*  @ingroup MusicIdFile_Options
*/ 

-(NSString*) networkInterface:(NSError**) error;

/**
*  General option setting for custom options
*  @param optionKey		[in] set One of the MusicID-File Option Keys
*  @param enable			[in] set True or false to enable or disable
*  @ingroup MusicIdFile_Options
*/ 

-(void) customWithOptionKey: (NSString*)optionKey enable: (BOOL)enable error: (NSError**)error;

/**
*  Set option using option name
*  @param option			[in] Option name
*  @param value			[in] Option value
*  @ingroup MusicIdFile_Options
*/ 

-(void) customWithOption: (NSString*)option value: (NSString*)value error: (NSError**)error;


@end



#endif /* _GnMusicIdFileOptions_h_ */

