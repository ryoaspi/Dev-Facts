using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using TheFundation.Runtime;

namespace CatsClicker.Editor
{
    public class CreateGoalChainEditor : EditorWindow
    {
        #region Fields

        private string _chainName = "cc_new_chain";
        private Vector2 _scroll;

        private static readonly string _goalsFolderPath = "Assets/_/Database/CatsClicker/Goals";

        private List<GoalDefinition> _allGoals = new();
        private List<GoalDefinition> _steps = new();

        #endregion


        #region Unity Menu

        [MenuItem("CatsClicker/Generate/Create GoalChain")]
        public static void OpenWindow()
        {
            var window = GetWindow<CreateGoalChainEditor>("Create GoalChain");
            window.minSize = new Vector2(450, 500);
            window.LoadGoals();
        }

        #endregion


        #region Main UI

        private void OnGUI()
        {
            EditorGUILayout.Space();

            // --------------------------
            // Chain Name
            // --------------------------
            EditorGUILayout.LabelField("GoalChain Settings", EditorStyles.boldLabel);
            _chainName = EditorGUILayout.TextField("Chain Key", _chainName);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Steps", EditorStyles.boldLabel);

            // --------------------------
            // Display Steps List
            // --------------------------
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.Height(300));

            for (int i = 0; i < _steps.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                _steps[i] = (GoalDefinition)EditorGUILayout.ObjectField(_steps[i], typeof(GoalDefinition), false);

                if (GUILayout.Button("X", GUILayout.Width(25)))
                {
                    _steps.RemoveAt(i);
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();

            // --------------------------
            // Add Step Dropdown
            // --------------------------
            if (_allGoals.Count > 0)
            {
                if (GUILayout.Button("Add Step"))
                {
                    GenericMenu menu = new GenericMenu();

                    foreach (var goal in _allGoals)
                    {
                        menu.AddItem(new GUIContent(goal.m_Key), false, () =>
                        {
                            _steps.Add(goal);
                        });
                    }

                    menu.ShowAsContext();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Aucun GoalDefinition trouvé dans le dossier.", MessageType.Warning);
            }

            EditorGUILayout.Space(20);

            // --------------------------
            // Generate Chain Button
            // --------------------------
            GUI.color = Color.green;
            if (GUILayout.Button("Generate Chain", GUILayout.Height(40)))
            {
                GenerateChain();
            }
            GUI.color = Color.white;
        }

        #endregion


        #region Logic

        private void LoadGoals()
        {
            _allGoals.Clear();

            string[] guids = AssetDatabase.FindAssets("t:GoalDefinition", new[] { _goalsFolderPath });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GoalDefinition goal = AssetDatabase.LoadAssetAtPath<GoalDefinition>(path);

                if (goal != null)
                    _allGoals.Add(goal);
            }
        }


        private void GenerateChain()
        {
            if (_steps.Count == 0)
            {
                Debug.LogError("[GoalChain] Impossible de générer : aucune étape.");
                return;
            }

            if (string.IsNullOrWhiteSpace(_chainName))
            {
                Debug.LogError("[GoalChain] Le nom de la chaîne est vide.");
                return;
            }

            // Création du GoalChainDefinition
            GoalChainDefinition chain = ScriptableObject.CreateInstance<GoalChainDefinition>();
            chain.m_ChainKey = _chainName;
            chain.m_steps = _steps.ToArray();

            string assetPath = $"{_goalsFolderPath}/{_chainName}.asset";

            AssetDatabase.CreateAsset(chain, assetPath);

            EditorUtility.SetDirty(chain);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"[CatsClicker] GoalChain '{_chainName}' générée avec {_steps.Count} étapes !");
        }

        #endregion
    }
}
