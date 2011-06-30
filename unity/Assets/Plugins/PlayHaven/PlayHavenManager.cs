using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using LitJson;

public class PlayHavenManager : MonoBehaviour {
    void Awake(){ gameObject.name = this.GetType().ToString(); }
	
	public void HandleNativeEvent(string json){
		JsonData nativeData = JsonMapper.ToObject(json);
		int hash = (int)nativeData["hash"];

		PlayHaven.IRequest request = PlayHaven.GetRequestWithHash(hash);
		if (request != null){
			string eventName = (string)nativeData["name"];
			JsonData eventData = (JsonData)nativeData["data"];
			request.TriggerEvent(eventName, eventData);
		}
	}
	
	
}
