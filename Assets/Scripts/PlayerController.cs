using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float Speed;
	public float JumpForce;
	public bool FaceRight = true;
	public bool grounded = false;
	float move;
	private Animator Anim;
	public LayerMask DestroyMask;
	public LayerMask GroundMask;
    [HideInInspector]
	public Transform groundCheck;
	void Start () {
		Anim = GetComponent<Animator> ();
		groundCheck = GameObject.Find ("GroundCheck").transform;
	}

	void FixedUpdate () 
	{
		Anim.SetFloat ("charSpeed",Mathf.Abs(move));
		Anim.SetBool ("Ground", grounded);
		Anim.SetFloat ("vSpeed", transform.GetComponent<Rigidbody2D>().velocity.y);
		grounded = Physics2D.OverlapCircle (groundCheck.position, 0.05f,GroundMask);
		move = Input.GetAxisRaw ("Horizontal") * Speed;
		GetComponent<Rigidbody2D>().velocity = new Vector2 (move*Time.deltaTime, GetComponent<Rigidbody2D>().velocity.y);
        if (move > 0 && !FaceRight)
            flip();
        else if (move < 0 && FaceRight)
            flip();
	}
	void Update()
	{
		if (grounded&&Input.GetKeyDown(KeyCode.Space)) 
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0,JumpForce));
	} 
	void flip()
	{
		FaceRight = !FaceRight;
		Quaternion theRot = transform.rotation;
		if(theRot.y==0)
			theRot.Set(theRot.x,180,theRot.z,theRot.w);
		else
			theRot.Set(theRot.x,0,theRot.z,theRot.w);
		transform.rotation = theRot;
	}
}
