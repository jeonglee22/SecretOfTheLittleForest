using System.Collections;
using UnityEngine;

public class ToyControl : MonoBehaviour
{
	public PlayLogic playLogic;
	public BoardManager boardManager;
	private Toy toy;

	private float movingSpeed = 1f;
	private float corouteTime = 0.001f;

	private bool isMove = false;
	public bool IsMove {  get { return isMove; } }

	public Toy Toy 
	{ 
		get
		{
			return toy;
		}
	}

	public void ToyMove(ref Node before)
	{
		if (isMove)
			return;

		isMove = true;
		playLogic.ChoosedNode.Toy = before.Toy;
		
		var nextPos = playLogic.ChoosedNode.transform.position;
		var currentPos = before.Toy.gameObject.transform.position;

		nextPos.y = currentPos.y;
		before.Toy.gameObject.transform.LookAt(nextPos);
		StartCoroutine(CoMove(currentPos, nextPos));
		before.Toy = null;
	}

	private IEnumerator CoMove(Vector3 startPos, Vector3 endPos)
	{
		var time = Time.deltaTime;
		var pos = Vector3.Lerp(startPos, endPos, movingSpeed * time);
		var toyGo = playLogic.ChoosedNode.Toy.gameObject;
		while (time <= 1 / movingSpeed)
		{
			toyGo.transform.position = pos;
			yield return new WaitForSeconds(corouteTime);
			time += Time.deltaTime;
			pos = Vector3.Lerp(startPos, endPos, movingSpeed * time);
		}
		isMove = false;
		toyGo.transform.position = endPos;
	}
}
