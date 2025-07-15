using System;
using UnityEngine;

namespace TheFundation.Runtime
{
    public static class InputIconEvents
    {
        public static event Action OnInputSchemeChanged;

        public static void InvokeOnInputSchemeChanged()
        {
            OnInputSchemeChanged?.Invoke();
        }
    }
}
