using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraMove : MonoBehaviour {

    //Speed variable for how fast the camera should scroll.
    public int speed = 0;
    //How far the camera will go on the x-axis
    public Vector3 endPoint;
    //TODO:list of vector waypoints for camera?
    //public List<Vector3> waypoints;

	// Use this for initialization
	void Start () {
	
	}
	/// <summary>
    /// TODO: If the end of level trigger is reached, stop moving the camera.
    /// </summary>
	// Update is called once per frame
	void Update () {
        //Get camera position.
        Vector3 position = transform.localPosition;
        //move camera to the right until it reaches the desired position.
        if (position.x < endPoint.x)
        {
            transform.Translate(Vector3.right * (Time.deltaTime * speed), Space.World);
        }
        
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, endPoint + this.transform.position);
    }
}
