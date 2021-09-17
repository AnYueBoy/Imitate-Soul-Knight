/*
 * @Author: l hy 
 * @Date: 2021-09-17 14:00:13 
 * @Description: 角色控制
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleControl : MonoBehaviour {

    private readonly float moveSpeed = 2;

    private Animator animator;

    private void OnEnable () {
        this.animator = transform.GetComponent<Animator> ();
    }

    private void Update () {
        this.editorControl ();
    }

    private void editorControl () {
#if UNITY_EDITOR
        if (Input.GetKey (KeyCode.D)) {
            transform.Translate (Vector3.right * Time.deltaTime * this.moveSpeed);
            transform.localScale = new Vector3 (1, 1, 1);
        } else if (Input.GetKey (KeyCode.A)) {
            transform.Translate (Vector3.left * Time.deltaTime * this.moveSpeed);
            transform.localScale = new Vector3 (-1, 1, 1);
        } else if (Input.GetKey (KeyCode.W)) {
            transform.Translate (Vector3.up * Time.deltaTime * this.moveSpeed);
        } else if (Input.GetKey (KeyCode.S)) {
            transform.Translate (Vector3.down * Time.deltaTime * this.moveSpeed);
        }
#endif
    }
}