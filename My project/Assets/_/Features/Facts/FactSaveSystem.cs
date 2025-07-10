using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TheFundation.Runtime
{
    [Serializable]
    public class SerializableFact
    {
        public string key;
        public string TypeName;
        public string JsonValue;
    }

    [Serializable]
    public class SerializationWrapper
    {
        public List<SerializableFact> Facts;
    }

    public static class FactSaveSystem
    {
        #region Utils

        private static string GetSlotFilePath(int slot)
        {
            return Path.Combine(Application.persistentDataPath, $"slot_{slot}.json");
        }

        public static void SaveToSlot(FactDictionary factDictionary, int slot)
        {
            string json = SaveToJson(factDictionary);
            string path = GetSlotFilePath(slot);
            File.WriteAllText(path, json);
        }

        public static void LoadFromSlot(FactDictionary factDictionary, int slot)
        {
            string path = GetSlotFilePath(slot);
            if (!File.Exists(path)) return;
            string json = File.ReadAllText(path);
            LoadFromJson(factDictionary, json);
        }

        public static bool SlotExist(int slot)
        {
            return File.Exists(GetSlotFilePath(slot));
        }
        
        public static void DeleteSlot(int slot)
        {
            string path = GetSlotFilePath(slot);
            if (File.Exists(path)) File.Delete(path);
        }

        public static List<int> GetAvailableSlots(int maxSlots = 10)
        {
            List<int> existingSlots = new();
            for (int i = 0; i < maxSlots; i++)
            {
                if (SlotExist(i)) existingSlots.Add(i);
            }
            return existingSlots;
        }

        public static string SaveToJson(FactDictionary factsDictionary)
        {
            List<SerializableFact> serializableFacts = new();

            foreach (var pair in factsDictionary.m_AllFacts)
            {
                if (pair.Value.IsPersistent)
                {
                    object value = pair.Value.GetObjectValue();
                    string jsonValue = JsonUtility.ToJson(value);
                    string typeName = value.GetType().AssemblyQualifiedName;

                    serializableFacts.Add(new SerializableFact
                    {
                        key = pair.Key,
                        TypeName = typeName,
                        JsonValue = jsonValue
                    });
                }
            }

            SerializationWrapper wrapper = new() { Facts = serializableFacts };
            return JsonUtility.ToJson(wrapper);
        }

        public static void LoadFromJson(FactDictionary factsDictionary, string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            SerializationWrapper wrapper = JsonUtility.FromJson<SerializationWrapper>(json);
            if (wrapper == null || wrapper.Facts == null) return;

            foreach (var sFact in wrapper.Facts)
            {
                Type type = Type.GetType(sFact.TypeName);
                if (type == null) continue;

                object value = JsonUtility.FromJson(sFact.JsonValue, type);
                
                var method = typeof(FactDictionary).GetMethod("SetFact")!.MakeGenericMethod(type);
                method.Invoke(factsDictionary, new object[] { sFact.key, value, FactDictionary.FactPersistence.Persistent });
            }
        }
        
        
        //Sauvegarde dans un fichier JSON
        public static void SaveToFile(FactDictionary factsDictionary)
        {
            string json = SaveToJson(factsDictionary);
            File.WriteAllText(_SaveFilePath, json);
        }
        
        // Chargement depuis un fichier JSON
        public static void LoadFromFile(FactDictionary factsDictionary)
        {
            if (!File.Exists(_SaveFilePath)) return;
            string json = File.ReadAllText(_SaveFilePath);
            LoadFromJson(factsDictionary, json);
        }
        #endregion
        
        
        
        #region Private And Protected
        
        private static readonly string _SaveFilePath = Path.Combine(Application.persistentDataPath, "facts_save.json");

        #endregion
    }
}
