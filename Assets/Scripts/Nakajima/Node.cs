using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

using AI_EditorWindow;

namespace AI_EditorWindow
{
    public abstract class Node : ScriptableObject
    {
        public Rect rect = new Rect();
        internal Vector2 contentOffset = Vector2.zero;
        //[SerializeField]
        //public List<>
    }
}

