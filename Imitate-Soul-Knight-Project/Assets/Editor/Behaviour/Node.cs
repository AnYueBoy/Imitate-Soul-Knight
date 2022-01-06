/*
 * @Author: l hy 
 * @Date: 2022-01-04 10:03:17 
 * @Description: 节点
 */

using UnityEngine;
public class Node {
    public Rect rect;
    public string title = "默认";
    public bool isDragged;
    public bool isSelected;
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
        GUIStyle selectedStyle,
        GUIStyle inPointStyle,
        GUIStyle outPointStyle) {
        rect = new Rect (postion.x, postion.y, width, height);
        this.title = title;
        this.style = nodeStyle;
        defaultNodeStyle = nodeStyle;
        this.selectedNodeStyle = selectedStyle;
        this.inPoint = new ConnectionPoint (this, ConnectionPointType.In, inPointStyle);
        this.outPoint = new ConnectionPoint (this, ConnectionPointType.Out, outPointStyle);
    }

    public void drag (Vector2 delta) {
        rect.position += delta;
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
                        this.isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    } else {
                        GUI.changed = true;
                        isSelected = false;
                        style = defaultNodeStyle;
                    }
                }
                break;

            case EventType.MouseUp:
                this.isDragged = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && this.isDragged) {
                    this.drag (e.delta);
                    e.Use ();
                    return true;
                }
                break;
        }
        return false;
    }
}