using UnityEngine;

public class StatCanvasController : MonoBehaviour
{
	private Canvas billboard;

	private void Awake()
	{
		billboard = GameObject.FindWithTag(Tags.BillBoard).GetComponent<Canvas>();
	}

	private void FixedUpdate()
	{
		transform.rotation = billboard.transform.rotation;
	}
}
