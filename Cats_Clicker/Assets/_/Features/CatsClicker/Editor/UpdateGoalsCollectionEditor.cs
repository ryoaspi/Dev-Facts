using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using TheFundation.Runtime;

namespace CatsClicker.Editor
{
    public class UpdateGoalsCollectionEditor : EditorWindow
    {
        #region Constants

        private static readonly string _goalsFolderPath = "Assets/_/Database/CatsClicker/Goals";
        private static readonly string _collectionAssetPath = "Assets/_/Database/CatsClicker/Goals/CatsClickerGoalsCollection.asset";

        #endregion


        #region Menu

        [MenuItem("CatsClicker/Generate/Update Goals Collection")]
        public static void UpdateCollection()
        {
            // Vérifie si le dossier existe
            if (!AssetDatabase.IsValidFolder(_goalsFolderPath))
            {
                Debug.LogError($"[UpdateGoalsCollection] Le dossier {_goalsFolderPath} n'existe pas !");
                return;
            }

            // Recherche de tous les GoalDefinition dans le dossier
            string[] assets = AssetDatabase.FindAssets("t:GoalDefinition", new[] { _goalsFolderPath });

            List<GoalDefinition> goals = new();

            foreach (string guid in assets)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GoalDefinition goal = AssetDatabase.LoadAssetAtPath<GoalDefinition>(path);

                if (goal != null)
                    goals.Add(goal);
            }

            if (goals.Count == 0)
            {
                Debug.LogWarning("[UpdateGoalsCollection] Aucun GoalDefinition trouvé.");
                return;
            }

            // Cherche la GoalsCollection principale
            GoalsCollection collection = AssetDatabase.LoadAssetAtPath<GoalsCollection>(_collectionAssetPath);

            if (collection == null)
            {
                // Si elle n'existe pas, on la crée
                collection = ScriptableObject.CreateInstance<GoalsCollection>();
                AssetDatabase.CreateAsset(collection, _collectionAssetPath);
            }

            // Tri par ordre alphabétique (lisibilité)
            goals = goals.OrderBy(g => g.m_Key).ToList();

            // Mise à jour de la collection
            collection.m_goals = goals.ToArray();

            // Sauvegarde
            EditorUtility.SetDirty(collection);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"[CatsClicker] GoalsCollection mise à jour avec {goals.Count} objectifs.");
        }

        #endregion
    }
}
