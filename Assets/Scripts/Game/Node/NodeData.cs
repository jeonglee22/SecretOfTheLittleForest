using UnityEngine;

[CreateAssetMenu(fileName = "NodeData", menuName = "Scriptable Objects/NodeData")]
public class NodeData : ScriptableObject
{
	public bool IsCenterNode = false;
	public string nodeAxisString;
	public string nodeAxisNumber;
}
