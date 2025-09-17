using UnityEngine;

public class TriggerChecking : MonoBehaviour
{
	public StatShowManager statShowManager;
	private Collider2D col2d;

	private void Awake()
	{
		col2d = GetComponent<Collider2D>();
	}

	private void Start()
	{
		col2d.isTrigger = true;
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		var go = collision.gameObject;

		if(go.GetComponent<Toy>() != null)
		{
			var toy = go.GetComponent<Toy>();

			statShowManager.SetGridImage(toy.Toy2D);
			statShowManager.SetToyText(toy.Data);
		}
	}
}
