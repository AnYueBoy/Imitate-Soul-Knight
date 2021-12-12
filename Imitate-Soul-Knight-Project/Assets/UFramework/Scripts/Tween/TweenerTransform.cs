/*
 * @Author: l hy 
 * @Date: 2021-12-10 08:51:06 
 * @Description: 
 */

namespace UFramework.Tween {
    using System.Collections.Generic;
    using UFramework.Tween.Core;
    using UnityEngine;
    public class TweenerTransform<T> : Tweener<T> {

        public void pathTween (float dt, TweenerCore<Vector3> tweenerCore) {
            this.timer += dt;
            if (this.timer > tweenerCore.duration) {
                this.tweenerCompleted ();
                return;
            }

            float time = Mathf.Min (tweenerCore.duration, this.timer);
            float ratioValue = EaseManager.getEaseFuncValue (tweenerCore.easeTye, time, tweenerCore.duration);
            float curMoveDistance = tweenerCore.changeValue.x * ratioValue;

            List<Vector3> pathList = this.getExtraData<List<Vector3>> ();
            float cumulativeDis = 0;
            for (int i = 0; i < pathList.Count - 1; i++) {
                Vector3 prePos = pathList[i];
                Vector3 curPos = pathList[i + 1];
                cumulativeDis += (curPos - prePos).magnitude;

                if (curMoveDistance <= cumulativeDis) {
                    Vector3 dir = curPos - prePos;
                    dir = dir.normalized;
                    Vector3 endPos = prePos + curMoveDistance * dir;
                    tweenerCore.setter (endPos);
                    break;
                }
            }
        }

        public void moveTween (float dt, TweenerCore<Vector3> tweenerCore) {
            this.timer += dt;
            if (this.timer > tweenerCore.duration) {
                this.tweenerCompleted ();
                return;
            }

            float time = Mathf.Min (tweenerCore.duration, this.timer);
            float ratioValue = EaseManager.getEaseFuncValue (tweenerCore.easeTye, time, tweenerCore.duration);

            Vector3 endPos = tweenerCore.changeValue * ratioValue + tweenerCore.beginValue;
            tweenerCore.setter (endPos);
        }

    }
}