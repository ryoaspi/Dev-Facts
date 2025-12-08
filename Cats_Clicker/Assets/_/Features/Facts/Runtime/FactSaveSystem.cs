using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEngine;
using CompressionLevel = System.IO.Compression.CompressionLevel;

namespace TheFundation.Runtime
{
    [Serializable] public class SerializableFact { public string key; public string TypeName; public string JsonValue; }
    [Serializable] public class SerializationWrapper { public int SaveVersion = 1; public List<SerializableFact> Facts; }
    [Serializable] public class PrimitiveWrapper<T> { public T value; public PrimitiveWrapper(T v) { value = v; } }

    public static class FactSaveSystem
    {
        static string FilePath => Path.Combine(Application.persistentDataPath, "facts_save.json.gz");
        static string SlotPath(int slot) => Path.Combine(Application.persistentDataPath, $"slot_{slot}.json.gz");

        public static void SaveToFile(FactDictionary d) => WriteGz(FilePath, SaveToJson(d));
        public static void LoadFromFile(FactDictionary d) { if (File.Exists(FilePath)) LoadFromJson(d, ReadGz(FilePath)); }

        public static void SaveToSlot(FactDictionary d, int slot) => WriteGz(SlotPath(slot), SaveToJson(d));
        public static void LoadFromSlot(FactDictionary d, int slot) { var p = SlotPath(slot); if (File.Exists(p)) LoadFromJson(d, ReadGz(p)); }
        public static void DeleteSlot(int slot) { var p = SlotPath(slot); if (File.Exists(p)) File.Delete(p); }
        public static bool SlotExist(int slot) => File.Exists(SlotPath(slot));

        public static string SaveToJson(FactDictionary dict)
        {
            var list = new List<SerializableFact>();
            foreach (var kv in dict.m_allFacts)
            {
                if (!kv.Value.IsPersistent) continue;
                var v = kv.Value.GetObjectValue();
                var t = kv.Value.valueType;
                string json = (t.IsPrimitive || t == typeof(string))
                    ? JsonUtility.ToJson(Activator.CreateInstance(typeof(PrimitiveWrapper<>).MakeGenericType(t), v))
                    : JsonUtility.ToJson(v);

                list.Add(new SerializableFact{ key = kv.Key, TypeName = t.AssemblyQualifiedName, JsonValue = json });
            }
            return JsonUtility.ToJson(new SerializationWrapper { SaveVersion = 1, Facts = list });
        }

        public static void LoadFromJson(FactDictionary dict, string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            var wrap = JsonUtility.FromJson<SerializationWrapper>(json);
            if (wrap?.Facts == null) return;

            foreach (var s in wrap.Facts)
            {
                var t = ResolveType(s.TypeName);
                if (t == null) continue;

                object val = (t.IsPrimitive || t == typeof(string))
                    ? GetWrappedValue(t, s.JsonValue)
                    : JsonUtility.FromJson(s.JsonValue, t);

                typeof(FactDictionary).GetMethod(nameof(FactDictionary.SetFact))!
                    .MakeGenericMethod(t)
                    .Invoke(dict, new object[] { s.key, val, FactDictionary.FactPersistence.Persistent });
            }
        }

        static Type ResolveType(string aqn) =>
            AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(aqn)).FirstOrDefault(t => t != null);

        static object GetWrappedValue(Type t, string json)
        {
            var wType = typeof(PrimitiveWrapper<>).MakeGenericType(t);
            var inst = JsonUtility.FromJson(json, wType);
            return wType.GetField("value").GetValue(inst);
        }

        static void WriteGz(string path, string json)
        {
            using var fs = File.Create(path);
            using var gz = new GZipStream(fs, CompressionLevel.Optimal);
            using var sw = new StreamWriter(gz);
            sw.Write(json);
        }

        static string ReadGz(string path)
        {
            using var fs = File.OpenRead(path);
            using var gz = new GZipStream(fs, CompressionMode.Decompress);
            using var sr = new StreamReader(gz);
            return sr.ReadToEnd();
        }
    }
}
