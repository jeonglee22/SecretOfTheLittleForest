public abstract class ObjectResource
{
	public static readonly string FormatPath = "GameObjects/{0}";

	public abstract bool Load(int fileid);

	public abstract bool UnLoad();
}
