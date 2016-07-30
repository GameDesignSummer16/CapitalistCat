using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {


	public GameObject endPosition;
	public bool destroyOnTrigger;
	public bool plummetOnTrigger;
	public bool reverseOnTrigger;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public Transform getEndTransform()
	{
		return endPosition.transform;
	}


}
