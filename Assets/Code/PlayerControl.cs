using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// see https://github.com/Aescosaurus/Crystal-Hopper/blob/master/Engine/MouseTracker.h
//  and https://github.com/Aescosaurus/Crystal-Hopper/blob/master/Engine/MouseTracker.cpp
public class PlayerControl
	:
	MonoBehaviour
{
	void Start()
	{
		body = GetComponent<Rigidbody2D>();
	}
	
	void Update()
	{
		var curInput = InputClickTouch.GetClickTouch();

		if( curInput.clicking )
		{
			pressedLastFrame = true;
			canUnpress = false;

			if( !clickMovement )
			{
				if( realLastMousePos )
				{
					// todo: play click sound
					lastMousePos = curInput.pos;
					realLastMousePos = false;
				}
			}
			else lastMousePos = transform.position;

			var curMousePos = curInput.pos;

			diff = lastMousePos - curMousePos;

			if( diff.sqrMagnitude > Mathf.Pow( maxDist,2 ) ) diff = diff.normalized * maxDist;

			diff /= diffDampen;
		}
		else
		{
			if( canUnpress ) pressedLastFrame = false;
			else
			{
				// todo: stop click sound, play release sound
				canUnpress = true;
			}
			realLastMousePos = true;
		}

		bool isReleased = ( !curInput.clicking && diff != Vector2.zero &&
			pressedLastFrame && diff.sqrMagnitude > 0.0f );

		if( canJump && isReleased && !jumpDisabled )
		{
			body.AddForce( diff * speed * ( invertControls ? 1.0f : -1.0f ) * ( clickMovement ? 2.0f : 1.0f ) );
			clampSpdNextFrame = true;
			canJump = false;
			// todo: add jump point penalty
		}

		ClampSpeed();
	}

	void OnCollisionEnter2D( Collision2D coll )
	{
		canJump = true;
	}

	void ClampSpeed()
	{
		if( body.velocity.sqrMagnitude > maxSpeed * maxSpeed ) body.velocity = body.velocity.normalized * maxSpeed;
	}

	Rigidbody2D body;

	Vector2 lastMousePos = Vector2.zero;
	bool realLastMousePos = true;
	Vector2 diff = Vector2.zero;
	bool pressedLastFrame = false;
	bool canUnpress = true;

	[SerializeField] float maxDist = 650.0f;
	[SerializeField] float diffDampen = 50.0f;
	[SerializeField] bool clickMovement = false;
	[SerializeField] float maxSpeed = 1.0f;

	bool canJump = true;
	const bool jumpDisabled = false;
	bool clampSpdNextFrame = false;

	[SerializeField] float speed = 2.2f;
	[SerializeField] bool invertControls = true;
}