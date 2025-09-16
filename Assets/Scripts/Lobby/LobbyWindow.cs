using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyWindow : MonoBehaviour
{
	public GameObject firstSelected;

	protected LobbyWindowManager manager;

	public void Init(LobbyWindowManager mgr)
	{
		manager = mgr;
	}

	public virtual void Open()
	{
		gameObject.SetActive(true);
	}

	public virtual void Close()
	{
		gameObject.SetActive(false);
	}
}
