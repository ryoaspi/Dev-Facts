using UnityEngine;

namespace TheFundation.Runtime
{
    [CreateAssetMenu(
        fileName = "GoalsCollection",
        menuName = "TheFundation/Goals/Goals Collection")]
    public class GoalsCollection : ScriptableObject
    {
        public GoalDefinition[] m_Goals;

        public GoalDefinition Get(string key)
        {
            foreach (var g in m_Goals)
                if (g != null && g.m_Key == key)
                    return g;

            return null;
        }
    }
}