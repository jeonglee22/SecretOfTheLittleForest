using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayLogic playLogic;
    public ToyControl toyControl;
    public BoardManager boardManager;
    public PlayManager playManager;

    public TMP_Dropdown dropdown;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI endText;

    private float initFontSize;
    //public TextMeshProUGUI statText;

    public Toy toy;

    private bool isSetEnemy = false;

	private void Start()
	{
		turnText.text = string.Empty;
        endText.text = string.Empty;
        initFontSize = turnText.fontSize;
        //statText.text = string.Empty;

        OnClickSetEnemyToy();
	}

	public void OnValueChangeMoveType()
    {
        var moveType = (MoveType)dropdown.value;
        playLogic.ClearNodes();
    }

    public void OnClickSetPlayerToy()
    {
        var node = boardManager.GetRandomNodeInPlayer();
        do
        {
            toy.Data = DataTableManger.ToyTable.GetRandom();
        } while (toy.Data.Movement == 0);
		GameObjectManager.ToyResource.Load(toy.Data.ModelCode.ToString());
		if (node != null )
            boardManager.ToySettingOnNode( node, toy, false);
    }
    public void OnClickSetEnemyToy()
    {
        if (isSetEnemy)
            return ;

        isSetEnemy = true;
        var nodeTuples = boardManager.SetEnemyStageData();
        for(int i = 0; i < nodeTuples.Count; i++)
        {
            toy.Data = nodeTuples[i].data;
            boardManager.ToySettingOnNode(nodeTuples[i].node, toy, true, i);
        }
    }

    public void SetTurnText(PlayTurn turn)
    {
        turnText.gameObject.SetActive(true);
        turnText.text = turn switch
        {
            PlayTurn.Enemy => "상대 턴",
            PlayTurn.Player => "나의 턴",
            PlayTurn.EliteEnemy => "상대 턴",
            _ => "",
        };
        turnText.color = turn switch
        {
            PlayTurn.Enemy => Color.red,
            PlayTurn.Player => Color.blue,
            PlayTurn.EliteEnemy => new Color(0f, 100f / 255f, 0f, 1f),
            _ => Color.white,
        };
        turnText.outlineColor = Color.white;
        turnText.outlineWidth = 0.3f;
        StartCoroutine(CoChangeTextSize());
	}

    private IEnumerator CoChangeTextSize()
    {
        playManager.IsTurnShown = true;
        while(true)
        {
            turnText.fontSize += 5f;
            yield return new WaitForSeconds(0.001f);

            if (turnText.fontSize >= Screen.height * 0.15f)
                break;
        }
        yield return new WaitForSeconds(1f);
        turnText.gameObject.SetActive(false);
        turnText.fontSize = initFontSize;
        playManager.IsTurnShown = false;
    }

	public void SetEndText(bool isEnemyWin)
	{
		endText.text = isEnemyWin ? "Enemy Win" : "Player Win";
	}
}
