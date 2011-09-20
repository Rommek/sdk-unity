//
//  PHUnityIntegration.h
//  Unity-iPhone
//
//  Created by Jesus Fernandez on 6/30/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "PlayHavenSDK.h"
@class SBJsonWriter;
@interface PHUnityIntegration : NSObject<PHAPIRequestDelegate, PHPublisherContentRequestDelegate>{
  SBJsonWriter *_writer;
}
+(PHUnityIntegration *)sharedIntegration;

@property (nonatomic, readonly) SBJsonWriter *writer;
@end
