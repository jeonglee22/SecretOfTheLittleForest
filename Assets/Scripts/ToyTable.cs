using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//[Serializable]
public class ToyData
{
    public int UnitID { get; set; }
    public int Movement { get; set; }
    public int ModelCode { get; set; }
    public string Name { get; set; }
    public int HP { get; set; }
    public int Attack { get; set; }
    public int Price { get; set; }
    public char Rating { get; set; }
    public int DefUnit { get; set; }

    //public Image Image { get; set; }

    public static bool operator==(ToyData data1, ToyData data2)
    {
        if(data1 == null || data2 == null) return false;

        if(data1.UnitID == data2.UnitID) return true;

        return false;
    }

    public static bool operator!=(ToyData data1, ToyData data2) { return !(data1 == data2); }

    public override string ToString()
    {
        return $"{UnitID} / {Movement} / {ModelCode} / {Name} / {HP} / {Attack} / {Price} / {Rating} / {DefUnit}";
    }
}

public class ToyTable : DataTable
{
    private readonly Dictionary<int, ToyData> table = new Dictionary<int, ToyData>();
    public int Count { get { return table.Count; } }
    public List<ToyData> Table { get { return table.Values.ToList(); } }

    public override void Load(string filename)
    {
        table.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<ToyData>(textAsset.text);
        foreach (var toy in list)
        {
            if (!table.ContainsKey(toy.UnitID))
            {
                //toy.Image = Resources.Load<Image>(string.Format("Images/Toy{0}", toy.ModelCode));
                table.Add(toy.UnitID, toy);
            }
            else
            {
                Debug.LogError("아이템 아이디 중복!");
            }
        }
    }

    public ToyData Get(int id)
    {
        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }

    public ToyData GetRandom()
    {
        var toyList = table.Values.ToList();
        return toyList[UnityEngine.Random.Range(0, toyList.Count)];
    }
}
