using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	//private int fingerId = -1;

	private Vector2 fingerTouchStartPosition;
	private float fingerTouchStartTime;

	private float tapTimeLimit = 0.2f;

	public static float FingersDelta { get; private set; }
	public static bool IsTap { get; private set; } = false;
	public static bool IsLongPress { get; private set; } = false;

	private float deltaOffset = 5f;

	public Action tapFunc;
	public Action longPressFunc;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		var touches = Input.touches;
		foreach (UnityEngine.Touch touch in touches)
		{
			switch (touch.phase)
			{
				case TouchPhase.Began:
					break;
				case TouchPhase.Moved:
					break;
				case TouchPhase.Stationary:

					break;
				case TouchPhase.Ended:
				case TouchPhase.Canceled:
					break;
			}
		}

		if (Input.touchCount == 2)
		{
			var touch1 = Input.GetTouch(0);
			var touch2 = Input.GetTouch(1);

			float currentDistance = Vector2.Distance(touch1.position, touch2.position);

			Vector2 touch1beforePos = touch1.position - touch1.deltaPosition;
			Vector2 touch2beforePos = touch2.position - touch2.deltaPosition;
			float beforeDistance = Vector2.Distance(touch1beforePos, touch2beforePos);

			FingersDelta = (currentDistance - beforeDistance) / Screen.dpi;
			FingersDelta *= Time.deltaTime * deltaOffset;
		}
	}

	private void DoFunction()
	{
		if (Input.touchCount == 1 && IsTap && tapFunc != null)
		{
			tapFunc();
		}
		else if (Input.touchCount == 1 && IsLongPress && longPressFunc != null)
		{
			longPressFunc();
		}

		IsTap = false;
		IsLongPress = false;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		fingerTouchStartPosition = eventData.position;
		fingerTouchStartTime = Time.time;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		var timeDiff = Time.time - fingerTouchStartTime;
		var disDiff = Vector2.Distance(eventData.position, fingerTouchStartPosition);

		if (Input.touchCount == 1)
		{
			if (timeDiff <= tapTimeLimit)
				IsTap = true;
			else
			{
				IsLongPress = true;
				IsTap = false;
			}
		}
		DoFunction();
	}
}
