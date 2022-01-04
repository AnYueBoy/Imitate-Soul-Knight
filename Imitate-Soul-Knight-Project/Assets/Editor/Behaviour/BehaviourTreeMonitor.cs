using System.Collections.Generic;
using UFramework.AI.BehaviourTree;
using UnityEditor;
using UnityEngine;

public class BehaviourTreeMonitor : EditorWindow {
    private List<Node> nodes;
    private GUIStyle nodeStyle;

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
    }

    private void OnGUI () {
        this.drawBehaviourTree ();
        this.drawNodes ();
        this.processEvent (Event.current);
        if (GUI.changed) Repaint ();
    }

    private void processEvent (Event e) {
        switch (e.type) {
            case EventType.MouseDown:
                if (e.button == 1) {
                    this.processContextMenu (e.mousePosition);
                }
                break;
            default:
                break;
        }

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

    private void drawNodes () {
        if (this.nodes != null) {
            foreach (Node node in nodes) {
                node.draw ();
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

        nodes.Add (new Node (mousePosition, 100, 120, nodeStyle));
    }
}