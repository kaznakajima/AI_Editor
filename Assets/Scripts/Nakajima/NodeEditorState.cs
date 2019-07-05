using UnityEngine;
using System;
using System.Collections.Generic;

namespace AI_EditorWindow
{
    public class NodeEditorState : ScriptableObject
    {
        public NodeCanvas canvas;
        public NodeEditorState parentEditor;

        public Node selectNode;
        [NonSerialized]
        public Node focusedNode;
        
    }
}
