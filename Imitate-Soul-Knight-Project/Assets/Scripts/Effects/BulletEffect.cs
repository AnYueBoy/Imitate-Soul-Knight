using System.Collections;
using System.Collections.Generic;
using UFramework.GameCommon;
using UnityEngine;

public class BulletEffect : MonoBehaviour {
    private void OnParticleSystemStopped () {
        ObjectPool.instance.returnInstance (this.gameObject);
    }
}