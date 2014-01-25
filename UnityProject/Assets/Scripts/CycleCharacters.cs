using UnityEngine;
using System.Collections;

public class CycleCharacters : MonoBehaviour {

	public GameObject[] character_faces;
	public GameObject[] characters;

	public Transform leftposition;
	public Transform rightposition;

	public Transform leftspawn;
	public Transform rightspawn;

	public string axis1;
	public string axis2;
	public string confirm1;
	public string confirm2;

	private GameObject left;
	private GameObject right;
	private int lindex = 0;
	private int rindex = 0;

	private float lasta1 = 0;
	private float lasta2 = 0;

	private bool p1_ready = false;
	private bool p2_ready = false;

	// Use this for initialization
	void Start () {
		left = Instantiate(character_faces[0], leftposition.position, leftposition.rotation) as GameObject;
		right = Instantiate(character_faces[0], rightposition.position, rightposition.rotation) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		//listen for button presses
		float a1 = Input.GetAxis(axis1);
		float a2 = Input.GetAxis(axis2);


		if(lasta1 == 0 && !p1_ready){
			if(a1 < 0){
				Destroy(left);
				lindex += 1;
				if(lindex >= character_faces.Length) lindex = 0;
				left = Instantiate(character_faces[lindex], leftposition.position, leftposition.rotation) as GameObject;

				Debug.Log("player 1 down");
			}
			else if(a1 > 0){
				Destroy(left);
				lindex -= 1;
				if(lindex < 0) lindex = character_faces.Length - 1;
				left = Instantiate(character_faces[lindex], leftposition.position, leftposition.rotation) as GameObject;

				Debug.Log("player 1 up");
			}
		}
		if(lasta2 == 0 && !p2_ready){
			if(a2 < 0){
				Destroy(right);
				rindex += 1;
				if(rindex >= character_faces.Length) rindex = 0;
				right = Instantiate(character_faces[rindex], rightposition.position, rightposition.rotation) as GameObject;

				Debug.Log("player 2 down");
			}
			else if(a2 > 0){
				Destroy(right);
				rindex -= 1;
				if(rindex < 0) rindex = character_faces.Length - 1;
				right = Instantiate(character_faces[rindex], rightposition.position, rightposition.rotation) as GameObject;

				Debug.Log("player 2 up");
			}
		}

		lasta1 = a1;
		lasta2 = a2;

		if(Input.GetButton(confirm1)){
			p1_ready = true;
		}
		if(Input.GetButton(confirm2)){
			p2_ready = true;
		}

		if(p1_ready && p2_ready){
			Destroy(gameObject);
			Destroy(left);
			Destroy(right);
			//Destroy(leftposition);
			//Destroy(rightposition);
			//Destroy(leftspawn);
			//Destroy(rightspawn);

			Instantiate(characters[lindex], leftspawn.position, leftspawn.rotation);
			Instantiate(characters[rindex], rightspawn.position, rightspawn.rotation);
		}
	}
}
