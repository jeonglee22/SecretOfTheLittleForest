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
    //public TextMeshProUGUI statText;

    public Toy toy;

    private bool isSetEnemy = false;

	private void Start()
	{
		turnText.text = string.Empty;
        endText.text = string.Empty;
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
        turnText.text = turn switch
        {
            PlayTurn.Enemy => "Enemy1 Turn",
            PlayTurn.Player => "Player Turn",
            PlayTurn.EliteEnemy => "Enemy2 Turn",
            _ => "",
        };
    }

	public void SetEndText(bool isEnemyWin)
	{
        endText.text = isEnemyWin ? "Enemy Win" : "Player Win";
	}

    //public void SetStageStat(string s)
    //{
    //    statText.text = s;
    //}
}
