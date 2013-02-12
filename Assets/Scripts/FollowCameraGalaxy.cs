using UnityEngine;
using System.Collections;

public class FollowCameraGalaxy : MonoBehaviour
{

	public float damping = .025f;
	public Actor target;
	private	Vector3 offset;
	
	// Player Camera Tripod 
	private Transform pivotPoint;
	
	// Use this for initialization
	void Start ()
	{			
		pivotPoint = GameObject.FindGameObjectWithTag("CameraPivotPoint").transform;
		offset = target.transform.position - pivotPoint.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!target.PLANET_GRAVITY){
	    // Calculations for a smooth camera rotation around planetoid
			float currentAngle = transform.eulerAngles.y;
        	float desiredAngle = target.transform.eulerAngles.y;
        	float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);
        	Quaternion rotation = Quaternion.Euler(0, angle, 0);		
			transform.position = pivotPoint.position - (rotation * pivotPoint.position);
		}
		else {
			transform.position = pivotPoint.position;
		}
		transform.LookAt(target.transform, target.baseJoint.up);
	}
}
