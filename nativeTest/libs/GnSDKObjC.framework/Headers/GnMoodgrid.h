/*
 *
 *  GRACENOTE, INC. PROPRIETARY INFORMATION
 *  This software is supplied under the terms of a license agreement or
 *  nondisclosure agreement with Gracenote, Inc. and may not be copied
 *  or disclosed except in accordance with the terms of that agreement.
 *  Copyright(c) 2000-2015. Gracenote, Inc. All Rights Reserved.
 *
 */
 
#ifndef _GnMoodgrid_h_
#define _GnMoodgrid_h_


#import <Foundation/Foundation.h>
#import "GnDefines.h"

#import "GnEnums.h"
#import "GnMoodgridDataPoint.h"
#import "GnMoodgridPresentation.h"
#import "GnMoodgridProvider.h"
#import "GnUser.h"


@class GnMoodgridProviderEnumerator;


/**
* <b>Experimental</b>: GnMoodgrid
*/ 

@interface GnMoodgrid : NSObject

/**
* GnMoodgrid
*/
-(INSTANCE_RETURN_TYPE) init;

/**
* Version information for the library
* @return version
*/ 

+(NSString*) version;

/**
* Build Date for the library
* @return build date
*/ 

+(NSString*) buildDate;

/**
* Enumeration of all providers currently available for the moodgrid.
* @return iterable container of moodgrid providers.
*/ 

-(GnMoodgridProviderEnumerator*) providers;

/**
* Creates a Presentation that represents the type of moodgrid layout to  generate recommendations for. A presentation
* object is the way to access all Mood names and recommendations supported by its layout.
* @param user [in] : valid user
* @param type [in] : enum value representing the Presentation type .
* @param coordinate [in] : enum value representing the coordinate type for the presentation layout.
* @return presentation.
*/ 

-(GnMoodgridPresentation*) createPresentation: (GnUser*)user type: (GnMoodgridPresentationType)type coordinate: (GnMoodgridCoordinateType)coordinate error: (NSError**)error;

/**
* Retrieves a data point representing the dimensions of the presentation e.g. 5,5
* @return datapoint.
*/ 

-(GnMoodgridDataPoint*) dimensions: (GnMoodgridPresentationType)type error: (NSError**)error;


@end



#endif /* _GnMoodgrid_h_ */

