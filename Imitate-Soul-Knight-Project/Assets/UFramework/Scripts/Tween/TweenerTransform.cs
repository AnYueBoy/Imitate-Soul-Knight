/*
 * @Author: l hy 
 * @Date: 2021-12-10 08:51:06 
 * @Description: 
 */

namespace UFramework.Tween {
    using UFramework.Tween.Core;
    using UnityEngine;
    public class TweenerTransform<T> : Tweener<T> {

        public void pathTween (float dt) {

        }

        public void moveXTween (float dt, TweenerCore<Vector3> tweenerCore) {
            this.timer += dt;
            if (this.timer > tweenerCore.duration) {
                this.executeHandler = null;
                this.timer = 0;
                return;
            }

            float time = Mathf.Min (tweenerCore.duration, this.timer);
            float ratioValue = EaseManager.getEaseFuncValue (tweenerCore.easeTye, time, tweenerCore.duration);

            Vector3 endPos = tweenerCore.changeValue * ratioValue + tweenerCore.beginValue;
            tweenerCore.setter (endPos);
        }

    }
}