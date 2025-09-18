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

	public SaveDataV1()
	{
		Version = 1;
	}

	public override SaveData VersionUp()
	{
		throw new NotImplementedException();
	}
}