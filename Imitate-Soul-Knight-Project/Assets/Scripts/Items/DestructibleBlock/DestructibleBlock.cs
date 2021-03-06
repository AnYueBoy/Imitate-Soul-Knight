/*
 * @Author: l hy 
 * @Date: 2021-12-14 12:24:06 
 * @Description: 可破坏障碍
 */

using System;
using UnityEngine;
public abstract class DestructibleBlock : MonoBehaviour {
    public abstract void init (Action<Vector3> callback);

    public abstract void destroyItem ();

}