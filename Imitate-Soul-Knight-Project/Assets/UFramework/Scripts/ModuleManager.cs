/*
 * @Author: l hy 
 * @Date: 2021-02-23 21:39:35 
 * @Description: 模块管理
 * @Last Modified by: l hy
 * @Last Modified time: 2021-11-15 12:30:00
 */

namespace UFramework {
    using UFramework.Const;
    using UFramework.Develop;
    using UFramework.GameCommon;
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

        [SerializeField]
        private GameObject uiRoot;

        #region 业务模块
        public InputManager inputManager;

        public MapManager mapManager;

        public PlayerDataManager playerDataManager = new PlayerDataManager ();

        public ConfigManager configManager = new ConfigManager ();

        public PlayerManager playerManager;

        public BulletManager bulletManager = new BulletManager ();

        public EnemyManager enemyManager = new EnemyManager ();

        #endregion

        #region 系统模块
        private GUIConsole guiConsole = null;
        public PromiseTimer promiseTimer = new PromiseTimer ();

        public UIManager uIManager = new UIManager ();
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
            this.uIManager.init (this.uiRoot);
            this.playerDataManager.init ();
            this.configManager.init ();
            this.playerManager.init ();
            this.mapManager.init ();
        }

        private void Update () {
            float dt = Time.deltaTime;
            this.guiConsole?.localUpdate (dt);
            this.promiseTimer?.localUpdate (dt);
            this.inputManager.localUpdate (dt);
            this.playerManager?.localUpdate (dt);
            this.mapManager.localUpdate (dt);
            this.bulletManager?.localUpdate (dt);
            this.enemyManager?.localUpdate (dt);
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

        public Transform gameObjectTrans;

    }
}