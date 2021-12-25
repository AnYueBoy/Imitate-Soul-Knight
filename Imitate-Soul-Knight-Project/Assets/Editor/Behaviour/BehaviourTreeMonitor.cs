using UFramework.AI.BehaviourTree;
using UnityEditor;
using UnityEngine;

public class BehaviourTreeMonitor : EditorWindow {

    [MenuItem ("UFramework/BehaviourTreeMonitor")]
    private static void ShowWindow () {
        var window = GetWindow<BehaviourTreeMonitor> ();
        window.titleContent = new GUIContent ("BehaviourTreeMonitor");
        window.Show ();
    }

    private Texture lightTexture;

    private void Awake () {
        this.lightTexture = Resources.Load<Texture> ("BehaviourTree/LightTaskBlue");
    }

    private void OnGUI () {
        this.drawBehaviourTree ();
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

        // TODO: 构建行为树
        EditorGUILayout.ObjectField ("添加贴图", lightTexture, typeof (Texture), true);
        EditorGUILayout.LabelField ("选择item: " + behaviourNode.name);
    }
}