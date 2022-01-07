using System.Collections.Generic;
/*
 * @Author: l hy 
 * @Date: 2022-01-04 10:03:17 
 * @Description: 节点
 */

using UFramework.AI.BehaviourTree;
using UnityEngine;
public class Node {
    public Rect rect;
    public string title = "默认";
    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;
    public GUIStyle runningStyle;
    public GUIStyle successStyle;
    public GUIStyle failedStyle;
    public BaseNode btNode;
    private Texture2D[] runningList;
    public Node (
        BaseNode btNode,
        Vector2 postion,
        float width,
        float height,
        GUIStyle runningStyle,
        GUIStyle successStyle,
        GUIStyle failedStyle,
        Texture2D[] runningList) {
        this.btNode = btNode;
        rect = new Rect (postion.x, postion.y, width, height);
        this.runningStyle = runningStyle;
        this.successStyle = successStyle;
        this.failedStyle = failedStyle;
        this.inPoint = new ConnectionPoint (this, ConnectionPointType.In);
        this.outPoint = new ConnectionPoint (this, ConnectionPointType.Out);
        this.runningList = runningList;
        this.title = btNode.GetType ().Name;
    }

    public void drag (Vector2 delta) {
        rect.position += delta;
    }
    public void scale (float value) {
        value = value / 2;
        rect.width += value;
        rect.height += value;
        rect.x += value;
        rect.y += value;
    }
    public void draw () {
        inPoint.draw ();
        outPoint.draw ();
        this.checkNodeStatus ();
    }

    private int curRuningIndex = 0;
    private int runningFrame = 0;

    private void checkNodeStatus () {
        if (btNode == null) {
            return;
        }

        RunningStatus runningStatus = btNode.curNodeRunningStatus;
        switch (runningStatus) {
            case RunningStatus.Executing:
                GUI.Box (rect, title, runningStyle);
                GUIStyle GUIStyle = new GUIStyle ();
                GUIStyle.normal.background = this.runningList[curRuningIndex];
                GUI.Box (new Rect (rect.x + rect.width / 2 - 10, rect.y + 10, 20, 20), "", GUIStyle);
                this.runningFrame++;
                if (runningFrame > 10) {
                    this.curRuningIndex++;
                    this.curRuningIndex %= this.runningList.Length;
                    this.runningFrame = 0;
                }
                GUI.changed = true;
                break;
            case RunningStatus.Success:
                GUI.Box (rect, title, successStyle);
                break;
            case RunningStatus.Failed:
                GUI.Box (rect, title, failedStyle);
                break;
        }

    }

    public bool processEvents (Event e) {
        switch (e.type) {
            case EventType.MouseDown:
                break;

            case EventType.MouseUp:
                break;

            case EventType.MouseDrag:
                break;
        }
        return false;
    }
}