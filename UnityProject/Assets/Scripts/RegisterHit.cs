using UnityEngine;
using System.Collections;

public class RegisterHit : MonoBehaviour {
	public ControlPlayer mainController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "ArmCollider"){
			mainController.TriggerHit(other, 5);
		}
		else if(other.gameObject.tag == "LegCollider"){
			mainController.TriggerHit(other, 7);
		}
	}
}
