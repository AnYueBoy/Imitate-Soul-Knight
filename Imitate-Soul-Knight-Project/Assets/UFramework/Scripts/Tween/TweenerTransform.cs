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
                return;
            }

            Vector3 endPos = this.changeValue * this.timer / tweenerCore.duration + this.beginValue;
            tweenerCore.setter (endPos);
        }

    }
}