/*
 * @Author: l hy 
 * @Date: 2021-02-23 21:39:35 
 * @Description: 模块管理
 * @Last Modified by: l hy
 * @Last Modified time: 2021-11-22 09:43:19
 */

namespace UFramework {
    using UFramework.Const;
    using UFramework.Develop;
    using UFramework.GameCommon;
    using UFramework.Promise;
    using UFramework.Tween;
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
        private RectTransform uiRootRectTrans;

        #region 业务模块
        public InputManager inputManager;

        public MapManager mapManager;

        public PlayerDataManager playerDataManager = new PlayerDataManager ();

        public ConfigManager configManager = new ConfigManager ();

        public PlayerManager playerManager;

        public BulletManager bulletManager = new BulletManager ();

        public EnemyManager enemyManager = new EnemyManager ();

        public ItemManager itemManager = new ItemManager ();

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
            this.uIManager.init (this.uiRootRectTrans);
            this.playerDataManager.init ();
            this.configManager.init ();
            this.playerManager.init ();
            this.mapManager.init ();

            this.uIManager.showBoard (UIPath.BATTLE_BOARD);
        }

        private void Update () {
            float dt = Time.deltaTime;
            TweenManager.localUpdate(dt);
            this.guiConsole?.localUpdate (dt);
            this.promiseTimer?.localUpdate (dt);
            this.inputManager.localUpdate (dt);
            this.playerManager?.localUpdate (dt);
            this.mapManager.localUpdate (dt);
            this.bulletManager?.localUpdate (dt);
            this.enemyManager?.localUpdate (dt);
            this.itemManager?.localUpdate (dt);
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

        public Transform effectsTrans;

    }
}