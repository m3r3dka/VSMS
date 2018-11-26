using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetUp : NetworkBehaviour {

	[SerializeField] Behaviour[] componentsToDisable;
	Camera SceneCamera;

	void Start () 
	{
		if (!isLocalPlayer) 
		{
			for (int i = 0; i < componentsToDisable.Length; i++)
				componentsToDisable [i].enabled = false;
			gameObject.layer = LayerMask.NameToLayer ("remotePlayer");
		}
		else {
			SceneCamera = Camera.main;
			if(SceneCamera !=null)
				SceneCamera.gameObject.SetActive(false);
		}

		transform.name = "Player " + GetComponent<NetworkIdentity> ().netId;
	}

	void OnDisable () 
	{
		if(SceneCamera !=null)
			SceneCamera.gameObject.SetActive(true);
	}
}
