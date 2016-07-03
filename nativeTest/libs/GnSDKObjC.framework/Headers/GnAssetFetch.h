/*
 *
 *  GRACENOTE, INC. PROPRIETARY INFORMATION
 *  This software is supplied under the terms of a license agreement or
 *  nondisclosure agreement with Gracenote, Inc. and may not be copied
 *  or disclosed except in accordance with the terms of that agreement.
 *  Copyright(c) 2000-2015. Gracenote, Inc. All Rights Reserved.
 *
 */
 
#ifndef _GnAssetFetch_h_
#define _GnAssetFetch_h_


#import <Foundation/Foundation.h>
#import "GnDefines.h"

#import "GnCancellableDelegate.h"
#import "GnEnums.h"
#import "GnStatusEventsDelegate.h"
#import "GnUser.h"




/**
* <b>Experimental</b>: Fetch asset raw data. 
* The raw data is fetched during object construction
* and may result in lengthy network access if accessing
* from online.
*/ 

@interface GnAssetFetch : NSObject

/**
* Perform the fetch of the data from the provided URL. 
* This method can fetch data from any valid URL. This class must
* be used to fetch data from a GNSDK local URL.
* Construction may result in lengthy network access if the URL is
* online and the target data is large.
*/ 

-(INSTANCE_RETURN_TYPE) initWithGnUser: (GnUser*)user url: (NSString*)url statusEventsDelegate: (id <GnStatusEventsDelegate>)pEventHandler;

/**
* Raw data buffer
*/ 

-(const unsigned char*) data;

/**
* Raw data size in bytes
*/ 

-(NSUInteger) size;


@end



#endif /* _GnAssetFetch_h_ */

