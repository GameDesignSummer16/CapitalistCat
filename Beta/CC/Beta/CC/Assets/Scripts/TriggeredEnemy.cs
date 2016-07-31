using UnityEngine;
using System.Collections;

public class TriggeredEnemy : MonoBehaviour
{

    public Vector3 endPos = Vector3.zero;
    public float speed = 1;

    private bool active = false;
    private float timer = 0;
    private Vector3 startPos = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        startPos = this.gameObject.transform.position;
        endPos = endPos + startPos;

        float distance = Vector3.Distance(startPos, endPos);
        if (distance != 0)
        {
            speed = speed / distance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * speed;

        //TODO: Need to do some testing on this. Should stop at endPos.
        //TODO: Also need to make sure that the triggeredEnemies reset on TP's
        if (this.active && !this.transform.position.Equals(endPos))
        {
            this.transform.position = Vector3.Lerp(startPos, endPos, timer);
            if (timer > 1)
            {
                timer = 0;
            }  
        }
        else
        {
            this.active = false;
        }       
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, endPos + this.transform.position);
    }

    /// <summary>
    /// If active is true then we should start moving the enemy.
    /// </summary>
    public void setActive()
    {
        this.active = !active;
    }
}
