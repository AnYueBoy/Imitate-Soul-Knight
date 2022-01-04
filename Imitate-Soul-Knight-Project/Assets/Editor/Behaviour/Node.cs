using System;
/*
 * @Author: l hy 
 * @Date: 2022-01-04 10:03:17 
 * @Description: 节点
 */

using UnityEngine;

public class Node {

    public Rect rect;
    public string title;
    public bool isDragged;
    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;
    public GUIStyle style;

    public Node (
        Vector2 postion,
        float width,
        float height,
        GUIStyle nodeStyle,
        GUIStyle inPointStyle,
        GUIStyle outPointStyle,
        Action<ConnectionPoint> onClickInPoint,
        Action<ConnectionPoint> onClickOutPoint) {
        rect = new Rect (postion.x, postion.y, width, height);
        this.style = nodeStyle;
        this.inPoint = new ConnectionPoint (this, ConnectionPointType.In, inPointStyle, onClickOutPoint);
        this.outPoint = new ConnectionPoint (this, ConnectionPointType.Out, outPointStyle, onClickOutPoint);
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
                    } else {
                        GUI.changed = true;
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
            default:
                break;
        }
        return false;
    }
}