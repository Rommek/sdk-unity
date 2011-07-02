//
//  PHUnityIntegration.mm
//  Unity-iPhone
//
//  Created by Jesus Fernandez on 6/30/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "PHUnityIntegration.h"
#import "PHPublisherMetadataRequest.h"
#import "PHAPIRequest.h"
#import "JSON.h"

#pragma mark - Unity Externs
extern void UnityPause(bool pause);
extern void UnitySendMessage(const char *obj, const char *method, const char *msg);

#pragma mark -

static PHUnityIntegration *sharedIntegration;

@implementation PHUnityIntegration
+(PHUnityIntegration *)sharedIntegration{
    if (sharedIntegration == nil) {
        sharedIntegration = [PHUnityIntegration new];
    }
    
    return sharedIntegration;
}

-(void)request:(PHAPIRequest *)request didSucceedWithResponse:(NSDictionary *)responseData{
    UnityPause(false);
    NSDictionary *messageDictionary = [NSDictionary dictionaryWithObjectsAndKeys:
                                       [NSNumber numberWithInt:request.hashCode],@"hash",
                                       @"success", @"name",
                                       responseData,@"data", 
                                       nil];
    NSString *messageJSON = [messageDictionary JSONRepresentation];
    UnitySendMessage("PlayHavenManager", "HandleNativeMethod", [messageJSON cStringUsingEncoding:NSUTF8StringEncoding]);
}

-(void)request:(PHAPIRequest *)request didFailWithError:(NSError *)error{
    UnityPause(false);
    NSDictionary *messageDictionary = [NSDictionary dictionaryWithObjectsAndKeys:
                                       [NSNumber numberWithInt:request.hashCode],@"hash",
                                       @"error", @"name",
                                       [NSDictionary dictionary],@"data", 
                                       nil];
    NSString *messageJSON = [messageDictionary JSONRepresentation];
    UnitySendMessage("PlayHavenManager", "HandleNativeEvent", [messageJSON cStringUsingEncoding:NSUTF8StringEncoding]);
    
}

-(void)request:(PHPublisherContentRequest *)request contentDidFailWithError:(NSError *)error{
    UnityPause(false);
    NSDictionary *messageDictionary = [NSDictionary dictionaryWithObjectsAndKeys:
                                       [NSNumber numberWithInt:request.hashCode],@"hash",
                                       @"error", @"name",
                                       [NSDictionary dictionary],@"data", 
                                       nil];
    NSString *messageJSON = [messageDictionary JSONRepresentation];
    UnitySendMessage("PlayHavenManager", "HandleNativeEvent", [messageJSON cStringUsingEncoding:NSUTF8StringEncoding]);
}

-(void)requestContentDidDismiss:(PHPublisherContentRequest *)request{
    UnityPause(false);
    NSDictionary *messageDictionary = [NSDictionary dictionaryWithObjectsAndKeys:
                                       [NSNumber numberWithInt:request.hashCode],@"hash",
                                       @"dismiss", @"name",
                                       [NSDictionary dictionary],@"data", 
                                       nil];
    NSString *messageJSON = [messageDictionary JSONRepresentation];
    UnitySendMessage("PlayHavenManager", "HandleNativeEvent", [messageJSON cStringUsingEncoding:NSUTF8StringEncoding]);  
}

                

@end

NSString* CreateNSString(const char* string){
    if (string) {
        return [NSString stringWithUTF8String:string];
    } else {
        return @"";
    }
}

extern "C" {
    void _PlayHavenOpenRequest(const int hash, const char* token, const char* secret){
        PHPublisherOpenRequest *request = [PHPublisherOpenRequest 
                                           requestForApp:CreateNSString(token)
                                           secret:CreateNSString(secret)];
        request.delegate = [PHUnityIntegration sharedIntegration];
        request.hashCode = hash;
        [request send];
    }
    
    void _PlayHavenMetadataRequest(const int hash, const char* token, const char* secret, const char* placement){
        PHPublisherMetadataRequest *request = [PHPublisherMetadataRequest 
                                               requestForApp:CreateNSString(token)
                                               secret:CreateNSString(secret)
                                               placement:CreateNSString(placement) 
                                               delegate:[PHUnityIntegration sharedIntegration]];
        request.hashCode = hash;
        [request send];
    }
    
    void _PlayHavenContentRequest(const int hash, const char* token, const char* secret, const char* placement){
        PHPublisherContentRequest *request = [PHPublisherContentRequest 
                                               requestForApp:CreateNSString(token)
                                               secret:CreateNSString(secret)
                                               placement:CreateNSString(placement) 
                                               delegate:[PHUnityIntegration sharedIntegration]];
        request.hashCode = hash;
        [request send];
        
        UnityPause(true);
    }
}
