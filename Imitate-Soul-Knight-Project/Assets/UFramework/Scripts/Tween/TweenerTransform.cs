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

            Vector3 endPos = tweenerCore.changeValue * time / tweenerCore.duration + tweenerCore.beginValue;
            tweenerCore.setter (endPos);
        }

    }
}