using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour 
{
	public Planetoid gravInfluence;
	public Transform baseJoint;
	public Transform planetoidRef;
	
	private Vector3 normalAlign;
	private Vector3 posAlign;
	
	// Use this for initialization
	void Start() 
	{			
		// TODO: Generalize this later when we implement multiple planets; grabs first (not nearest) planetoid
		if (gravInfluence == null)
		{
			gravInfluence = (Planetoid) GameObject.FindObjectOfType(typeof(Planetoid));
		}
		
		if (baseJoint == null)
		{
			Debug.LogError("No base joint to found; can't calculate planetoid gravity without some reference for the " +
				"character's position.", this);
		}
	}
	
	// Update is called once per frame
	void Update() 
	{
		RaycastHit rayHit = new RaycastHit();
		
		Physics.Linecast(this.baseJoint.position, gravInfluence.transform.position, out rayHit);
		Debug.DrawLine(this.baseJoint.position, gravInfluence.transform.position, Color.red, 0.5f);
				
		normalAlign = rayHit.normal;
		
		if (rayHit.point != Vector3.zero)
		{
			// Store the new position ONLY if we have moved (because linecast will have returned zero otherwise)
			posAlign = rayHit.point;
		}
				
		// Adjust character and reference rotations
		this.transform.rotation = AdjustedRotation(this.transform.rotation, this.transform.up, normalAlign);	
		planetoidRef.rotation = AdjustedRotation(planetoidRef.rotation, planetoidRef.up, normalAlign);
		planetoidRef.position = posAlign;				
		
		// LookAt points local z of planetoid coordinate system in the direction of the player
		gravInfluence.coordinateSystem.LookAt(baseJoint);				
		
		// Temporarily use planetoid coordinate system and perform player height calculations (Z axis)
		this.transform.parent = gravInfluence.coordinateSystem;
		planetoidRef.parent = gravInfluence.coordinateSystem;
		
		this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.planetoidRef.localPosition.z + 0.5f);
		
		// Restore planetoid coordinate system to normal
		this.transform.parent = null;
		planetoidRef.parent = null;
		
		Debug.DrawRay(baseJoint.position, normalAlign, Color.green, 0.5f);		
		Debug.DrawRay(baseJoint.position, baseJoint.up * 0.5f, Color.cyan, 0.5f);
		Debug.DrawRay(baseJoint.position, baseJoint.forward, Color.blue, 0.5f);
	}
	
	/// <summary>
	/// Adjusts the rotation of <param name='axis'> to <param name='alignTo'>.
	/// Basically Blitz3D's AlignToVector function.
	/// </summary>
	/// <returns>
	/// The newly adjusted rotation, relative to <param name='alignTo'>.
	/// </returns>
	/// <param name='axis'>
	/// Axis of object you want to align.
	/// </param>
	/// <param name='alignTo'>
	/// Most likely a polygon normal.
	/// </param>
	/// <param name='rotation'>
	/// Rotation Quaternion of object you're dealing with.
	/// </param>
	private Quaternion AdjustedRotation(Quaternion rotation, Vector3 axis, Vector3 alignTo)
	{
		Quaternion surfaceRot = Quaternion.FromToRotation(axis, alignTo);		
		Quaternion newRot = surfaceRot * rotation;		
		return Quaternion.Slerp(rotation, newRot, 0.2f);	
	}
	
	void OnDrawGizmos() 
	{
		Gizmos.DrawWireSphere(posAlign, 0.5f);
	}
}
