using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{	
	private Vector3 startPos;
	public float moveDur;
	private bool moving = false;
	private Vector3 difference;
	
	//Duration is half a lap
    // Start is called before the first frame update
    void Start()
    {
		startPos = transform.Find("Platform").position;
		difference = transform.Find("End Point").position - startPos;
    }

    // Update is called once per frame
    void Update()
    {
        float timepassed = (Time.time % (moveDur * 2))/moveDur;
		if(timepassed > 1)
		{
			timepassed = 2 - timepassed;
		}
		transform.position = startPos + difference * timepassed;
    }
}
