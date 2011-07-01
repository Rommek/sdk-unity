using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using LitJson;


public class PlayHaven : MonoBehaviour {
	protected static Hashtable sRequests;
	protected static PlayHavenManager sManager;
	
	static PlayHaven() {
		sRequests = new Hashtable();
		sManager = GameObject.Find("PlayHavenManager").GetComponent<PlayHavenManager>();
	}
	
	public static IRequest GetRequestWithHash(int hash){
		if (sRequests.ContainsKey(hash)){
			return sRequests[hash] as PlayHaven.IRequest;
		} else {
			return null;
		}
	}
	
	// Event Handlers
	public delegate void SuccessHandler(JsonData responseData);
	public delegate void ErrorHandler(JsonData errorData);
	public delegate void DismissHandler();
	
	//Interface
	public interface IRequest{
		void Send();
		void TriggerEvent(string eventName, JsonData eventData);
	}
	
	public class OpenRequest: IRequest{
		protected String mToken;
		protected String mSecret;
		
		public OpenRequest(String token, String secret){
			mToken = token;
			mSecret = secret;
			
			sRequests.Add(GetHashCode(), this);
		}
		
		[DllImport("__Internal")]
		private static extern void _PlayHavenOpenRequest(int hash, string token, string secret);

		public void Send(){
			Debug.Log("PlayHaven: Sending open request?");
			
			if (Application.isEditor){
				Hashtable result = new Hashtable();
				result["hash"] = GetHashCode();
				result["name"] = "success";
				result["data"] = new Hashtable();
				string jsonResult = JsonMapper.ToJson(result);
				sManager.HandleNativeEvent(jsonResult);
			} else {
				_PlayHavenOpenRequest(GetHashCode(), mToken, mSecret);
			}
		}
		
		// Events
	    public event SuccessHandler OnSuccess = delegate {};
	    public event ErrorHandler OnError = delegate {};
	    
	    public void TriggerEvent(string eventName, JsonData eventData){
	    	if (String.Compare(eventName,"success") == 0){
	    		Debug.Log("PlayHaven: Open request success!");
	    		OnSuccess(eventData);
	    	} else if (String.Compare(eventName, "error") == 0){
	    		Debug.Log("PlayHaven: Open request failed!");
	    		OnError(eventData);
	    	}
	    }
	}
	
	public class MetadataRequest: IRequest{
		protected String mToken;
		protected String mSecret;
		protected String mPlacement;
		
		public MetadataRequest(String token, String secret, String placement){
			mToken = token;
			mSecret = secret;
			mPlacement = placement;
			
			sRequests.Add(GetHashCode(), this);
		}
		
		[DllImport("__Internal")]
		private static extern void _PlayHavenMetadataRequest(int hash, string token, string secret, string placement);
		
		public void Send(){
			Debug.Log("PlayHaven: Sending metadata request?");
			if (Application.isEditor){
				Hashtable result = new Hashtable();
				result["hash"] = GetHashCode();
				result["name"] = "success";
				
				Hashtable dataTable = new Hashtable();
				dataTable["type"] = "badge";
				dataTable["value"] = "1";
				result["data"] = dataTable;
				
				string jsonResult = JsonMapper.ToJson(result);
				sManager.HandleNativeEvent(jsonResult);
			} else {
				_PlayHavenMetadataRequest(GetHashCode(), mToken, mSecret, mPlacement);
			}
		}
		
		// Events	    
	    public event SuccessHandler OnSuccess = delegate {};
	    public event ErrorHandler OnError = delegate {};
	    
	    public void TriggerEvent(string eventName, JsonData eventData){
	    	if (String.Compare(eventName,"success") == 0){
	    		Debug.Log("PlayHaven: Metadata request success!");
	    		Debug.Log(JsonMapper.ToJson(eventData));
	    		OnSuccess(eventData);
	    	} else if (String.Compare(eventName, "error") == 0){
	    		Debug.Log("PlayHaven: Metadata request failed!");
	    		OnError(eventData);
	    	}
	    }
	} 
	
	public class ContentRequest: IRequest{
		protected String mToken;
		protected String mSecret;
		protected String mPlacement;
		
		public ContentRequest(String token, String secret, String placement){
			mToken = token;
			mSecret = secret;
			mPlacement = placement;
			
			sRequests.Add(GetHashCode(), this);	
		}
		
		[DllImport("__Internal")]
		private static extern void _PlayHavenContentRequest(int hash, string token, string secret, string placement);
		public void Send(){
			Debug.Log("PlayHaven: Sending content request?");
			if (Application.isEditor){
				Hashtable result = new Hashtable();
				result["hash"] = GetHashCode();
				result["name"] = "dismiss";
				result["data"] = new Hashtable();
				string jsonResult = JsonMapper.ToJson(result);
				sManager.HandleNativeEvent(jsonResult);
			} else {
				_PlayHavenContentRequest(GetHashCode(), mToken, mSecret, mPlacement);
			}
		}
		
		// Events
		public event DismissHandler OnDismiss = delegate {};  
	    public event ErrorHandler OnError = delegate {};
	    
	    public void TriggerEvent(string eventName, JsonData eventData){
	    	if (String.Compare(eventName,"dismiss") == 0){
	    		Debug.Log("PlayHaven: Content was dismissed!");
	    		OnDismiss();
	    	} else if (String.Compare(eventName, "error") == 0){
	    		Debug.Log("PlayHaven: Content failed!");
	    		OnError(eventData);
	    	}
	    }
	} 
	
}
