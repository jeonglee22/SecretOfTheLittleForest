using UnityEngine;

public class TriggerChecking : MonoBehaviour
{
	public StatShowManager statShowManager;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var go = collision.gameObject;

		if(go.GetComponent<Toy>() != null)
		{
			var toy = go.GetComponent<Toy>();

			statShowManager.SetToyText(toy.Data);
			statShowManager.SetGridImage(toy.Toy2D);
		}
	}
}
