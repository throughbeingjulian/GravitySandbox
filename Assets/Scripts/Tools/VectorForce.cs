using UnityEngine;
using System.Collections;
 
public class VectorForce : MonoBehaviour 
{
	public Vector3 gravForce;
	public const float GRAV_CONSTANT = 3.0f; // 6.67398f;
	public const float PARTICLE_MASS = 1.0f;
	
	void Awake()
	{
		gravForce = new Vector3(1.0f, 1.0f, 1.0f);
	}
	
	void Update()
	{
		float gravForceMultiplier = -1 * GRAV_CONSTANT * PARTICLE_MASS;
		Vector3 gravForceAdditive = new Vector3();
		
		for (int i = 0; i < PlanetoidSingleton.Instance.Planetoids.Length; i++)
		{
			Vector3 distBetweenObjs = this.transform.position - PlanetoidSingleton.Instance.Planetoids[i].transform.position;
			gravForceAdditive += (PlanetoidSingleton.Instance.Planetoids[i].mass / Mathf.Pow(Vector3.Magnitude(distBetweenObjs), 3.0f)) * distBetweenObjs;
		}
		
		gravForce = (gravForceAdditive * gravForceMultiplier);
	}
	
	void OnDrawGizmos() 
	{
		Gizmos.DrawWireSphere(this.transform.position, 0.5f);
//		DrawArrow.ForGizmo(transform.position, transform.forward, Color.green);
		Debug.DrawRay(transform.position, gravForce, Color.green);
	}
}
