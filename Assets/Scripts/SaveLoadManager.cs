using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
using SaveDataVC = SaveDataV1;

public class SaveLoadManager
{
    public static int SaveDataVersion { get; } = 1;

    static SaveLoadManager()
    {
        Load();
    }

    public static SaveDataVC Data { get; set; } = new SaveDataVC();

    public static readonly string[] SaveFileName =
    {
        "UserTotalDeck.json",
        "StageDeck.json",
		"StageInfo.json",

		"Save2.json",
        "Save3.json"
        
    };

    public static string SaveDirectory => $"{Application.persistentDataPath}/Save";

    private static JsonSerializerSettings settings = new JsonSerializerSettings()
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.All,
    };

    public static bool Save(int slot = 0)
    {
        if (Data == null || slot < 0 || slot >= SaveFileName.Length)
            return false;

        try
        {
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }

            var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
            var json = JsonConvert.SerializeObject(Data, settings);

            // 암호화 과정 필요
            // byte 변환, 압축, 암호화

            File.WriteAllText(path, json);

            return true;
        }
        catch
        {
            Debug.Log("Save 예외 발생");
            return false;
        }
    }

    public static bool Load(int slot = 0)
    {
        if (slot < 0 || slot >= SaveFileName.Length)
            return false;

        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);

        if (!File.Exists(path))
            return false;

        try
        {
            var json = File.ReadAllText(path);
            var saveData = JsonConvert.DeserializeObject<SaveData>(json, settings);
            //Data = JsonConvert.DeserializeObject<SaveDataVC>(json, settings);

            // 복호화 과정 필요
            // 복호화, 압축해제, string 변환
            while (saveData.Version < SaveDataVersion)
            {
                saveData = saveData.VersionUp();
            }

            Data = saveData as SaveDataVC;
            return true;
        }
        catch
        {
            Debug.Log("Load 예외 발생");
            return false;
        }
    }
}
