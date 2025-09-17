using System;
using System.IO;
using System.Text;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ImageToolManager : MonoBehaviour
{
	[Header("ObjectID")]
	public TMP_InputField objectid;
	public TextMeshProUGUI objectName;
	private int id;

	[Header("Position")]
	public TMP_InputField xPos;
	public TMP_InputField yPos;
	public TMP_InputField zPos;
	private Vector3 objectPos;

	[Header("Rotation")]
	public TMP_InputField xRot;
	public TMP_InputField yRot;
	public TMP_InputField zRot;
	private Vector3 objectRot;

	[Header("Scale")]
	public TMP_InputField scaleX;
	public TMP_InputField scaleY;
	public TMP_InputField scaleZ;
	private Vector3 objectScale;

	[Header("")]
	public TMP_InputField projectionSize;
	private float cameraProjectionSize;

	public TMP_InputField imageWidth;
	public TMP_InputField imageHeight;
	private Vector2 imageSize;

	public Transform origin;

	public Camera renderCamera;
	public RectTransform imageRect;

	private GameObject obj;

	private string path = "./Images";

	private void Awake()
	{
	}

	private void Init()
	{
		objectPos = obj.transform.position;
		objectRot = obj.transform.rotation.eulerAngles;
		objectScale = obj.transform.localScale;

		xPos.text = objectPos.x.ToString();
		yPos.text = objectPos.y.ToString();
		zPos.text = objectPos.z.ToString();

		xRot.text = objectRot.x.ToString();
		yRot.text = objectRot.y.ToString();
		zRot.text = objectRot.z.ToString();

		scaleX.text = objectScale.x.ToString();
		scaleY.text = objectScale.y.ToString();
		scaleZ.text = objectScale.z.ToString();
		
		cameraProjectionSize = renderCamera.orthographicSize;
		projectionSize.text = cameraProjectionSize.ToString();

		imageSize = imageRect.rect.size;
		imageWidth.text = imageRect.rect.width.ToString();
		imageHeight.text = imageRect.rect.height.ToString();
	}

	private void Update()
	{
		if (obj != null)
		{
			obj.transform.position = objectPos;
			obj.transform.rotation = Quaternion.Euler(objectRot);
			obj.transform.localScale = objectScale;

			renderCamera.orthographicSize = cameraProjectionSize;

			imageRect.sizeDelta = imageSize;
		}
	}

	public void OnClickOpenObj()
	{
		OnClickClear();

		GameObjectManager.ToyResource.Load(id);
		var obj = GameObjectManager.ToyResource.Get(id);
		this.obj = Instantiate(obj, origin);
		this.obj.transform.localScale = obj.transform.localScale;
		Init();
	}

	public void OnClickClear()
	{
		Destroy(obj);
		objectPos = Vector3.zero;
		objectRot = Vector3.zero;
		objectScale = Vector3.one;
	}

	public void OnClickSave()
	{
		//renderCamera.Render();

		//Texture2D texture = new Texture2D(Mathf.FloorToInt(imageSize.x), Mathf.FloorToInt(imageSize.y));
		//texture.ReadPixels(imageRect.rect, 0, 0);
		//texture.Apply();
		//var png = texture.EncodeToPNG();

		//if (!Directory.Exists(path))
		//{
		//	Directory.CreateDirectory(path);
		//}
		//var sb = new StringBuilder();
		//sb = sb.Append(path).Append("/Toy").Append(id).Append(".png");
		//Debug.Log(sb.ToString());
	}

	public void OnChangeID(string s) => id = int.Parse(s);

	public void OnChangeXPos(string s) => objectPos.x = float.Parse(s);
	public void OnChangeYPos(string s) => objectPos.y = float.Parse(s);
	public void OnChangeZPos(string s) => objectPos.z = float.Parse(s);

	public void OnChangeXRot(string s) => objectRot.x = float.Parse(s);
	public void OnChangeYRot(string s) => objectRot.y = float.Parse(s);
	public void OnChangeZRot(string s) => objectRot.z = float.Parse(s);

	public void OnChangeXScale(string s) => objectScale.x = float.Parse(s);
	public void OnChangeYScale(string s) => objectScale.y = float.Parse(s);
	public void OnChangeZScale(string s) => objectScale.z = float.Parse(s);

	public void OnChangeSize(string s) => cameraProjectionSize = float.Parse(s);

	public void OnChangeImageWidth(string s) => imageSize.x = float.Parse(s);
	public void OnChangeImageHeight(string s) => imageSize.y = float.Parse(s);
}
