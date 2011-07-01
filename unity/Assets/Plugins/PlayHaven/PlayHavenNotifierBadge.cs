using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class PlayHavenNotifierBadge : MonoBehaviour {
	public string Token = "token";
	public string Secret = "secret";
	public string Placement = "more_games";
	public float xPos = 0.0f;
	public float yPos = 0.0f;
	public GUIStyle mGUIStyle;
	
	protected JsonData mResponseData;
	
	void Awake(){		
		PlayHaven.MetadataRequest request = new PlayHaven.MetadataRequest(Token,Secret,Placement);
		request.OnSuccess += new PlayHaven.SuccessHandler(this.HandleSuccess);
		request.Send();
	}
	
	public void HandleSuccess(JsonData responseData){
		mResponseData = responseData;
	}
	
	public void Clear(){
		mResponseData = null;
	}
	
	void OnGUI(){
		if (mResponseData != null){
			string badgeType, badgeValue;
			try{
				badgeType = (string)mResponseData["type"];
			} catch (KeyNotFoundException e){
				Debug.Log(e);
				badgeType = null;
			}
			
			if (badgeType == "badge"){		
				try{
					badgeValue = (string)mResponseData["value"];
				} catch (KeyNotFoundException e){
					Debug.Log(e);
					badgeValue = null;
				}
				
				if (badgeValue != null){
					float lWidth = mGUIStyle.CalcSize(new GUIContent(badgeValue)).x + 8.0f;
					if (lWidth < 35.0f) lWidth = 35.0f;
					GUI.depth = 1;
					GUI.Label(new Rect (xPos,yPos,lWidth, 35), badgeValue, mGUIStyle);
				}
			}
		}
	}
}
