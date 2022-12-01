using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputClickTouch
{
	public class InputPos
	{
		public Vector2 pos = Vector2.zero;
		public bool clicking = false;
	}

	public static InputPos GetClickTouch()
	{
		var clickTouch = new InputPos();
		switch( Application.platform )
		{
			case RuntimePlatform.Android:
				{
					if( Input.touchCount > 0 )
					{
						var touch = Input.touches[0];
						clickTouch.pos = touch.position;
						clickTouch.clicking = true;
					}
				}
				break;
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WebGLPlayer:
				clickTouch.pos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
				clickTouch.clicking = Input.GetAxis( "Fire1" ) > 0.0f;
				break;
		}

		return( clickTouch );
	}
}