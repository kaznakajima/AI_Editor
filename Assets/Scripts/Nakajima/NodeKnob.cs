using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using AI_EditorWindow.Utilities;

namespace AI_EditorWindow
{
    /// <summary>
    /// ノードの辺を定義
    /// </summary>
    public enum NodeSlide { Left = 4, Top = 3, Right = 2, Bottom = 1 }

    /// <summary>
    /// 描画やラベリングを行う
    /// </summary>
    [System.Serializable]
    public class NodeKnob : ScriptableObject
    {
        public Node node;

        protected virtual GUIStyle labelStyle { get { return GUI.skin.label; } }
        [System.NonSerialized]
        protected internal Texture2D knobTex;

        protected virtual NodeSlide defaultSlide { get { return NodeSlide.Right; } }
        public NodeSlide slide;
        public float slidePos = 0;
        public float slideOffset = 0;

        protected void InitBase(Node _node, NodeSlide _slide, float _slidePos, string _knobName)
        {
            node = _node;
            slide = _slide;
            slidePos = _slidePos;
            name = _knobName;
            ReloadKnobTexture();
        }

        #region Knob Texture Loading

        internal void Check()
        {
            if (slide == 0) slide = defaultSlide;
            if(knobTex == null) ReloadKnobTexture();
        }

        protected void ReloadKnobTexture()
        {
            if (knobTex == null) throw new UnityException("Knob texture could not loaded!");

            if(slide != defaultSlide)
            {
                ResourceManager.SetDefaultResourcePath("Resources/");
                int rotationSteps = getRotationStepsAntiCW(defaultSlide, slide);

                ResourceManager.MemoryTexture memTex = ResourceManager.FindInMemory(knobTex);
                if(memTex != null)
                {
                    string[] mods = new string[memTex.modifications.Length + 1];
                    memTex.modifications.CopyTo(mods, 0);
                    mods[mods.Length - 1] = "Rotation :" + rotationSteps;
                    Texture2D knobTexInMemory = ResourceManager.GetTexture(memTex.path, mods);
                    if(knobTexInMemory != null)
                    {
                        knobTex = knobTexInMemory;
                    }
                    else
                    {
                        knobTex = RTEditorGUI.RotateTextureCCW(knobTex, rotationSteps);
                        ResourceManager.AddTextureToMemory(memTex.path, knobTex, mods.ToArray());
                    }
                }
                else
                {
                    knobTex = RTEditorGUI.RotateTextureCCW(knobTex, rotationSteps);
                }
            }
        }

        protected virtual void ReloadTexture()
        {
            knobTex = RTEditorGUI.ColorToTex(1, Color.red);
        }

        #endregion

        #region Addtional Serialization

        protected internal virtual ScriptableObject[] GetScriptableObjects() { return new ScriptableObject[0]; }

        protected internal virtual void CopyScriptableObjects(System.Func<ScriptableObject, ScriptableObject> _replaceSerializeObj) { }

        #endregion

        #region GUI drawing and Positioning

        public virtual void DrawKnob()
        {
            Rect knobRect = GetGUIKnob();
            GUI.DrawTexture(knobRect, knobTex);
        }

        public void DisplayLayout()
        {
            DisplayLayout(new GUIContent(name), labelStyle);
        }

        public void DisplayLayout(GUIStyle _style)
        {
            DisplayLayout(new GUIContent(name), _style);
        }

        public void DisplayLayout(GUIContent _content)
        {
            DisplayLayout(_content, labelStyle);
        }

        public void DisplayLayout(GUIContent _content, GUIStyle _style)
        {
            GUILayout.Label(_content, _style);
            if (Event.current.type == EventType.Repaint) SetPosition();
        }

        public void SetPosition(float _position, NodeSlide _slide)
        {
            if(slide != _slide)
            {
                slide = _slide;
                ReloadKnobTexture();
            }
            SetPosition(_position);
        }

        public void SetPosition(float _position)
        {
            slidePos = _position;
        }

        public void SetPosition()
        {
            Vector2 pos = GUILayoutUtility.GetLastRect().center + node.contentOffset;
            slidePos = slide == NodeSlide.Bottom || slide == NodeSlide.Top ? pos.x : pos.y;
        }

        #endregion

        #region Position requests

        internal Rect GetGUIKnob()
        {
            Check();
            Vector2 knobSize = new Vector2((knobTex.width / knobTex.height) * NodeEditorGUI.knobSize,
               (knobTex.height / knobTex.width) * NodeEditorGUI.knobSize);
            Vector2 knobCenter = new Vector2(node.rect.x + (slide == NodeSlide.Bottom || slide == NodeSlide.Top ?
                slidePos : (slide == NodeSlide.Left ? -slideOffset - knobSize.x / 2 :
                node.rect.width + slideOffset + knobSize.x / 2)),
                node.rect.y + (slide == NodeSlide.Left || slide == NodeSlide.Right ?
                slidePos : (slide == NodeSlide.Top ? -slideOffset - knobSize.y / 2 :
                node.rect.height + slideOffset + knobSize.y / 2)));
            return new Rect(knobCenter.x - knobSize.x / 2 + NodeEditor.curEditorState.zoomPanAdjust.x,
                knobCenter.y - knobSize.y / 2 + NodeEditor.curEditorState.zoomPanAdjust.y,
                knobSize.x, knobSize.y);
        }

        internal Rect GetScreenKnob()
        {
            Rect rect = GetGUIKnob();
            rect.position -= NodeEditor.curEditorState.zoomPanAdjust;
            return NodeEditor.CanvasGUIToScreenRect(rect);
        }

        internal Vector2 GetDirection()
        {
            return slide == NodeSlide.Right ? Vector2.right :
                (slide == NodeSlide.Bottom ? Vector2.up :
                (slide == NodeSlide.Top ? Vector2.down :
                Vector2.left));
        }

        private static int getRotationStepsAntiCW(NodeSlide _slideA, NodeSlide _slideB)
        {
            return _slideB - _slideA + (_slideA > _slideB ? 4 : 0);
        }

        #endregion
    }
}


