using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTutorial : TutorialManager
{
    public GameObject learnMovingPanel;
    public GameObject backToGame;

    public GameObject gameCanvas;
    public GameObject readyCanvas;

    public GameObject upExplainField;
    public GameObject downExplainField;
    public TextMeshProUGUI upExplainText;

    public List<Node> centerNodes;

    public List<Node> toyNodes;

    public BoardManager boardManager;
    public PlayLogic playLogic;
    public ToyControl toyControl;

    public GameObject downArrowAtBack;

    private bool canTouchMap = false;
    private Node touchedNode;
    private Node beforeNode;

    private bool firstNodeTouch = false;
    private bool moveNodeTouch = false;
    private bool canUpdateTutorialTouch = true;
    private bool infiniteMove = false;

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

        learnMovingPanel.SetActive(true);

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
        learnMovingPanel.SetActive(false);
        backToGame.SetActive(false);
        readyCanvas.SetActive(true);
        boardManager.SetEnemyToy();
    }
}
