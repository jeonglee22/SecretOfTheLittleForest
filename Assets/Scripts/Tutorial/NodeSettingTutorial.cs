using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeSettingTutorial : TutorialManager
{
    [Header("ExplainField")]
    public GameObject upExplainField;
    public GameObject downExplainField;
    public TextMeshProUGUI upExplainText;

    [Header("Arrows")]
    public GameObject unitLimitArrow;
    public GameObject downArrowPrefab;
    public GameObject upArrowPrefab;
    private GameObject nodeArrow;
    public GameObject unitRemainArrow;

    [Header("Nodes")]
    public List<Node> playerNodes;
    public Node movingNode;
    private Node touchedNode;
    private Node beforeNode;

    [Header("Managers")]
    public ReadyCanvasManager canvasManager;
    public SetObjectControl objectControl;
    public PlayLogic playLogic;

    [Header("Additionals")]
    public Canvas canvas;
    public RectTransform EmptyRect;
    public ScrollRect unitDeckRect;
    private RectTransform content;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        if (!IsTutorial)
            return;

        behaveFunc.Add(() => ExplainSetting());
        behaveFunc.Add(() => ExplainUnitLimit());
        behaveFunc.Add(() => ReadyMovingToy());
        behaveFunc.Add(() => ExplainMoving());
        behaveFunc.Add(() => ReadytoChangeCaptain());
        behaveFunc.Add(() => ChangeCaptain());
        behaveFunc.Add(() => ShowMovable());

        upExplainField.SetActive(true);
        downExplainField.SetActive(false);

        Instantiate(EmptyRect, upExplainField.transform);
        Instantiate(EmptyRect, upExplainField.transform);

        nodeArrow = Instantiate(downArrowPrefab, canvas.transform);
        nodeArrow.SetActive(false);

        SetTutorialText();
        content = unitDeckRect.content;
    }

    protected override void SetTutorialText()
    {
        upExplainText.text = DataTableManger.StageStringTable.GetTutorialString(textIndex);
    }

    private void ExplainSetting()
    {
        tutorialIndex++;
        textIndex++;
        SetTutorialText();
        unitLimitArrow.SetActive(true);
    }

    private void ExplainUnitLimit()
    {
        tutorialIndex++;
        textIndex++;
        SetTutorialText();
        unitLimitArrow.SetActive(false);
        SetArrowOnNode(nodeArrow, 2);
        
        isHoldingEvent = true;
    }

    private void ReadyMovingToy()
    {
        nodeArrow.SetActive(false);

        tutorialIndex++;
        textIndex++;
        SetTutorialText();

        objectControl.ChoosingNode = playerNodes[2];
        objectControl.MakeDragImage();
        playerNodes[2].Toy.gameObject.SetActive(false);

        SetArrowOnNode(nodeArrow, movingNode,true);
        unitRemainArrow.SetActive(true);
    }

    private void SetArrowOnNode(GameObject arrow, int nodeIndex, bool totalWindow = false)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(playerNodes[nodeIndex].transform.position);
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out Vector2 canvasPos);
        arrow.SetActive(true);
        tutorialRects.Insert(tutorialIndex, totalWindow ? blockCanvas.GetComponent<RectTransform>() : arrow.GetComponent<RectTransform>());
        canvasPos.y -= canvasRect.rect.height * 0.5f + 10f;
        arrow.GetComponent<RectTransform>().anchoredPosition = canvasPos;
    }
    private void SetArrowOnNode(GameObject arrow, Node node, bool totalWindow = false)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(node.transform.position);
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out Vector2 canvasPos);
        arrow.SetActive(true);
        tutorialRects.Insert(tutorialIndex, totalWindow ? blockCanvas.GetComponent<RectTransform>() : arrow.GetComponent<RectTransform>());
        canvasPos.y -= canvasRect.rect.height * 0.5f + 10f;
        arrow.GetComponent<RectTransform>().anchoredPosition = canvasPos;
    }

    private void ExplainMoving()
    {
        nodeArrow.SetActive(false);
        unitRemainArrow.SetActive(false);

        tutorialIndex++;
        textIndex++;
        SetTutorialText();

        var dragObject = objectControl.DragObject;
        Destroy(dragObject);
        playerNodes[2].Toy.gameObject.SetActive(true);
        playLogic.ClearNodes();

        SetArrowOnNode(nodeArrow, 1);
    }

    private void ReadytoChangeCaptain()
    {
        tutorialIndex++;
        textIndex++;
        SetTutorialText();
        playerNodes[1].Toy.kingCanvas.SetActive(true);
        tutorialRects.Insert(tutorialIndex, nodeArrow.GetComponent<RectTransform>());
    }

    private void ChangeCaptain()
    {
        tutorialIndex++;
        textIndex++;
        SetTutorialText();
        playerNodes[3].Toy.kingCanvas.SetActive(false);
        SetArrowOnNode(nodeArrow, 3);
    }

    private void ShowMovable()
    {
        touchedNode = playerNodes[3];
        playLogic.ChoosedNode = touchedNode;
        playLogic.ShowMovable(touchedNode.NodeIndex, 0);

        tutorialIndex++;
        textIndex++;
        SetTutorialText();
    }
}
