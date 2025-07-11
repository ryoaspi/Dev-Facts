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

    [Serializable]
    public class PrimitiveWrapper<T>
    {
        public T value;

        public PrimitiveWrapper(T val)
        {
            value = val;
        }
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
                if (!pair.Value.IsPersistent) continue;
                
                object value = pair.Value.GetObjectValue();
                Type valueType = value.GetType();

                string jsonValue;

                if (valueType.IsPrimitive || value is string)
                {
                    Type wrapperType = typeof(PrimitiveWrapper<>).MakeGenericType(valueType);
                    object wrapper = Activator.CreateInstance(wrapperType, value);
                    jsonValue = JsonUtility.ToJson(wrapper);
                }

                else
                {
                    jsonValue = JsonUtility.ToJson(value);
                }
                
                serializableFacts.Add(new SerializableFact
                {
                    key = pair.Key,
                    TypeName = valueType.FullName,
                    JsonValue = jsonValue
                });
                
            }

            SerializationWrapper wrapperObj = new() { Facts = serializableFacts };
            return JsonUtility.ToJson(wrapperObj);
        }

        public static void LoadFromJson(FactDictionary factsDictionary, string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            
            
            
            SerializationWrapper wrapper = JsonUtility.FromJson<SerializationWrapper>(json);
            if (wrapper?.Facts== null) return;

            foreach (var sFact in wrapper.Facts)
            {
                Type type = Type.GetType(sFact.TypeName);
                if (type == null)
                {
                    continue;
                }

                object value;

                if (type.IsPrimitive || type == typeof(string))
                {
                    Type wrapperType = typeof(PrimitiveWrapper<>).MakeGenericType(type);
                    object wrapperInstance = JsonUtility.FromJson(sFact.JsonValue, wrapperType);
                    value = wrapperType.GetField("value").GetValue(wrapperInstance);
                }

                else
                {
                    value = JsonUtility.FromJson(sFact.JsonValue, type);
                }
                
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
