using UnityEngine;
using System.Collections;
using LitJson;

public class TestSceneManager : MonoBehaviour {	
	public PlayHavenNotifierView mNotifierView;
	public GameObject mGameObject;
	PlayHaven.ContentRequest request;
	PlayHaven.DismissHandler HandlePlayHavenDismiss;
	PlayHaven.ErrorHandler HandlePlayHavenError;
	
	void NewRequest(string placement_id){
		if (HandlePlayHavenDismiss == null)
			HandlePlayHavenDismiss = new PlayHaven.DismissHandler(this.HandleDismiss);
		
		if (HandlePlayHavenError == null)
			HandlePlayHavenError = new PlayHaven.ErrorHandler(this.HandleError);
		
		
		if (request == null){
			request = new PlayHaven.ContentRequest("zombie1","haven1",placement_id);
			request.OnDismiss += HandlePlayHavenDismiss;
			request.OnError += HandlePlayHavenError;
			request.Send();

			if(mNotifierView != null) mNotifierView.Clear();
		}
	}
	
	public void HandleDismiss(){
		Debug.Log("handled dismiss");
		
		if(request != null) {
            request.OnDismiss -= HandlePlayHavenDismiss;
			request.OnError -= HandlePlayHavenError;
            request = null;
		}
		
		mGameObject.transform.Rotate(mGameObject.transform.eulerAngles.x, mGameObject.transform.eulerAngles.y, mGameObject.transform.eulerAngles.z + 15);
	}
	
	public void HandleError(JsonData errorData){
		Debug.Log("handled error");
		
		if(request != null) {
            request.OnDismiss -= HandlePlayHavenDismiss;
			request.OnError -= HandlePlayHavenError;
            request = null;
		}
    }
	
	void OnGUI(){
		float yPos = 20.0f;
		float xPos = 20.0f;
		float width = 280;
		
		GUI.depth = 2;
		
		if( GUI.Button( new Rect( xPos, yPos, width, 40 ), "Open" ) ){
			PlayHaven.OpenRequest request = new PlayHaven.OpenRequest("token","secret");
			request.Send();
		}
				
		if( GUI.Button( new Rect( xPos, yPos += 50, width, 40 ), "More Games" ) ){
			NewRequest("more_games");
		}
		
		if( GUI.Button( new Rect( xPos, yPos += 50, width, 40 ), "Achievement A" ) ){
			NewRequest("achievement_a");
		}
		
		if( GUI.Button( new Rect( xPos, yPos += 50, width, 40 ), "Level Complete" ) ){
			NewRequest("level_complete");
		}
	}
	
}