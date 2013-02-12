using UnityEngine;
using System.Collections;

public class GravityMovement : MonoBehaviour 
{
	[SerializeField]
	protected Animator animator;
	public float DirectionDampTime = .25f;
	public bool ApplyGravity = false;
	
	// Turning around from idle
	private const float IDLE_THRESHOLD = 0.1f;

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
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);			

			if (stateInfo.IsName("Base Layer.Run"))
			{
				if (Input.GetButton("Fire1")) animator.SetBool("Jump", true);                 
            }
			else
			{
				animator.SetBool("Jump", false);            
            }
			
			if (stateInfo.IsName("Base Layer.Idle"))
			{
				if (Input.GetButton("Fire1")) animator.SetBool("IdleJump", true);    
			}
			else
			{
				animator.SetBool("IdleJump", false);     				
			}
			
			// Pull values from controller/keyboard
      		float h = Input.GetAxis("Horizontal");
        	float v = Input.GetAxis("Vertical");
//			Debug.Log("v: " + v + " h: " + h, this);
			
			float speed = h * h + v * v;
						
//			Debug.Log("Running? " + stateInfo.IsName("Base Layer.Run"));
			// Turn around from idle
			if (v < 0)// && speed < IDLE_THRESHOLD)
			{
				float angle = Mathf.Atan2(h, v) * Mathf.Rad2Deg;
				
				// Normalize values so that they span from 0 (turn 90 deg right) to 180 (turn 90 deg left)
				if (angle < 0)
				{
					// -90 (turn 90 deg left) will become 180 and -180 (turn 180 deg left) will become 90
					angle += 270;
				}
				else
				{
					// 90 (turn 90 deg right) will become 0 and 180 (turn 180 deg right) will become 90
					angle -= 90;
				}
				
				// Clamp any values between full left and full right spin
				if (angle > 90 && angle < 91)
				{
					angle = 90;
				}
				
//				Debug.Log("Turning " + angle, this);
				animator.SetFloat("IdleTurnDirection", angle);
				animator.SetBool("IdleTurn", true);
			}
			else
			{
				animator.SetBool("IdleTurn", false);        
			}
			
			
			animator.SetFloat("Speed", speed);
            animator.SetFloat("Direction", h, DirectionDampTime, Time.deltaTime);	
		}   		  
	}
}
