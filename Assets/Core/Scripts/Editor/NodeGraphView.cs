using System;
using System.Linq;
using Core.Editor.Nodes;
using Core.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.Editor
{
    public class NodeGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<NodeGraphView, UxmlTraits> {}

        private NodeGraph m_NodeGraph;

        public NodeGraphView()
        {
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            GridBackground gridBackground = new GridBackground();
            Insert(0, gridBackground);
            gridBackground.StretchToParentSize();

            var styleSheet =
                AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Core/Scripts/Editor/NodeGraphEditorWindow.uss");
            styleSheets.Add(styleSheet);
        }

        internal void PopulateView(NodeGraph nodeGraph)
        {
            m_NodeGraph = nodeGraph;

            if (m_NodeGraph.rootNode == null)
            {
                m_NodeGraph.rootNode = ScriptableObject.CreateInstance<ResultNode>();
                m_NodeGraph.rootNode.name = nodeGraph.rootNode.GetType().Name;
                m_NodeGraph.rootNode.guid = GUID.Generate().ToString();
                m_NodeGraph.AddNode(m_NodeGraph.rootNode);
            }
            
            m_NodeGraph.nodes.ForEach(n => CreateAndAddNodeView(n));
        }

        private void CreateAndAddNodeView(CodeFunctionNode node)
        {
            Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(NodeView).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract).ToArray();

            foreach (Type type in types)
            {
                if (type.GetCustomAttributes(typeof(NodeType), false) is NodeType[] attrs && attrs.Length > 0)
                {
                    if (attrs[0].type == node.GetType())
                    {
                        NodeView nodeView = (NodeView)Activator.CreateInstance(type);
                        nodeView.node = node;
                        nodeView.viewDataKey = node.guid;
                        nodeView.style.left = node.position.x;
                        nodeView.style.top = node.position.y;
                        AddElement(nodeView);
                    }
                }
            }
        }
    }
}