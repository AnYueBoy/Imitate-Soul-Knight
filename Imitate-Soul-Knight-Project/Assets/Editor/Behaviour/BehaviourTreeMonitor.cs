using System.Collections.Generic;
using UFramework.AI.BehaviourTree;
using UnityEditor;
using UnityEngine;

public class BehaviourTreeMonitor : EditorWindow {
    private List<Node> nodes;
    private List<Connection> connections;
    private GUIStyle nodeStyle;
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;
    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

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

        inPointStyle = new GUIStyle ();
        inPointStyle.normal.background = EditorGUIUtility.Load ("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load ("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset (4, 4, 12, 12);

        outPointStyle = new GUIStyle ();
        outPointStyle.normal.background = EditorGUIUtility.Load ("builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load ("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset (4, 4, 12, 12);
    }

    private void OnGUI () {
        this.drawBehaviourTree ();
        this.drawNodes ();
        this.drawConnections ();
        this.processNodesEvents (Event.current);
        this.processEvents (Event.current);
        if (GUI.changed) Repaint ();
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

    private void processEvents (Event e) {
        if (e.type != EventType.MouseDown) {
            return;
        }
        if (e.button == 0) {
            clearConnectionSelection ();
        }
        if (e.button == 1) {
            this.processContextMenu (e.mousePosition);
        }
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

    private void processContextMenu (Vector2 mousePosition) {
        GenericMenu genericMenu = new GenericMenu ();
        genericMenu.AddItem (new GUIContent ("Add Item"), false, () => onClickAddNode (mousePosition));
        genericMenu.ShowAsContext ();
    }

    private void onClickAddNode (Vector2 mousePosition) {
        if (nodes == null) {
            nodes = new List<Node> ();
        }
        nodes.Add (new Node (mousePosition, 100, 120, nodeStyle, inPointStyle, outPointStyle, onClickInPoint, onClickOutPoint));
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

    private void onClickRemoveConnection (Connection connection) {
        connections.Remove (connection);
    }

    private void createConnection () {
        if (connections == null) {
            connections = new List<Connection> ();
        }

        connections.Add (new Connection (selectedInPoint, selectedOutPoint, onClickRemoveConnection));
    }

    private void clearConnectionSelection () {
        selectedInPoint = selectedOutPoint = null;
    }

    private void drawBehaviourTree () {
        GameObject behaviourNode = Selection.activeGameObject;
        if (behaviourNode == null) {
            return;
        }

        BehaviourTreeRunner behaviourTreeRunner = behaviourNode.GetComponent<BehaviourTreeRunner> ();
        if (behaviourTreeRunner == null) {
            return;
        }

        // this.drawNodes ();
    }
}