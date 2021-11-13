using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.Editor
{
    public class NodeGraphEditorWindow : EditorWindow
    {
        private NodeGraph m_NodeGraph;
        public static void ShowWindow(NodeGraph nodeGraph)
        {
            NodeGraphEditorWindow window = GetWindow<NodeGraphEditorWindow>();
            window.SelectNodeGraph(nodeGraph);
            window.minSize = new Vector2(800, 600);
            window.titleContent = new GUIContent("NodeGraph");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) is NodeGraph nodeGraph)
            {
                ShowWindow(nodeGraph);
                return true;
            }

            return false;
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Core/Scripts/Editor/NodeGraphEditorWindow.uxml");
            visualTree.CloneTree(root);
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Core/Scripts/Editor/NodeGraphEditorWindow.uss");
            root.styleSheets.Add(styleSheet);
        }

        private void OnSelectionChange()
        {
            if (Selection.activeObject is NodeGraph nodeGraph)
            {
                SelectNodeGraph(nodeGraph);
            }
        }

        private void SelectNodeGraph(NodeGraph nodeGraph)
        {
            m_NodeGraph = nodeGraph;
        }
    }
}