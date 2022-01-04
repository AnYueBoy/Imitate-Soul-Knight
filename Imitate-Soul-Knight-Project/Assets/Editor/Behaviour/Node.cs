/*
 * @Author: l hy 
 * @Date: 2022-01-04 10:03:17 
 * @Description: 节点
 */

using UnityEngine;

public class Node {

    public Rect rect;

    public string title = "默认";

    public GUIStyle style;

    public Node (Vector2 postion, float width, float height, GUIStyle style) {
        rect = new Rect (postion.x, postion.y, width, height);
        this.style = style;
    }

    public void drag (Vector2 delta) {
        rect.position += delta;
    }
    public void draw () {
        GUI.Box (rect, title, style);
    }

    public bool processEvents (Event e) {
        return false;
    }
}