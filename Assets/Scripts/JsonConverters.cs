using Newtonsoft.Json;
using NUnit.Framework.Interfaces;
using System;
using UnityEngine;

//public class DeckConverter : JsonConverter<Deck>
//{
//	public override Deck ReadJson(JsonReader reader, Type objectType, Deck existingValue, bool hasExistingValue, JsonSerializer serializer)
//	{
//		var id = reader.Value as string;
//		return DataTableManger.ItemTable.Get(id);
//	}

//	public override void WriteJson(JsonWriter writer, Deck value, JsonSerializer serializer)
//	{
//		writer.WriteValue(value.Id);
//	}
//}
