using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AI_EditorWindow
{
    public class AI_Editor : EditorWindow
    {
        private static AI_Editor aiEdit;
        public static AI_Editor AIEdit { get { AssureEditor(); return aiEdit; } }
        public static void AssureEditor() { if (aiEdit == null) CreateEditor(); }

        

        // ウインドウの大きさ
        public Rect windowRect = new Rect(10f, 10f, 100f, 100f);

        void OnGUI()
        {
            BeginWindows();

            windowRect = GUI.Window(1, windowRect, OpenWindow, "Root");

            EndWindows();
        }

        void OpenWindow(int _windowID)
        {
            GUI.DragWindow();
        }

        [MenuItem("Window/AI Editor")]
        static void CreateEditor()
        {
            aiEdit = GetWindow<AI_Editor>();
            aiEdit.minSize = new Vector2(800f, 600f);

        }
    }

}

