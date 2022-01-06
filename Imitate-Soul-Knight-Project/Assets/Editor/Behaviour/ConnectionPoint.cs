/*
 * @Author: l hy 
 * @Date: 2022-01-04 14:03:53 
 * @Description: 链接点
 */
using UnityEngine;
public class ConnectionPoint {
    public Rect rect;
    public ConnectionPointType type;
    public Node node;

    public ConnectionPoint (Node node, ConnectionPointType type) {
        this.node = node;
        this.type = type;
        this.rect = new Rect (0, 0, 20f, 10f);
    }

    public void draw () {
        // GUI坐标左上角为（0,0），右下角为(screenWidth,screenHeight)
        rect.x = node.rect.x + node.rect.width / 2 - rect.width / 2;

        switch (type) {
            case ConnectionPointType.In:
                rect.y = node.rect.y;
                break;

            case ConnectionPointType.Out:
                rect.y = node.rect.y + node.rect.height - rect.height - 2f;
                break;
            default:
                break;
        }
    }
}