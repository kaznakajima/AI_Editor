using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using AI_EditorWindow.Utilities;

namespace AI_EditorWindow
{
    public enum NodeSide { LEFT = 4, TOP = 3, RIGHT = 2, BOTTOM = 1}
    public enum Direction { NONE, IN, OUT }
    public enum ConnectionShape { LINE, BEZIER }
    public enum ConnectionCount { SINGLE, MULTI, MAX }

    public class ConnectionPort : ScriptableObject
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
