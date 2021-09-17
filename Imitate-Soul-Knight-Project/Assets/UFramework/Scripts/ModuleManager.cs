/*
 * @Author: l hy 
 * @Date: 2021-02-23 21:39:35 
 * @Description: 模块管理
 * @Last Modified by: l hy
 * @Last Modified time: 2021-09-17 16:29:54
 */

namespace UFramework {
    using UFramework.Const;
    using UFramework.Develop;
    using UFramework.Promise;
    using UnityEngine;
    public class ModuleManager : MonoBehaviour {
        public AppMode appMode = AppMode.Developing;

        private static ModuleManager _instance;

        public static ModuleManager instance {
            get {
                return _instance;
            }
        }

        #region mono模块
        public InputManager inputManager = new InputManager ();

        #endregion

        #region 非mono模块
        private GUIConsole guiConsole = null;
        public PromiseTimer promiseTimer = new PromiseTimer ();
        #endregion

        #region  程序生命周期函数 
        private void Awake () {
            _instance = this;
            if (this.appMode != AppMode.Release) {
                this.guiConsole = new GUIConsole ();
                this.guiConsole.init ();
            }
        }

        private void Start () {

        }

        private void Update () {
            float dt = Time.deltaTime;
            this.guiConsole?.localUpdate (dt);
            this.promiseTimer?.localUpdate (dt);
            this.inputManager.localUpdate (dt);
        }

        private void LateUpdate () {

        }

        private void OnGUI () {
            this.guiConsole?.drawGUI ();
        }

        private void OnDisable () {
            this.guiConsole?.quit ();
        }
        #endregion 

    }
}