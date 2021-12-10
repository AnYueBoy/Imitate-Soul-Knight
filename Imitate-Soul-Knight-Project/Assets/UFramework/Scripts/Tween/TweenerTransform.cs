/*
 * @Author: l hy 
 * @Date: 2021-12-10 08:51:06 
 * @Description: 
 */

namespace UFramework.Tween {
    using UnityEngine;
    public class TweenerTransform : Tweener<T1, T2> {

        public void pathTween (float dt) {

        }

        public void moveXTween (float dt) {
            this.timer += dt;
            if (this.timer > this.duration) {
                this.executeHandler = null;
                return;
            }

            Vector3 endPos = this.changeValue * this.timer / this.duration + this.beginValue;
            this.setter (endPos);
        }

    }
}