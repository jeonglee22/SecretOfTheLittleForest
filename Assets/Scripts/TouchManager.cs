using UnityEngine.EventSystems;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
	private int fingerId = -1;

	private Vector2 fingerTouchStartPosition;
	private float fingerTouchStartTime;

	private float sweepDistance = 1f;

	public static 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var touches = Input.touches;
        foreach (Touch touch in touches)
        {
			switch (touch.phase)
			{
				case TouchPhase.Began:
					if (fingerId == -1)
					{
						fingerId = touch.fingerId;
						fingerTouchStartPosition = touch.position;
						fingerTouchStartTime = Time.time;
					}
					break;
				case TouchPhase.Moved:
				case TouchPhase.Stationary:
					break;
				case TouchPhase.Ended:
				case TouchPhase.Canceled:
					break;
			}
		}
	}
}
