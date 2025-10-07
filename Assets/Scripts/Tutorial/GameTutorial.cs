using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTutorial : TutorialManager
{
    [Header("TutorialShownObjects")]
    public GameObject learnMovingPanel;
    public GameObject backToGame;
    public GameObject gameCanvas;
    public GameObject readyCanvas;

    [Header("ExplainField")]
    public GameObject upExplainField;
    public GameObject downExplainField;
    public TextMeshProUGUI upExplainText;

    [Header("GameNodes")]
    public List<Node> centerNodes;
    public List<Node> toyNodes;
    public List<Node> playNodes;

    [Header("Managers")]
    public BoardManager boardManager;
    public PlayLogic playLogic;
    public ToyControl toyControl;
    public ButtonFunctions buttonFunctions;
    public PlayManager playManager;

    [Header("Arrows")]
    public GameObject downArrowAtBack;
    public GameObject upStartButton;
    public GameObject seeingPlayer;
    public GameObject seeingEnemy;
    public GameObject TurnArrow;
    public GameObject finishTurnArrow;
    public GameObject downArrowPrefab;
    private GameObject nodeArrow;

    [Header("Additionals")]
    public GameObject EmptyRect;
    public Canvas canvas;
    public Image turnImage;

    private bool canTouchMap = false;
    private Node touchedNode;
    private Node beforeNode;

    private bool firstNodeTouch = false;
    private bool moveNodeTouch = false;
    private bool canUpdateTutorialTouch = true;
    private bool infiniteMove = false;
    private Color originTurnColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        if (!IsTutorial)
            return;

        behaveFunc.Add(() => ChangeToLearnMoving());
        behaveFunc.Add(() => CenterNodeExplain());
        behaveFunc.Add(() => StartExplainEach());
        behaveFunc.Add(() => PawnExplain());
        behaveFunc.Add(() => KingExplain());
        behaveFunc.Add(() => KnightExplain());
        behaveFunc.Add(() => BishopExplain());
        behaveFunc.Add(() => RookExplain());
        behaveFunc.Add(() => QueenExplain());
        behaveFunc.Add(() => StartPractice());
        behaveFunc.Add(() => TouchToy());
        behaveFunc.Add(() => MoveToy());
        behaveFunc.Add(() => EndPractice());
        behaveFunc.Add(() => StartGame());
        behaveFunc.Add(() => SeeingPlayerOn());
        behaveFunc.Add(() => SeeingPlayerOff());
        behaveFunc.Add(() => SeeingEnemyOn());
        behaveFunc.Add(() => SeeingEnemyOff());
        behaveFunc.Add(() => ShowTurnText());
        behaveFunc.Add(() => TouchPlayerToy());
        behaveFunc.Add(() => MovePlayerToy());
        behaveFunc.Add(() => ShowTurnDecrease());
        behaveFunc.Add(() => TouchPlayerMovedToy());
        behaveFunc.Add(() => ExplainPlayerMovedToy());
        behaveFunc.Add(() => TouchEndTurn());
        behaveFunc.Add(() => ExplainRemainTurn());
        behaveFunc.Add(() => TouchEnemyToy());
        behaveFunc.Add(() => ShowEnemyAttack());
        behaveFunc.Add(() => ShowEnemyAttack());
        behaveFunc.Add(() => TouchEndTurn());
        behaveFunc.Add(() => TouchPlayerAttackToy());
        behaveFunc.Add(() => MovePlayerAttackToy());

        learnMovingPanel.SetActive(true);

        nodeArrow = Instantiate(downArrowPrefab, canvas.transform);
        nodeArrow.SetActive(false);

        base.SetTutorialText();
    }

    protected override void SetTutorialText()
    {
        upExplainText.text = DataTableManger.StageStringTable.GetTutorialString(textIndex);
    }

    protected override void Update()
    {
        canUpdateTutorialTouch = true;

        if (canTouchMap && Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);
            var notTouch = false;
            switch (touch.phase)
            {
                case TouchPhase.Began:
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    notTouch = true;
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    break;
            }

            if (!notTouch)
            {
                TutorialMove(touch);
            }
        }

        if (moveNodeTouch && firstNodeTouch)
            infiniteMove = true;

        if (infiniteMove)
            canUpdateTutorialTouch = true;

        if (canUpdateTutorialTouch)
            base.Update();
    }

    private void TutorialMove(Touch touch)
    {
        var touchPos = touch.position;
        var ray = Camera.main.ScreenPointToRay(touchPos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerId.node))
        {
            var go = hit.collider.gameObject;
            beforeNode = touchedNode;
            touchedNode = go.GetComponent<Node>();
            if (touchedNode.State == NodeState.None)
            {
                canUpdateTutorialTouch = false;
                playLogic.ClearNodes();
            }
            else if ((!firstNodeTouch || infiniteMove) && touchedNode.State == NodeState.Player)
            {
                playLogic.ChoosedNode = touchedNode;
                playLogic.ShowMovable(touchedNode.NodeIndex, 0);
                firstNodeTouch = true;
            }
            else if (beforeNode != null && beforeNode.State == NodeState.Player &&
                touchedNode.State == NodeState.PlayerMove && (!moveNodeTouch || infiniteMove))
            {
                playLogic.ChoosedNode = touchedNode;
                playLogic.ClearNodes();

                toyControl.ToyMove(ref beforeNode, false, true);
                touchedNode.State = beforeNode.State;
                beforeNode.State = NodeState.None;
                touchedNode = null;
                moveNodeTouch = true;
            }
        }
        else
        {
            canUpdateTutorialTouch = false;
            playLogic.ClearNodes();
        }
    }

    private void ChangeToLearnMoving()
    {
        learnMovingPanel.SetActive(false);
        backToGame.SetActive(true);

        var enemies = boardManager.enemyStartNodes;
        foreach (var enemy in enemies)
        {
            if (enemy.Toy != null)
            {
                Destroy(enemy.Toy.gameObject);
                enemy.Toy = null;
                enemy.State = NodeState.None;
            }
        }

        gameCanvas.SetActive(false);
        readyCanvas.SetActive(false);
        upExplainField.SetActive(true);
        downExplainField.SetActive(false);

        tutorialIndex++;
        textIndex++;
        SetTutorialText();
    }

    private void CenterNodeExplain()
    {
        foreach( var node in centerNodes)
        {
            node.State = NodeState.Attack;
        }

        tutorialIndex++;
        textIndex++;
        SetTutorialText();
    }

    private void StartExplainEach()
    {
        foreach (var node in centerNodes)
        {
            node.State = NodeState.None;
        }

        textIndex++;
        tutorialIndex++;
        SetTutorialText();
    }

    private void PawnExplain()
    {
        toyNodes[0].State = NodeState.Attack;
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
    }
    private void KingExplain()
    {
        toyNodes[0].State = NodeState.Player;
        toyNodes[1].State = NodeState.Attack;
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
    }
    private void KnightExplain()
    {
        toyNodes[1].State = NodeState.Player;
        toyNodes[2].State = NodeState.Attack;
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
    }
    private void BishopExplain()
    {
        toyNodes[2].State = NodeState.Player;
        toyNodes[3].State = NodeState.Attack;
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
    }
    private void RookExplain()
    {
        toyNodes[3].State = NodeState.Player;
        toyNodes[4].State = NodeState.Attack;
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
    }
    private void QueenExplain()
    {
        toyNodes[4].State = NodeState.Player;
        toyNodes[5].State = NodeState.Attack;
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
    }

    private void StartPractice()
    {
        toyNodes[5].State = NodeState.Player;
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
        canTouchMap = true;
    }

    private void TouchToy()
    {
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
    }

    private void MoveToy()
    {
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
        downArrowAtBack.SetActive(true);
    }

    private void EndPractice()
    {
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
        learnMovingPanel.SetActive(false);
        backToGame.SetActive(false);
        readyCanvas.SetActive(true);

        foreach(var node in boardManager.allNodes)
        {
            if(node.Toy != null)
                Destroy(node.Toy.gameObject);
            node.Toy = null;
            node.State = NodeState.None;
        }
        boardManager.SetPlayerDeckOnNode();
        boardManager.SetEnemyToy();
        upStartButton.SetActive(true);
        canTouchMap = false;
    }

    private void StartGame()
    {
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
        Instantiate(EmptyRect, upExplainField.transform);
        Instantiate(EmptyRect, upExplainField.transform);
        upExplainText.transform.parent.SetAsLastSibling();
        upStartButton.SetActive(false);
        readyCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        seeingPlayer.SetActive(true);
    }

    private void SeeingPlayerOn()
    {
        playManager.ShowPlayerStats(true);
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
    }
    private void SeeingPlayerOff()
    {
        playManager.ShowPlayerStats(false);
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
        seeingPlayer.SetActive(false);
        seeingEnemy.SetActive(true);
    }

    private void SeeingEnemyOn()
    {
        playManager.ShowEnemyStats(true);
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
    }
    private void SeeingEnemyOff()
    {
        playManager.ShowEnemyStats(false);
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
        seeingEnemy.SetActive(false);
        TurnArrow.SetActive(true);
        upExplainText.transform.parent.SetAsFirstSibling();
    }

    private void ShowTurnText()
    {
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
        TurnArrow.SetActive(false);
        SetArrowOnNode(nodeArrow, 0);
        upExplainText.transform.parent.SetAsLastSibling();
    }

    private void SetArrowOnNode(GameObject arrow, int nodeIndex)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(playNodes[nodeIndex].transform.position);
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out Vector2 canvasPos);
        arrow.SetActive(true);
        tutorialRects.Insert(tutorialIndex,arrow.GetComponent<RectTransform>());
        canvasPos.y -= canvasRect.rect.height * 0.5f + 10f;
        arrow.GetComponent<RectTransform>().anchoredPosition = canvasPos;
    }

    private void TouchPlayerToy()
    {
        touchedNode = playNodes[0];
        playLogic.ChoosedNode = touchedNode;
        playLogic.ShowMovable(touchedNode.NodeIndex, 0);

        tutorialIndex++;
        SetArrowOnNode(nodeArrow, 1);
    }


    private void MovePlayerToy()
    {
        touchedNode = playNodes[1];
        playLogic.ChoosedNode = touchedNode;
        playLogic.ClearNodes();
        beforeNode = playNodes[0];
        toyControl.ToyMove(ref beforeNode, false, true);
        touchedNode.State = beforeNode.State;
        beforeNode.State = NodeState.None;
        touchedNode = null;
        moveNodeTouch = true;

        textIndex++;
        tutorialIndex++;
        SetTutorialText();
        nodeArrow.SetActive(false);
        originTurnColor = turnImage.color;
        turnImage.color = Color.gray;
        TurnArrow.SetActive(true);
    }

    private void ShowTurnDecrease()
    {
        textIndex++;
        tutorialIndex++;
        SetTutorialText();

        SetArrowOnNode(nodeArrow, 1);
        TurnArrow.SetActive(false);
    }

    private void TouchPlayerMovedToy()
    {
        touchedNode = playNodes[1];
        playLogic.ChoosedNode = touchedNode;
        touchedNode.Toy.IsMove = true;
        playLogic.ShowMovable(touchedNode.NodeIndex, 0);

        textIndex++;
        tutorialIndex++;
        SetTutorialText();
        nodeArrow.SetActive(false);
    }

    private void ExplainPlayerMovedToy()
    {
        playLogic.ClearNodes();
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
        finishTurnArrow.SetActive(true);
    }

    private void TouchEndTurn()
    {
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
        finishTurnArrow.SetActive(false);
        turnImage.color = originTurnColor;
    }

    private void MovePlayerAttackToy()
    {
        throw new NotImplementedException();
    }

    private void TouchPlayerAttackToy()
    {
        throw new NotImplementedException();
    }

    private void ShowEnemyAttack()
    {
        throw new NotImplementedException();
    }

    private void TouchEnemyToy()
    {
        throw new NotImplementedException();
    }

    private void ExplainRemainTurn()
    {
        throw new NotImplementedException();
    }
}
