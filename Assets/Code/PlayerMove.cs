using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class PlayerMove
	:
	MonoBehaviour
{
	void Start()
	{
		body = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		var rot = transform.eulerAngles;
		rot.z = Mathf.Atan2( body.velocity.y,body.velocity.x ) * Mathf.Rad2Deg;
		transform.eulerAngles = rot;
	}

	void OnCollisionEnter2D( Collision2D coll )
	{
		if( coll.contactCount > 0 )
		{
			var norm = Vector2.zero;
			for( int i = 0; i < coll.contactCount; ++i )
			{
				norm += coll.GetContact( i ).normal;
			}

			norm /= coll.contactCount;

			body.AddForce( norm * bounceForce,ForceMode2D.Impulse );
		}
	}

	Rigidbody2D body;

	[SerializeField] float bounceForce = 10.0f;
}