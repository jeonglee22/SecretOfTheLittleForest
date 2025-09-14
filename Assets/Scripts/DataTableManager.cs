using System.Collections.Generic;
using UnityEngine;

public static class DataTableManger
{
    private static readonly Dictionary<string, DataTable> tables =
        new Dictionary<string, DataTable>();

    static DataTableManger()
    {
        Init();
    }

    private static void Init()
    {
        var toyTable = new ToyTable();
        toyTable.Load(DataTableIds.Toy);
        tables.Add(DataTableIds.Toy, toyTable);
    }

    public static ToyTable ToyTable
    {
        get
        {
            return Get<ToyTable>(DataTableIds.Toy);
        }
    }

    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
        {
            Debug.LogError("테이블 없음");
            return null;
        }
        return tables[id] as T;
    }
}
