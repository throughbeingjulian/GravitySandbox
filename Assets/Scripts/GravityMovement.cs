using UnityEngine;
using System.Collections;

public class GravityMovement : MonoBehaviour 
{
	[SerializeField]
	protected Animator animator;
	public float DirectionDampTime = .25f;
	public bool ApplyGravity = false;
	
	// Turning
	float targetTurn = 180.0f;
	bool doTurn = false;
	Quaternion targetRotation;

	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator>();
		
		if(animator.layerCount >= 2)
			animator.SetLayerWeight(1, 1);
	}
		
	// Update is called once per frame
	void Update () 
	{
		if (animator)
		{
			// Turning
			animator.SetBool("Turn", doTurn);
			animator.SetFloat("TurnDirection", targetTurn);
			
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);			

			if (stateInfo.IsName("Base Layer.Run"))
			{
				if (Input.GetButton("Fire1")) animator.SetBool("Jump", true);                
            }
			else
			{
				animator.SetBool("Jump", false);                
            }

			if(Input.GetButtonDown("Fire2") && animator.layerCount >= 2)
			{
				animator.SetBool("Hi", !animator.GetBool("Hi"));
			}
			
			// Pull values from controller/keyboard
      		float h = Input.GetAxis("Horizontal");
        	float v = Input.GetAxis("Vertical");
			Debug.Log("v: " + v + " h: " + h, this);
			
			// Turn around
//			if (v < 0)
//			{
//				Debug.Log("Turning", this);
//				targetRotation = transform.rotation * Quaternion.AngleAxis(targetTurn, Vector3.up); // Compute target rotation when doTurn is triggered
////				doTurn = false;
//			}
//			else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Turn"))
//			{
//				// calls MatchTarget when in Turn state, subsequent calls are ignored until targetTime (0.9f) is reached .
//				animator.MatchTarget(Vector3.one, targetRotation, AvatarTarget.Root, new MatchTargetWeightMask(Vector3.zero, 1), animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 0.9f);			
//			}
			
			animator.SetFloat("Speed", h*h+v*v);
            animator.SetFloat("Direction", h, DirectionDampTime, Time.deltaTime);	
		}   		  
	}
}
