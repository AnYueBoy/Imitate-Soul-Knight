/*
 * @Author: l hy 
 * @Date: 2021-11-19 14:16:38 
 * @Description: 宝箱基类
 */
using UnityEngine;
public class BaseChest : MonoBehaviour {
    [SerializeField]
    private Transform left;

    [SerializeField]
    private Transform right;

    public virtual void localUpdate(float dt){

    }
}