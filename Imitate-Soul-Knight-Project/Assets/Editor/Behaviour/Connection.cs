/*
 * @Author: l hy 
 * @Date: 2022-01-04 16:02:40 
 * @Description: 链接
 */
using System;
using UnityEditor;
using UnityEngine;
public class Connection {

    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;

    public Connection (ConnectionPoint inPoint, ConnectionPoint outPoint) {
        this.inPoint = inPoint;
        this.outPoint = outPoint;
    }

    public void draw () {
        Handles.DrawBezier (
            inPoint.rect.center,
            outPoint.rect.center,
            inPoint.rect.center + Vector2.down * 50f,
            outPoint.rect.center - Vector2.down * 50f,
            Color.white,
            null,
            2f);
    }
}