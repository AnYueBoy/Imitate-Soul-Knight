using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util {
    /// <summary>
    /// 获取世界坐标下的旋转
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="angles"></param>
    /// <returns></returns>
    public static Vector3 GetWorldEulerAngles (Transform transform, Vector3 angles) {
        Vector3 resultAngles = angles;
        while (transform.parent != null) {
            transform = transform.parent;
            resultAngles += transform.eulerAngles;
        }

        return resultAngles;
    }

    /// <summary>
    /// 获取区间内的随机数值[min,max]
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int GetRandomValue (int min, int max) {
        min = min > max?max : min;
        max = max < min?min : max;
        int result = Random.Range (min, max + 1);
        return result;
    }

    public static float GetRandomValue (float min, float max) {
        min = min > max?max : min;
        max = max < min?min : max;
        float result = Random.Range (min, max + 1);
        return result;
    }

    public static Color GetRandomColor () {
        float r, g, b;
        do {
            r = GetRandomValue (0, 1.0f);
            g = GetRandomValue (0, 1.0f);
            b = GetRandomValue (0, 1.0f);
        } while (r == 0 && g == 0 && b == 0);
        return new Color (r, g, b, 1);
    }

    /// <summary> 
    /// 绘制线段 
    /// </summary> 
    /// <param name="startPos">开始位置</param> 
    /// <param name="direcition">方向</param> 
    /// <param name="distance">距离</param> 
    /// <param name="color">颜色</param> 
    public static void DrawLine (Vector3 startPos, Vector3 direcition, float distance, Color color) {
        Vector3 endPos = startPos + direcition * distance;
        Debug.DrawLine (startPos, endPos, color);
    }

    public static void DrawLine (Vector3 startPos, Vector3 endPos, Color color) {
        if (startPos == null || endPos == null) {
            Debug.Log ("start or end is null");
            return;
        }
        Debug.DrawLine (startPos, endPos, color);
    }

    /// <summary>
    ///绘制路径 
    /// </summary>
    /// <param name="posList"></param>
    public static void DrawPath (List<Vector3> posList, Color color) {
        if (posList == null || posList.Count <= 1) {
            return;
        }
        for (int i = 0; i < posList.Count - 1; i++) {
            Vector3 curPos = posList[i];
            Vector3 nextPos = posList[i + 1];

            DrawLine (curPos, nextPos, color);
        }
    }
}