using UnityEngine;
using System.Collections;

public class IntroControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKey){
			 Application.LoadLevel(1);
		}
	}

	public void nextRoom(){
		Application.LoadLevel(1);
	}
}
