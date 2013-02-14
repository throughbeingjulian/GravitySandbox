using UnityEngine;
using System.Collections;

public class GravityMovement : MonoBehaviour 
{
	[SerializeField]
	protected Animator animator;
	public float DirectionDampTime = .25f;
	public bool ApplyGravity = false;
	private float lastFrameAngle = 0.0f;
	private float angle = 0.0f;
	private float timeSliderValue = 1.0f;
	private bool pivot = false;
	float speed = 0.0f;
	public float jumpMultiplier = 1.0f;
	private AnimatorStateInfo currentBaseState;
	static int jumpUpState = -187591467; //Animator.StringToHash("Base.Player@Locomotion@Run_Jump.Player@Locomotion@Run_Jump_Start"); 
	static int jumpDownState = 1804630920; //Animator.StringToHash("Base.Player@Locomotion@Run_Jump.Player@Locomotion@Run_Jump_Land"); 
	
	// Turning around from idle
	private const float IDLE_THRESHOLD = 0.1f;

	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator>();
		
		if(animator.layerCount >= 2)
			animator.SetLayerWeight(1, 1);
	}
	
	void OnGUI()
	{
		GUILayout.BeginVertical();
		{
			GUILayout.Label(new GUIContent("Angle: " + angle));
			timeSliderValue = GUI.HorizontalSlider(new Rect(25.0f, 25.0f, 100.0f, 30.0f), timeSliderValue, 0.0f, 2.0f);
			Time.timeScale = timeSliderValue;
			if (pivot) { GUILayout.Label(new GUIContent("Pivot!")); }
			GUILayout.Label(new GUIContent("Speed: " + speed));
		}	
		GUILayout.EndVertical();
	}
	
	private bool FacingForward(float angle)
	{
		if (angle > -90.0f && angle < 90.0f)
		{
			return true;
		}
		
		return false;
	}
	
	private float CurveDirection(float curvePoint)
	{
		if (currentBaseState.nameHash == jumpUpState)
		{
			// Increasing
			Debug.Log("Increasing");
			return curvePoint;
		}
		else if (currentBaseState.nameHash == jumpDownState)
		{
			// Otherwise, curve is decreasing
			Debug.Log("Decreasing");
			return (-1.0f * curvePoint);
		}
		
		return 0.0f;
	}
		
	// Update is called once per frame
	void Update () 
	{
		if (animator)
		{	
			currentBaseState = animator.GetCurrentAnimatorStateInfo(0);
//			Debug.Log("Cur state: " + currentBaseState.nameHash + "\nJump up state: " + jumpUpState);
			
			// Pull values from controller/keyboard
      		float h = Input.GetAxis("Horizontal");
        	float v = Input.GetAxis("Vertical");
//			Debug.Log("v: " + v + " h: " + h, this);
			
			speed = h * h + v * v;
			angle = Mathf.Atan2(h, v) * Mathf.Rad2Deg;
//			Debug.Log("angle: " + angle + " lastangle: " + lastFrameAngle);
			
			
			
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);			

			if (stateInfo.IsName("Base.Run"))
			{
//				Debug.Log(animator.GetFloat("JumpCurve"));
				if (Input.GetButton("Fire1"))
				{
					animator.SetBool("Jump", true);   
				}
				if (FacingForward(lastFrameAngle) && !FacingForward(angle))
				{
					// Perform piviot
					// TODO: be more selective of what angles we do each pivot at
					animator.SetBool("RunPivot", true);    
					pivot = true;
				}
				else
				{
					animator.SetBool("RunPivot", false);   
					pivot = false;
				}
				
					

            }
			else
			{
				animator.SetBool("Jump", false);            
            }
			
			Vector3 newPosition = transform.position;
       		newPosition.y += (CurveDirection(animator.GetFloat("JumpCurve")) * jumpMultiplier * Time.deltaTime); 
			if (currentBaseState.nameHash == jumpUpState || currentBaseState.nameHash == jumpDownState)
			{			
				Debug.Log((CurveDirection(animator.GetFloat("JumpCurve")) * jumpMultiplier * Time.deltaTime));
			}
   			transform.position = newPosition;
			
			if (stateInfo.IsName("Base.Idle"))
			{
				if (Input.GetButton("Fire1")) animator.SetBool("IdleJump", true);    
			}
			else
			{
				animator.SetBool("IdleJump", false);     				
			}
						
//			Debug.Log("Running? " + stateInfo.IsName("Base.Run"));
			// Turn around from idle; perform cheaper check first to boolean short circuit
			if (v < 0 && stateInfo.IsName("Base.Idle"))
			{
				
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
			
			
			
			lastFrameAngle = angle;
			
		}   		  
	}
}
