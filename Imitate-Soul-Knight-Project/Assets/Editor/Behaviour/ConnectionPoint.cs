/*
 * @Author: l hy 
 * @Date: 2022-01-04 14:03:53 
 * @Description: 链接点
 */
using System;
using UnityEngine;
public class ConnectionPoint {
    public Rect rect;
    public ConnectionPointType type;
    public Node node;

    public GUIStyle style;

    public ConnectionPoint (Node node, ConnectionPointType type, GUIStyle style) {
        this.node = node;
        this.type = type;
        this.style = style;
        this.rect = new Rect (0, 0, 20f, 10f);
    }

    public void draw () {
        // GUI坐标左上角为（0,0），右下角为(screenWidth,screenHeight)
        rect.x = node.rect.x + node.rect.width / 2 - rect.width / 2;

        switch (type) {
            case ConnectionPointType.In:
                rect.y = node.rect.y - rect.height / 2;
                break;

            case ConnectionPointType.Out:
                rect.y = node.rect.y + node.rect.height - rect.height;
                break;
            default:
                break;
        }
    }
}