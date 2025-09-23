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

    public TextMeshProUGUI fpsText;
    private float fpsreloadTime;
    private float fpsreloadTimeMax = 0.3f;

    private float initFontSize;
    //public TextMeshProUGUI statText;

    public Toy toy;

    private bool isSetEnemy = false;
    private float speed = 500f;

    private int enemyTurn = 1;
    private int playerTurn = 0;

	private void Start()
	{
		turnText.text = string.Empty;
        endText.text = string.Empty;
        initFontSize = turnText.fontSize;
	}

	private void Update()
	{
        if(fpsreloadTime < fpsreloadTimeMax)
        {
            fpsreloadTime += Time.deltaTime;
            return;
        }    

        fpsText.text = $"FPS: {1f / Time.deltaTime:F0} ({Time.deltaTime:F5} ms)";
        fpsreloadTime = 0f;
	}

    public void SetTurnText(PlayTurn turn)
    {
        turnText.gameObject.SetActive(true);
        turnText.text = turn switch
        {
            PlayTurn.Enemy => DataTableManger.StageStringTable.GetStageString(enemyTurn),
            PlayTurn.Player => DataTableManger.StageStringTable.GetStageString(playerTurn),
            PlayTurn.EliteEnemy => DataTableManger.StageStringTable.GetStageString(enemyTurn),
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
            turnText.fontSize += speed * Time.deltaTime;
            yield return null;

            if (turnText.fontSize >= Screen.height * 0.15f)
                break;
        }
        yield return new WaitForSeconds(1f);
        turnText.gameObject.SetActive(false);
        turnText.fontSize = initFontSize;
        playManager.IsTurnShown = false;
    }
}
