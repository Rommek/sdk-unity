//
//  PHUnityIntegration.h
//  Unity-iPhone
//
//  Created by Jesus Fernandez on 6/30/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "PlayHavenSDK.h"

@interface PHUnityIntegration : NSObject<PHAPIRequestDelegate, PHPublisherContentRequestDelegate>
+(PHUnityIntegration *)sharedIntegration;
@end
