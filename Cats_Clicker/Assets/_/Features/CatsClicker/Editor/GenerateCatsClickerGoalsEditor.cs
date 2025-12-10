using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using TheFundation.Runtime; // GoalDefinition, GoalsCollection

namespace CatsClicker.Editor
{
    public class GenerateCatsClickerGoalsEditor : EditorWindow
    {
        #region Publics

        private static readonly string _folderPath = "Assets/_/Database/CatsClicker/Goals";

        // Liste des objectifs à générer automatiquement
        private static readonly (string key, GoalCategory cat, string label, string desc, int target)[] _goals =
        {
            // Tutorial
            ("cc_first_click",       GoalCategory.Tutorial,    "Premier clic",           "Faites votre premier clic !", 1),
            ("cc_buy_first_cat",     GoalCategory.Tutorial,    "Premier chat",           "Achetez votre premier chat.", 1),
            ("cc_cat_level5",        GoalCategory.Tutorial,    "Chat niveau 5",          "Atteignez le niveau 5.", 5),
            ("cc_buy_first_upgrade", GoalCategory.Tutorial,    "Premier upgrade",        "Achetez votre premier upgrade.", 1),

            // Progression
            ("cc_collect_100",       GoalCategory.Progression, "100 croquettes",         "Collectez 100 croquettes.", 100),
            ("cc_collect_1000",      GoalCategory.Progression, "1 000 croquettes",       "Collectez 1000 croquettes.", 1000),
            ("cc_collect_10000",     GoalCategory.Progression, "10 000 croquettes",      "Collectez 10 000 croquettes.", 10000),

            ("cc_cat_level10",       GoalCategory.Progression, "Chat niveau 10",         "Atteignez le niveau 10.", 10),
            ("cc_cat_level25",       GoalCategory.Progression, "Chat niveau 25",         "Atteignez le niveau 25.", 25),

            ("cc_upgrade_5",         GoalCategory.Progression, "5 upgrades",             "Achetez 5 upgrades.", 5),
            ("cc_upgrade_10",        GoalCategory.Progression, "10 upgrades",            "Achetez 10 upgrades.", 10),
            ("cc_mult2",             GoalCategory.Progression, "Production x2",          "Atteignez un multiplicateur global x2.", 2),

            // Story / Prestige
            ("cc_first_prestige",    GoalCategory.Story,       "Premier prestige",       "Réalisez votre premier prestige.", 1),
            ("cc_poulets10",         GoalCategory.Story,       "10 poulets",             "Gagnez 10 poulets au total.", 10),
            ("cc_prestige5",         GoalCategory.Story,       "5 prestiges",            "Effectuez 5 prestiges.", 5),

            // Collection
            ("cc_3_chats",           GoalCategory.Collectible, "3 chats débloqués",      "Débloquez 3 chats différents.", 3),
            ("cc_5_chats",           GoalCategory.Collectible, "5 chats débloqués",      "Débloquez 5 chats différents.", 5),

            // Exploration / Click
            ("cc_click100",          GoalCategory.Exploration, "100 clics",              "Réalisez 100 clics.", 100),
            ("cc_click1000",         GoalCategory.Exploration, "1000 clics",             "Réalisez 1000 clics.", 1000),
        };

        #endregion


        #region Unity Menu

        [MenuItem("CatsClicker/Generate/Generate All Goals")]
        public static void GenerateAllGoals()
        {
            // Crée le dossier s'il n'existe pas
            if (!AssetDatabase.IsValidFolder(_folderPath))
            {
                string[] parts = _folderPath.Split('/');
                string path = parts[0];

                for (int i = 1; i < parts.Length; i++)
                {
                    string subPath = path + "/" + parts[i];
                    if (!AssetDatabase.IsValidFolder(subPath))
                    {
                        AssetDatabase.CreateFolder(path, parts[i]);
                    }

                    path = subPath;
                }
            }

            List<GoalDefinition> createdGoals = new();

            // ------------------------------
            // Génération des GoalDefinitions
            // ------------------------------
            foreach (var g in _goals)
            {
                string filePath = $"{_folderPath}/{g.key}.asset";

                // Optionnel : éviter d'écraser si l'asset existe déjà
                GoalDefinition goal = AssetDatabase.LoadAssetAtPath<GoalDefinition>(filePath);
                if (goal == null)
                {
                    goal = ScriptableObject.CreateInstance<GoalDefinition>();
                    AssetDatabase.CreateAsset(goal, filePath);
                }

                goal.m_Key = g.key;
                goal.m_Category = g.cat;
                goal.m_LabelKey = g.label;
                goal.m_DescriptionKey = g.desc;
                goal.m_TargetValue = g.target;

                EditorUtility.SetDirty(goal);
                createdGoals.Add(goal);
            }

            // ------------------------------
            // Génération / mise à jour de la GoalsCollection
            // ------------------------------
            string collectionPath = $"{_folderPath}/CatsClickerGoalsCollection.asset";
            GoalsCollection collection = AssetDatabase.LoadAssetAtPath<GoalsCollection>(collectionPath);

            if (collection == null)
            {
                collection = ScriptableObject.CreateInstance<GoalsCollection>();
                AssetDatabase.CreateAsset(collection, collectionPath);
            }

            collection.m_goals = createdGoals.ToArray();
            EditorUtility.SetDirty(collection);

            // Sauvegarde et refresh
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"[CatsClicker] Génération terminée : {createdGoals.Count} objectifs + GoalsCollection.");
        }

        #endregion
    }
}
