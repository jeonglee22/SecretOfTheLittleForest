using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	public List<Node> allNodes;

	public static int allNodeCount = 16 * 6;

	public bool IsChoosed { get; set; }

	public void ReloadBoard()
	{

	}
}
