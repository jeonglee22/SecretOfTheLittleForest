using System;
using System.Collections.Generic;

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
	public float unitLimit = 12;
	public float unitCount = 8;
	public float gold = 0;
	public int Crystal = 0;
	public int StageCount = 0;
	public Scenes Scenes { get; set; } = Scenes.Game;
	public List<Room> Rooms { get; set; }

	public float masterPitch;
	public float bgmPitch;
	public float sfxPitch;

	public SaveDataV1()
	{
		Version = 1;
	}

	public override SaveData VersionUp()
	{
		throw new NotImplementedException();
	}
}