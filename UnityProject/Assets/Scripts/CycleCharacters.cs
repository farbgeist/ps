using UnityEngine;
using System.Collections;

public class CycleCharacters : MonoBehaviour {

	public GameObject[] character_faces;
	public GameObject[] characters;
	public GameObject[] winnings;

	public Transform leftposition;
	public Transform rightposition;

	public Transform leftspawn;
	public Transform rightspawn;

	public string axis1;
	public string axis2;
	public string confirm1;
	public string confirm2;

	public SpriteRenderer leftbar;
	public SpriteRenderer rightbar;
	public SpriteRenderer leftlast;
	public SpriteRenderer rightlast;

	public AudioSource intro;
	public AudioSource game;

	private GameObject left;
	private GameObject right;
	private int lindex = 0;
	private int rindex = 0;

	private float lasta1 = 0;
	private float lasta2 = 0;

	private bool p1_ready = false;
	private bool p2_ready = false;

	public SpriteRenderer controls;
	public GUIText text1;
	public GUIText text2;

	public SpriteRenderer[] activaterenderers;

	private float switchtime = 0;

	// Use this for initialization
	void Start () {
		left = Instantiate(character_faces[0], leftposition.position, leftposition.rotation) as GameObject;
		right = Instantiate(character_faces[0], rightposition.position, rightposition.rotation) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(!controls.enabled){
			if(Input.GetKeyDown(KeyCode.H)){
				controls.enabled = true;
				text1.enabled = false;
				text2.enabled = false;
				switchtime = Time.time + 1f;
			}
			else{
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
					if(!p1_ready){
						Animator[] tmpobj = left.gameObject.GetComponentsInChildren<Animator>();
						tmpobj[0].SetTrigger("selected");

						//left.gameObject.GetComponent<GameObject>().GetComponent<Animator>().SetTrigger("selected");
					}
					p1_ready = true;
				}
				if(Input.GetButton(confirm2)){
					if(!p2_ready){
						Animator[] tmpobj = right.gameObject.GetComponentsInChildren<Animator>();
						tmpobj[0].SetTrigger("selected");
						//right.gameObject.GetComponent<GameObject>().GetComponent<Animator>().SetTrigger("selected");
					}
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

					GameObject go = Instantiate(characters[lindex], leftspawn.position, leftspawn.rotation) as GameObject;
					ControlPlayer[] cp1 = go.GetComponentsInChildren<ControlPlayer>();
					cp1[0].healthbar = leftbar;
					cp1[0].lasthealth = leftlast;
					cp1[0].mainbgm = game;
					cp1[0].winning = winnings[rindex];

					go = Instantiate(characters[rindex], rightspawn.position, rightspawn.rotation) as GameObject;
					ControlPlayer[] cp2 = go.GetComponentsInChildren<ControlPlayer>();
					cp2[0].healthbar = rightbar;
					cp2[0].lasthealth = rightlast;
					cp2[0].mainbgm = game;
					cp2[0].winning = winnings[lindex];

					cp1[0].otherPlayer = cp2[0];
					cp2[0].otherPlayer = cp1[0];

					foreach(SpriteRenderer sr in activaterenderers){
						sr.enabled = true;
					}

					intro.Stop();
					game.Play();
				}
			}
		}
		else{
			if(Input.anyKey && Time.time > switchtime){
				controls.enabled = false;
				text1.enabled = true;
				text2.enabled = true;
			}
		}
	}
}
