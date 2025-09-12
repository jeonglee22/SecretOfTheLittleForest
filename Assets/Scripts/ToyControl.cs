using System.Collections;
using UnityEngine;
using static UnityEngine.InputManagerEntry;

public class ToyControl : MonoBehaviour
{
	public PlayLogic playLogic;
	public BoardManager boardManager;
	private Toy toy;

	private float movingSpeed = 1f;
	private float corouteTime = 0.001f;

	private bool isMove = false;

	public Toy Toy 
	{ 
		get
		{
			return toy;
		}
		set
		{
			toy = ToySettingOnNode(playLogic.ChoosedNode, value);
		}
	}

	public void ToyMove(ref Node before)
	{
		if (isMove)
			return;

		isMove = true;
		Debug.Log(isMove);
		playLogic.ChoosedNode.Toy = before.Toy;
		
		var nextPos = playLogic.ChoosedNode.transform.position;
		var currentPos = before.Toy.gameObject.transform.position;

		nextPos.y = currentPos.y;
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
		Debug.Log("Finish Move");
		isMove = false;
		toyGo.transform.position = endPos;
	}

	public Toy ToySettingOnNode(Node node, Toy toy)
	{
		var nodeScale = node.transform.localScale;

		var childTransform = node.gameObject.transform.GetChild(0);
		var spawnedToy = Instantiate(toy, childTransform);

		var scale = spawnedToy.transform.localScale;
		spawnedToy.transform.localScale = new Vector3(scale.x / nodeScale.x, scale.y / nodeScale.y, scale.z / nodeScale.z);
		playLogic.ChoosedNode.Toy = spawnedToy;

		return spawnedToy;
	}
}
