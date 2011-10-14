PlayHaven SDK for Unity3D
=========================
PlayHaven is a real-time mobile game marketing platform to help you take control of the business of your games.
Acquire, retain, re-engage, and monetize your players with the help of PlayHaven's powerful marketing platform. Integrate once and embrace the flexibility of the web as you build, schedule, deploy, and analyze your in-game promotions and monetization in real-time through PlayHaven's easy-to-use, web-based dashboard.
An API token and secret is required to use this SDK. These tokens uniquely identify your app to PlayHaven and prevent others from making requests to the API on your behalf. To get a token and secret, please visit the PlayHaven developer dashboard at https://dashboard.playhaven.com

Integration
-----------
    NOTE: For compatibility with XCode 4, we recommend a manual integration. We understand it's a less than ideal workflow, and we will implement automatic integration as soon as the required functionality is available in XCode 4.

### Pre-Integration

1. This SDK is built on our iPhone SDK, if you cloned this repository from github, make sure to initalize and update submodules to get the latest compatible version of the PlayHaven SDK for iOS. From the directory you cloned:

	git submodule init
	git submodule update

Otherwise, if you downloaded this repository as a .zip file from github, you may also need to download sdk-ios version 1.3.6. It's available at https://github.com/playhaven/sdk-ios/zipball/1.3.6

1. Import PlayHavenSDK.unitypackage into your project. This will create the following folders in your project:
	* _Plugins/PlayHaven_
	* _Examples/PlayHaven_

1. In your iOS player settings (accessible from File>Build Settings...) make sure the value of "Other Settings > Optimization > SDK Version is set to an appropriate device version. _Once you integrate a native code plugin like the PlayHaven SDK, you may not be able to build your app for the iOS Simulator_

