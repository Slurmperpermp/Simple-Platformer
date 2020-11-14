﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float speed;
	public float bounceFactor;
	private ParticleSystem particles;
	private ParticleSystem.ShapeModule shape;
    public float jumpStren;
    public float gravityscale;
	public int jumpsMax;
	private int jumps;
	private bool dashing = false; 
	public float dashspeed;
	public float dashdur;
	public int dashesMax;
	private int dashes;
	Transform camera; 
    CharacterController cc;
    Vector3 moveDirect ;
	
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
		particles = GetComponent<ParticleSystem>();
		shape = particles.shape;
		camera = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {	
		if (Input.GetButtonDown("Dash") && (moveDirect.x != 0 || moveDirect.z != 0) && dashes > 0 && !dashing)
		{
			dashes -= 1;
			dashing = true;
			Invoke("StopDash",dashdur);
			
			moveDirect.y = 0; 
			moveDirect = moveDirect.normalized * dashspeed;
			particles.Play();
		}
		if(dashing)
		{
			
			moveDirect.y = 0; 
			moveDirect = moveDirect.normalized * dashspeed;
			///float angle = Mathf.Rad2Deg*Mathf.Atan2(moveDirect.x, moveDirect.z) + 180;
			///shape.rotation = new Vector3(0f,angle,0f);
			///transform.rotation = Quaternion.Euler(moveDirect.x,0,moveDirect.z);
			Vector3 angles = transform.eulerAngles;
			angles.x = 30;
			transform.eulerAngles = angles; 
		}
		else
		{
			float moveDirectY = moveDirect.y;
			moveDirect = new Vector3(Input.GetAxis("Horizontal") * speed, 0.0f, Input.GetAxis("Vertical") * speed);
			moveDirect = camera.TransformDirection(moveDirect);
			moveDirect.y = moveDirectY;
			if (cc.isGrounded)
			{
				jumps = jumpsMax;
				moveDirect.y = 0.0f;
				dashes = dashesMax;
			}
			moveDirect.y += Physics.gravity.y * gravityscale * Time.deltaTime;
			
		}
		if (Input.GetButtonDown("Jump") && jumps > 0 )
		{
		  jumps -= 1;
		  moveDirect.y = jumpStren;
		  StopDash();
		}
		Vector3 angles2 = transform.eulerAngles ;
		angles2.y = Mathf.Atan2(moveDirect.x, moveDirect.z )* Mathf.Rad2Deg;
		transform.eulerAngles = angles2;
		cc.Move(moveDirect * Time.deltaTime);
		
		
    }
	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Moving Platform")
		{
			transform.parent = other.transform.parent;
		}
		
	} 
	void OnCollisionStay(Collision other)
	{
		if(other.gameObject.tag == "Moving Platform")
		{
			transform.parent = other.transform.parent;
		}
	}
	void OnCollisionExit(Collision other)
	{
		if(other.gameObject.tag == "Moving Platform")
		{
			transform.parent = null;
		}
	}
	void StopDash()
	{	
		dashing = false;
		particles.Stop();
		transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,transform.eulerAngles.z);
	}
	void OnControllerColliderHit (ControllerColliderHit other)
	{
		
	} 
	void OnTriggerStay (Collider other)
	{
		if(other.gameObject.tag == "Bouncy Platform")
		{
			//moveDirect.y *= -1;
			moveDirect.y = Mathf.Abs(moveDirect.y)* bounceFactor;
		//	jumps = jumpsMax - 1 ; 
		}
	}
}
