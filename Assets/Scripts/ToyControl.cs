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

	public void ToyMove(ref Node before, bool isBack = false)
	{
		if (isMove)
			return;

		isMove = true;
		var originToy = playLogic.ChoosedNode.Toy;
		playLogic.ChoosedNode.Toy = before.Toy;
		before.Toy.IsMove = true;
		
		var nextPos = playLogic.ChoosedNode.transform.position;
		var currentPos = before.Toy.gameObject.transform.position;

		nextPos.y = currentPos.y;
		StartCoroutine(CoMove(currentPos, nextPos, isBack));
		if (!isBack)
			before.Toy = null;
		else
			playLogic.ChoosedNode.Toy = originToy;
	}

	private IEnumerator CoMove(Vector3 startPos, Vector3 endPos, bool isBack)
	{
		var time = Time.deltaTime;
		var pos = Vector3.Lerp(startPos, endPos, movingSpeed * time);
		var toy = playLogic.ChoosedNode.Toy;
		toy.gameObject.transform.LookAt(endPos);
		toy.canvas.rotation = Camera.main.transform.rotation;
		while (time <= 1 / movingSpeed)
		{
			toy.transform.position = pos;
			yield return new WaitForSeconds(corouteTime);
			time += Time.deltaTime;
			pos = Vector3.Lerp(startPos, endPos, movingSpeed * time);
		}
		if (isBack)
		{
			toy.gameObject.transform.LookAt(startPos);
			toy.canvas.rotation = Camera.main.transform.rotation;
			time = Time.deltaTime;
			while (time <= 1 / movingSpeed)
			{
				toy.transform.position = pos;
				yield return new WaitForSeconds(corouteTime);
				time += Time.deltaTime;
				pos = Vector3.Lerp(endPos, startPos, movingSpeed * time);
			}
		}
		isMove = false;
		toy.transform.position = isBack ? startPos : endPos;
	}
}
