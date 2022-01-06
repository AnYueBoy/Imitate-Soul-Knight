using System.Collections.Generic;
using UFramework.AI.BehaviourTree;
using UnityEditor;
using UnityEngine;

public class BehaviourTreeMonitor : EditorWindow {
    private List<Node> nodes = new List<Node> ();
    private List<Connection> connections;
    private GUIStyle nodeStyle;
    private GUIStyle selectedNodeStyle;
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;
    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;
    private Vector2 offset;
    private Vector2 drag;

    [MenuItem ("UFramework/BehaviourTreeMonitor")]
    private static void ShowWindow () {
        var window = GetWindow<BehaviourTreeMonitor> ();
        window.titleContent = new GUIContent ("BehaviourTreeMonitor");
        window.Show ();
    }

    private void OnEnable () {
        nodeStyle = new GUIStyle ();
        Texture2D texture = EditorGUIUtility.Load ("builtin skins/darkskin/images/node1.png") as Texture2D;
        nodeStyle.normal.background = texture;
        nodeStyle.border = new RectOffset (12, 12, 12, 12);
        nodeStyle.alignment = TextAnchor.MiddleCenter;
        nodeStyle.normal.textColor = Color.green;

        selectedNodeStyle = new GUIStyle ();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load ("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset (12, 12, 12, 12);
        selectedNodeStyle.alignment = TextAnchor.MiddleCenter;

        inPointStyle = new GUIStyle ();
        inPointStyle.normal.background = EditorGUIUtility.Load ("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load ("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset (4, 4, 12, 12);
        inPointStyle.alignment = TextAnchor.MiddleCenter;

        outPointStyle = new GUIStyle ();
        outPointStyle.normal.background = EditorGUIUtility.Load ("builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load ("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset (4, 4, 12, 12);
        outPointStyle.alignment = TextAnchor.MiddleCenter;
    }

    private void OnGUI () {
        this.drawBehaviourTree ();
        this.drawGrid (20, 0.2f, Color.gray);
        this.drawGrid (100, 0.4f, Color.gray);
        this.drawNodes ();
        this.drawConnections ();
        this.drawConnectionLine (Event.current);
        this.processNodesEvents (Event.current);
        this.processEvents (Event.current);
        if (GUI.changed) Repaint ();
    }

    private void drawGrid (float gridSpacing, float gridOpacity, Color gridColor) {
        int widthDivs = Mathf.CeilToInt (position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt (position.height / gridSpacing);
        Handles.BeginGUI ();
        Handles.color = new Color (gridColor.r, gridColor.g, gridColor.b, gridOpacity);
        offset += drag / 2;
        Vector3 newOffset = new Vector3 (offset.x % gridSpacing, offset.y % gridSpacing, 0);
        for (int i = 0; i < widthDivs; i++) {
            Handles.DrawLine (new Vector3 (gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3 (gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int i = 0; i < heightDivs; i++) {
            Handles.DrawLine (new Vector3 (-gridSpacing, gridSpacing * i, 0) + newOffset, new Vector3 (position.width, gridSpacing * i, 0) + newOffset);
        }
        Handles.color = Color.white;
        Handles.EndGUI ();
    }

    private void drawNodes () {
        if (this.nodes == null) {
            return;
        }

        for (int i = 0; i < nodes.Count; i++) {
            nodes[i].draw ();
        }
    }

    private void drawConnections () {
        if (connections == null) {
            return;
        }

        for (int i = 0; i < connections.Count; i++) {
            connections[i].draw ();
        }
    }

    private void drawConnectionLine (Event e) {
        if (selectedInPoint != null && selectedOutPoint == null) {
            Handles.DrawBezier (
                selectedInPoint.rect.center,
                e.mousePosition,
                selectedInPoint.rect.center + Vector2.down * 50f,
                e.mousePosition - Vector2.down * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (selectedOutPoint != null && selectedInPoint == null) {
            Handles.DrawBezier (
                selectedOutPoint.rect.center,
                e.mousePosition,
                selectedOutPoint.rect.center - Vector2.down * 50f,
                e.mousePosition + Vector2.down * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }
    }

    private void processEvents (Event e) {
        drag = Vector2.zero;
        switch (e.type) {
            case EventType.MouseDown:
                if (e.button == 0) {
                    clearConnectionSelection ();
                }
                break;

            case EventType.MouseDrag:
                if (e.button == 0) {
                    onDrag (e.delta);
                }
                break;
        }
    }

    private void onDrag (Vector2 delta) {
        drag = delta;
        if (nodes != null) {
            for (int i = 0; i < nodes.Count; i++) {
                nodes[i].drag (delta);
            }
        }
        GUI.changed = true;
    }

    private void processNodesEvents (Event e) {
        if (nodes == null) {
            return;
        }

        for (int i = nodes.Count - 1; i >= 0; i--) {
            bool guiChanged = nodes[i].processEvents (e);
            if (guiChanged) {
                GUI.changed = true;
            }
        }
    }

    private void onClickInPoint (ConnectionPoint inPoint) {
        selectedInPoint = inPoint;
        if (selectedOutPoint == null) {
            return;
        }

        if (selectedOutPoint.node != selectedInPoint.node) {
            this.createConnection ();
        }
        this.clearConnectionSelection ();
    }
    private void onClickOutPoint (ConnectionPoint outPoint) {
        selectedOutPoint = outPoint;
        if (selectedInPoint == null) {
            return;
        }
        if (selectedOutPoint.node != selectedInPoint.node) {
            this.createConnection ();
        }
        this.clearConnectionSelection ();
    }

    private void createConnection () {
        if (connections == null) {
            connections = new List<Connection> ();
        }

        connections.Add (new Connection (selectedInPoint, selectedOutPoint));
    }

    private void clearConnectionSelection () {
        selectedInPoint = selectedOutPoint = null;
    }

    private BehaviourTreeRunner curBehaviourTreeRunner;
    private void drawBehaviourTree () {
        if (!Application.isPlaying) {
            return;
        }
        GameObject behaviourNode = Selection.activeGameObject;
        if (behaviourNode == null) {
            return;
        }

        BehaviourTreeRunner behaviourTreeRunner = behaviourNode.GetComponent<BehaviourTreeRunner> ();
        if (behaviourTreeRunner == null) {
            return;
        }

        if (curBehaviourTreeRunner != behaviourTreeRunner) {
            curBehaviourTreeRunner = behaviourTreeRunner;
            this.nodes.Clear ();
            this.buildTree (behaviourTreeRunner.rootNode, 0, 0, null);
        }

    }

    private void buildTree (BaseNode btNode, int layer, int childIndex, Node parent) {
        Node rootNode = this.createNode (btNode, layer, childIndex, parent);
        if (btNode.children.Count <= 0) {
            return;
        }
        layer++;
        for (int i = 0; i < btNode.children.Count; i++) {
            this.buildTree (btNode.children[i], layer, i, rootNode);
        }
    }

    private readonly float verticalInterval = 200f;
    private readonly float horizontalInterval = 600f;
    private Node createNode (BaseNode btNode, int layer, int childIndex, Node parent) {
        int totalChildCount = 0;
        float rootNodeX = 0;
        if (btNode.parent != null) {
            totalChildCount = btNode.parent.children.Count;
            rootNodeX = parent.rect.x;
        }
        int midValue = totalChildCount / 2;

        bool isDouble = false;
        if (totalChildCount != 0 && totalChildCount % 2 == 0) {
            isDouble = true;
        }
        float nodeXValue = 0;
        float realHorizontalInterval = horizontalInterval;
        if (layer > 1) {
            // FIXME: 寻找新方法调整节点间距
            realHorizontalInterval = horizontalInterval - 80 * layer;
        }
        if (isDouble) {
            nodeXValue = rootNodeX + (childIndex - midValue) * realHorizontalInterval + realHorizontalInterval / 2;
        } else {
            nodeXValue = rootNodeX + (childIndex - midValue) * realHorizontalInterval;
        }

        Node rootNode = new Node (new Vector2 (nodeXValue, layer * verticalInterval), 100, 120, btNode.GetType ().Name, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, onClickInPoint, onClickOutPoint);
        nodes.Add (rootNode);
        return rootNode;
    }
}