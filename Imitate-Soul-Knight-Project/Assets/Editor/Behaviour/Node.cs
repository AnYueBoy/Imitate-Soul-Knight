/*
 * @Author: l hy 
 * @Date: 2022-01-04 10:03:17 
 * @Description: 节点
 */

using UnityEngine;
public class Node {
    public Rect rect;
    public string title = "默认";
    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;
    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;
    public Node (
        Vector2 postion,
        float width,
        float height,
        string title,
        GUIStyle nodeStyle,
        GUIStyle selectedStyle) {
        rect = new Rect (postion.x, postion.y, width, height);
        this.title = title;
        this.style = nodeStyle;
        defaultNodeStyle = nodeStyle;
        this.selectedNodeStyle = selectedStyle;
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
        GUI.Box (rect, title, style);
    }

    public bool processEvents (Event e) {
        switch (e.type) {
            case EventType.MouseDown:
                if (e.button == 0) {
                    if (rect.Contains (e.mousePosition)) {
                        GUI.changed = true;
                        style = selectedNodeStyle;
                    } else {
                        GUI.changed = true;
                        style = defaultNodeStyle;
                    }
                    return true;
                }

                break;

            case EventType.MouseUp:
                break;

            case EventType.MouseDrag:
                break;
        }
        return false;
    }
}