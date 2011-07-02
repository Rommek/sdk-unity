using UnityEngine;
using System.Collections;

public class TestSceneManager : MonoBehaviour {	
	public PlayHavenNotifierView mNotifierView;
	
	void OnGUI(){
		float yPos = 20.0f;
		float xPos = 20.0f;
		float width = 280;
		
		GUI.depth = 2;
		
		if( GUI.Button( new Rect( xPos, yPos, width, 40 ), "Open" ) ){
			PlayHaven.OpenRequest request = new PlayHaven.OpenRequest("token","secret");
			request.Send();
		}
				
		if( GUI.Button( new Rect( xPos, yPos += 50, width, 40 ), "Content" ) ){
			PlayHaven.ContentRequest request = new PlayHaven.ContentRequest("token","secret","more_games");
			request.Send();
			
			if(mNotifierView != null) mNotifierView.Clear();
		}
	}  	
}