1. Build (but don't Build & Run) your app from the Build Settings... window. Open the resulting XCode project.

### Post-XCode build

    NOTE: You'll need to do the following the first time the project is built as well as each time you choose to "Replace" an existing XCode project from Unity.

### Add PlayHaven SDK files
1. From your project's Project Navigator, right click on the _Classes_ folder and select _Add Files to "Unity-iPhone"..._.

1. In the file dialog that appears, navigate to the sdk-ios directory inside the repository you have checked out and select the JSON and src directories as well as PlayHaven.bundle. Leave the checkbox next to _Copy items into destination group's folder (if needed)_ unchecked. Leave the radio button next to _Create groups for any added folders_ selected. Ensure that the _Unity-iPhone_ target is selected. Click _Add_.

### Add Unity Integration files

1. From your project's Project Navigator, right click on the _Classes_ folder and select _Add Files to "Unity-iPhone"..._.

1. In the file dialog that appears, navigate to the top level of the repository you have checked out and select both PHUnityIntegration.mm and PHUnityIntegration.h. Leave the checkbox next to _Copy items into destination group's folder (if needed)_ unchecked. Leave the radio button next to _Create groups for any added folders_ selected. Ensure that the _Unity-iPhone_ target is selected. Click _Add_.

Adding a Cross-Promotion Widget to Your Game
--------------------------------------------
Each game is pre-configured for our Cross-Promotion Widget, which will give your game the ability to deliver quality game recommendations to your users. To integrate the Cross-Promotion Widget, you'll need to do the following:

### Record game opens
In order to better optimize your content units, it is necessary for your app to report all game opens. This allows you to measure the click-through rate of your Cross-Promotion Widget to help optimize the performance of your implementation. This request is asynchronous and may run in the background while your game is loading.

Inside of a script that is run when your game is launched, use the following code to record a game open:

    PlayHaven.OpenRequest request = new PlayHaven.OpenRequest("MYTOKEN","MYSECRET");
    request.Send();

Where MYTOKEN and MYSECRET are the token and secret for your game. That's it!
See "Recording game opens" for more information about recording game opens.

### Request the Cross-Promotion Widget
We recommend adding the Cross-Promotion Widget to an attractive "More Games" button in a prominent part of your game's UI. The most popular place to add this button is in the main menu, but we have seen great results from buttons on game over or level complete screens as well. Be creative and find places in your game where it is natural for users to want to jump to a new game.

Inside your button's event handler, use the following code to request the pre-configured Cross-Promotion Widget:

    PlayHaven.ContentRequest request = new PlayHaven.ContentRequest("MYTOKEN","MYSECRET","more_games");
    request.Send();

Add OnDismiss and OnError event handlers to the request instance if you would like to know when the Cross-Promotion Widget has loaded or dismissed. See "Requesting content for your placements" for more information about these delegate methods as well as other things you can do with PlayHaven.ContentRequest.

### Add a Notification View (Notifier Badge)
Adding a notification view to your "More Games" button will greatly increase the number of Cross-Promotion Widget opens for your game, by up to 300%. Add the PlayHavenNotifierView script (_Plugins/PlayHaven/PlayHavenNotifierView_) to an object in your scene.

You may set the editable properties _Token_ and _Secret_ to your game's token and secret. Set _Placement_ to "more\_games". Set _XPos_ and _YPos_ to the screen coordinates you would like to use as the notifier's center point. See "Displaying Notification Views" for more information about customizing the presentation of your PlayHavenNotifierView.

Making Requests and Displaying Content
--------------------------------------
### Integration
In order to receive callbacks from native code, you will need to create an empty game object in your scene that contains the PlayHavenManager script (_Plugins/PlayHaven/PlayHavenManager_).
### Recording game opens
In order to better optimize your campaigns, it is necessary for your app to report all game opens. This will allow us to calculate impression rates based on all game opens. You do not have to register any event handlers for this request.

    PlayHaven.OpenRequest request = new PlayHaven.OpenRequest("token","secret");
    request.Send();

### Requesting content for your placements
You may request content for your app using your API token, secret, as well as a placement_id to identify the placement you are requesting content for. Our Unity integration will automatically pause and unpause Unity for you while content is active. You will need to implement two event handlers to handle successful dismisses or errors.

    PlayHaven.ContentRequest request = new PlayHaven.ContentRequest("token","secret","placement");
    request.Send();

*NOTE:* You may set up placement_ids through the PlayHaven Developer Dashboard.

#### Controlling overlay behavior
A transparent black overlay separates content units from your game. It will block touches from reaching your UI and provide a way for users to cancel out of a request if it is taking too long.
By default, the overlay appears after the content request returns successfully. However, if you wish to show the overlay immediately after sending a request, you may add a parameter to request.Send(), like so:

    request.Send(true); //shows overlay immediately after sending

#### Content view dismissing
The content has successfully dismissed and control is being returned to your app. This can happen as a result of the user clicking on the close button or clicking on a link that will open outside of the app. The game will have already been unpaused at this point.

    request.OnDismiss += new PlayHaven.DismissHandler(this.HandleDismiss);
    public void HandleDismiss(){
        //Your dismiss implementation
    }

#### Content view failing to load
If for any reason a content unit fails to load, the request will stop and the overlay view (if present) will be removed. The game will have already been unpaused at this point.

    request.OnError += new PlayHaven.ErrorHandler(this.HandleError);
    public void HandleError(JsonData errorData){
    	//Your dismiss implementation
    }
    
#### Content view unlocks a reward
PlayHaven allows you to reward users with virtual currency, in-game items, or any other content within your game. Information about these rewards is passed back to your game through the OnReward event.

	request.OnReward += new PlayHaven.RewardHandler(this.HandleReward);
	public void HandleReward(JsonData rewardData){
		rewardData["name"];     //name of the reward configured on the dashboard
		rewardData["quantity"]; //if there is a quantity associated, it will be an integer value
		rewardData["receipt"];  //unique identifier used to identify duplicate reward unlocks
	}


Displaying Notifier Views
-------------------------
### Integration
The PlayHavenNotifierView script automatically retrieves placement metadata and displays a notification view if needed. Add the PlayHavenNotifierView script (_Plugins/PlayHaven/PlayHavenNotifierView_) to an object in your scene. While inside the editor, the notification view will display a default notification view that will help you position the view on your screen.
 
### Configuration
The PlayHavenNotifierView script exposes the following editable properties when selected in the editor:

* _Token_: Your publisher token
* _Secret_: Your publisher secret
* _Placement_: A placement id.
* _XPos_, _YPos_: Screen position to display the notifier badge.

*NOTE:* You may set up placement_ids through the PlayHaven Developer Dashboard.

### Customization
PlayHavenNotifierView also exposes a GUIStyle through the MGUIStyle property which is used to customize the appearance of the GUILabel instance that is created when notification view data is returned from the placement metadata call. To learn more on how to customize with GUIStyle, refer to the Unity documentation.
