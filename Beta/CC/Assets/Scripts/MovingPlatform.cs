using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	public Vector3 endPos = Vector3.zero;
	public float speed = 1;

	private float timer = 0;
	private Vector3 startPos = Vector3.zero;
	private bool outgoing = true;

	// Use this for initialization
	void Start () {
		startPos = this.gameObject.transform.position;
		endPos = endPos + startPos;

        float distance = Vector3.Distance(startPos, endPos);
        if(distance != 0)
        {
            speed = speed / distance;
        }
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime * speed;

		if (outgoing)
		{
			this.transform.position = Vector3.Lerp(startPos, endPos, timer);
            if (timer > 1)
            {
                outgoing = false;
                timer = 0;
            }
		}
		else
		{
			this.transform.position = Vector3.Lerp(endPos, startPos, timer);
            if (timer > 1)
            {
                outgoing = true;
                timer = 0;
            }
        }
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(this.transform.position, endPos + this.transform.position);
	}
}
