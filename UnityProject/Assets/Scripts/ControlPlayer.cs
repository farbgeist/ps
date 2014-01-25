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

		anim = GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () {
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
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  
		//anim.SetBool("isgrounded", grounded);


		float v = Input.GetAxis(verAxis);

		if(Input.GetButtonDown(fire1) && Time.time > nextFire){	//Normal Attack
			nextFire = Time.time + attackspeed;
			anim.SetTrigger("normalpunch");
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

	void FixedUpdate(){
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

	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log("object entered");
		Debug.Log(other.tag);
		//TODO: Enter code for life handling here
	}

	void TriggerPunch(){
		punching = !punching;
		armobject.collider2D.enabled = punching;

		/*Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>();
 
		foreach (Transform child in allChildren)
		{
			if (child.gameObject.tag == "ArmCollider"){
				child.gameObject.collider2D.enabled = punching;
				Debug.Log("object found");
				break;
			}
		}*/
	}

	void TriggerKick(){
		kicking = !kicking;
		legobject.collider2D.enabled = kicking;

		/*Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>();
 
		foreach (Transform child in allChildren)
		{
			if (child.gameObject.tag == "LegCollider"){
				child.gameObject.collider2D.enabled = kicking;
				Debug.Log("object found");
				break;
			}
		}*/
	}
}
