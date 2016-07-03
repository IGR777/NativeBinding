/*
 *
 *  GRACENOTE, INC. PROPRIETARY INFORMATION
 *  This software is supplied under the terms of a license agreement or
 *  nondisclosure agreement with Gracenote, Inc. and may not be copied
 *  or disclosed except in accordance with the terms of that agreement.
 *  Copyright(c) 2000-2015. Gracenote, Inc. All Rights Reserved.
 *
 */
 
#ifndef _GnMoodgridPresentation_h_
#define _GnMoodgridPresentation_h_


#import <Foundation/Foundation.h>
#import "GnDefines.h"

#import "GnEnums.h"
#import "GnMoodgridDataPoint.h"
#import "GnMoodgridProvider.h"
#import "GnMoodgridResult.h"


@class GnMoodgridDataPointEnumerator;


/**
** <b>Experimental</b>: GnMoodgridPresentation
*/ 

@interface GnMoodgridPresentation : NSObject

-(INSTANCE_RETURN_TYPE) init __attribute__((unavailable("init not available, use initWith instead")));

/**
* Retrieves an iterator to access the Mood data points in this presentation.
* @return moods
*/ 

-(GnMoodgridDataPointEnumerator*) moods:(NSError**) error;

/**
* Retrieves the presentation type the defines the no. of moods available in this presentation..
* @return moods
*/ 

-(GnMoodgridPresentationType) layoutType:(NSError**) error;

/**
*  Retrieves the coordinate type that defines the layout of the presentation.
* @return moodgridcoordinatetype.
*/ 

-(GnMoodgridCoordinateType) coordinateType;

/**
* Adds a filter to the presentation for the inclusion of a list type to include or exclude from the recommendations.
* @param uniqueIdentifier [in] : unique identifier for the presentation representing this filter.
* @param elistType [in] : list type
* @param strValueId [in] : list value that is to be operated upon.
* @param eConditionType [in]: filter condition
*/ 

-(void) addFilter: (NSString*)uniqueIdentifier elistType: (GnMoodgridFilterListType)elistType strValueId: (NSString*)strValueId eConditionType: (GnMoodgridFilterConditionType)eConditionType error: (NSError**)error;

/**
* Removes a filter from the presentation represented by the unique identifier.
* @param uniqueIdentifier [in] : identifier that represents the filter to be removed.
*/ 

-(void) removeFilter: (NSString*)uniqueIdentifier error: (NSError**)error;

/**
* Removes all filters from the presentation
*/ 

-(void) removeAllFilters:(NSError**) error;

/**
* Retrieves a mood name as defined by the locale for a given data point in the presentation.
* @param position [in] : data position
* @return moodname
*/ 

-(NSString*) moodName: (GnMoodgridDataPoint*)position error: (NSError**)error;

/**
* Retrieves a mood id for the given data point in the presentation.
* @param position [in] : data position
* @return moodid.
*/ 

-(NSString*) moodId: (GnMoodgridDataPoint*)position error: (NSError**)error;

/**
* Generates recommendations for a given mood data point and provider. The reccomentations are represented by a
* GnMoodgridResult.
* @param provider [in] : moodgrid provider that the results must come from.
* @param position [in] : data point that represents the mood for which reccomendation are requested.
* @return GnMoodgridResult
*/ 

-(GnMoodgridResult*) findRecommendations: (GnMoodgridProvider*)provider position: (GnMoodgridDataPoint*)position error: (NSError**)error;

/**
* Generates a recommendations estimate for a given mood data point and provider. The estimate is dependent on the
* provider. Use this functionality for creating a heat map of all the moods supported in the presentation.
* @param provider [in] :moodgrid provider that the estimate must come from.
* @param position [in] : data point that represents the mood for which the estimate is requested.
* @return count representing the estimate.
*/ 

-(NSUInteger) findRecommendationsEstimate: (GnMoodgridProvider*)provider position: (GnMoodgridDataPoint*)position error: (NSError**)error;


@end



#endif /* _GnMoodgridPresentation_h_ */

