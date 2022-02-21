using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UApplication = UFramework.Core.Application;
using UFramework.Bootstarp;
using UFramework.Core;
public class FrameLaunch : MonoBehaviour {

    private UApplication application;
    private void Awake () {
        application = UApplication.New ();

        application.Bootstrap (
            // 注册系统所需服务
            new SystemProviderBootstrap (),
            // 注册自定义服务
            new CustomProviderBootstrap ()
        );

    }

    private void Start () {
        application.Init ();
    }

    private void Update () {
        float deltaTime = Time.deltaTime;
        App.Make<IPlayerDataManager> ().SaveDataByFixedTime (deltaTime);
    }
}