using System.Collections.Generic;
using UnityEngine;
 
public class PlanetoidSingleton : MonoBehaviour
{
    private static PlanetoidSingleton instance;	
	[SerializeField]
	private Planetoid[] planetoids;
 
//    public PlanetoidSingleton() 
//    {
//        if (instance != null)
//        {
//            Debug.LogError ("Cannot have two instances of singleton. Self destruction in 3...");
//            return;
//        }
// 
//        instance = this;
//        Init();
//    }
	
	public void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else 
		{
			Debug.LogError("There is more than one PlanetoidSingleton in the scene.", this);
		}
		
		QueryForPlanetoids();
	}
 
    public static PlanetoidSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                new PlanetoidSingleton();
            }
 
            return instance;
        }
    }

	private void Init()
	{
		planetoids = new Planetoid[0];	
	}
 
	public Planetoid[] Planetoids
	{
		get
		{
			return planetoids;
		}
		set
		{
			planetoids = value;
		}
	}
	
	public void QueryForPlanetoids()
	{
		Planetoid[] foundPlanetoids = (Planetoid[]) FindObjectsOfType(typeof(Planetoid));
		planetoids = foundPlanetoids;
	}
}