using System;

[Serializable]
public abstract class SaveData
{
	public int Version { get; protected set; }

	public abstract SaveData VersionUp();
}

[Serializable]
public class SaveDataV1 : SaveData
{
	public string PlayerName { get; set; } = "TEST";
	public Deck Deck { get; set; } = new Deck();
	public BattleType BattleType { get; set; } = BattleType.Normal;
	public int stageId = 1;
	public float unitLimit = 0;
	public float unitCount = 0;
	public float gold = 0;

	public SaveDataV1()
	{
		Version = 1;
	}

	public override SaveData VersionUp()
	{
		throw new NotImplementedException();
	}
}