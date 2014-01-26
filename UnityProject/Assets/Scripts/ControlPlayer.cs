using UnityEngine;
using System.Collections;

public class ControlPlayer : MonoBehaviour {

	public bool facingRight = true;			// For determining which way the player is currently facing.

	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.

	public string horAxis = "Horizontal";
	public string verAxis = "Vertical";
	public string fire1 = "Fire1";
	public string fire2 = "Fire2";

	public float playerHealth = 100f;
	public SpriteRenderer healthbar;
	public SpriteRenderer lasthealth;


	public float attackspeed = 5f;
	private float nextFire = 0;

	public float jumpForce = 1000f;

	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.
	private bool jump = false;
	private bool iscrouching = false;

	private bool punching = false;
	private bool kicking = false;

	private GameObject armobject;
	private GameObject legobject;

	private Animator anim;					// Reference to the player's animator component.

	private bool isdead = false;

	public AudioSource mainbgm;
	public GameObject winning;

	void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("groundCheck");

		Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>();
 
		foreach (Transform child in allChildren)
		{
			if (child.gameObject.tag == "ArmCollider"){
				armobject = child.gameObject;
			}
			else if(child.gameObject.tag == "LegCollider"){
				legobject = child.gameObject;
			}
		}

		RegisterHit[] rh = gameObject.GetComponentsInChildren<RegisterHit>();
		rh[0].mainController = this;

		anim = GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () {
		//set left/right differences
		if(transform.position.x <= 0){
			horAxis = "Horizontal";
			verAxis = "Vertical";
			fire1 = "Fire1";
			fire2 = "Fire2";
			facingRight = true;
		}
		else{
			horAxis = "Horizontal_b";
			verAxis = "Vertical_b";
			fire1 = "Fire1_b";
			fire2 = "Fire2_b";
			facingRight = false;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		if(!isdead){
			// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
			grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  

			float v = Input.GetAxis(verAxis);

			if(Input.GetButtonDown(fire1) && Time.time > nextFire){	//Normal Attack
				nextFire = Time.time + attackspeed;
				anim.SetTrigger("normalpunch");
				Debug.Log("trigger punch");
			}
			else if(Input.GetButtonDown(fire2) && Time.time > nextFire){	//Heavy Attack
				nextFire = Time.time + attackspeed;
				anim.SetTrigger("heavykick");
			}	
			//Jumping
			if (v > 0 && grounded){
					jump = true;
			}
			else if (v < 0){
				anim.SetBool("crouching", true);
				iscrouching = true;
			}
			else{
				anim.SetBool("crouching", false);
				iscrouching = false;
			}
		}	
		else if(Input.anyKey){
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	void FixedUpdate(){
		if(!isdead){
			float h = 0;
			if(!iscrouching){
				// Cache the horizontal input.
				h = Input.GetAxis(horAxis);

				// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
				if(h * rigidbody2D.velocity.x < maxSpeed)
					// ... add a force to the player.
					rigidbody2D.AddForce(Vector2.right * h * moveForce);
			}
			// If the player's horizontal velocity is greater than the maxSpeed...
			if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
				// ... set the player's velocity to the maxSpeed in the x axis.
				rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);

			//adapt animation
			anim.SetFloat("speed", Mathf.Abs(rigidbody2D.velocity.x));

			// If the input is moving the player right and the player is facing left...
			if(h > 0 && !facingRight)
				// ... flip the player.
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if(h < 0 && facingRight)
				// ... flip the player.
				Flip();


			if(jump)
			{
				// Add a vertical force to the player.
				rigidbody2D.AddForce(new Vector2(0f, jumpForce));

				// Make sure the player can't jump again until the jump conditions from Update are satisfied.
				jump = false;
			}
		}
	}

	void Flip ()
	{
		if(!isdead){
			// Switch the way the player is labelled as facing.
			facingRight = !facingRight;

			// Multiply the player's x local scale by -1.
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
	}

	void TriggerPunch(){
		if(!isdead){
			punching = !punching;
			armobject.collider2D.enabled = punching;
		}
	}

	void TriggerKick(){
		if(!isdead){
			kicking = !kicking;
			legobject.collider2D.enabled = kicking;
		}
	}

	//Trigger a hit through collider other with the damage amount amt
	public void TriggerHit(Collider2D other, float amt){
		if(!isdead){
			Debug.Log("iscalled with dem damages "+ amt);
			playerHealth -= amt;
			if(playerHealth < 0) playerHealth = 0;
			
			if(playerHealth <= 0 && !isdead){
				isdead = true;
				anim.SetTrigger("death");
				lasthealth.enabled = false;
				GameObject hitSound = GameObject.Find("uff01");
				hitSound.audio.Play();
				mainbgm.Stop();
				Instantiate(winning, new Vector3(0, 0, -7), transform.rotation);

			}else{ // hit but not dead
				GameObject hitSound = GameObject.Find("hit01");
				hitSound.audio.Play();
			}
			healthbar.gameObject.transform.localScale = new Vector3(playerHealth / 100, 1, 1);// = playerHealth / 100;
		}
	}
}
