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
    public GUIStyle style;
    public BaseNode btNode;
    public Node (
        BaseNode btNode,
        Vector2 postion,
        float width,
        float height,
        string title,
        GUIStyle nodeStyle) {
        this.btNode = btNode;
        rect = new Rect (postion.x, postion.y, width, height);
        this.title = title;
        this.style = nodeStyle;
        this.inPoint = new ConnectionPoint (this, ConnectionPointType.In);
        this.outPoint = new ConnectionPoint (this, ConnectionPointType.Out);
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

    private void checkNodeStatus () {
        if (btNode == null) {
            return;
        }

        RunningStatus runningStatus = btNode.curNodeRunningStatus;
        switch (runningStatus) {
            case RunningStatus.Executing:
                GUI.Box (rect, title, style);
                break;
            case RunningStatus.Success:
                GUI.Box (rect, title, style);
                break;
            case RunningStatus.Failed:
                GUI.Box (rect, title, style);
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