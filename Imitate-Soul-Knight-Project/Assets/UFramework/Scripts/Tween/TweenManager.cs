/*
 * @Author: l hy 
 * @Date: 2021-12-08 18:04:44 
 * @Description: Tween管理
 */

namespace UFramework.Tween {
    using System.Collections.Generic;
    using UFramework.Tween.Core;

    public class TweenManager<T1, T2> {

        private HashSet<Tweener<T1, T2>> tweeners = new HashSet<Tweener<T1, T2>> ();

        public void localUpdate (float dt) {
            foreach (Tweener<T1, T2> tweener in this.tweeners) {
                tweener.localUpdate (dt);
            }
        }

        public void spawnTweener (TweenGetter<T1> getter, TweenSetter<T1> setter, T2 endValue, float duration) {
            Tweener<T1, T2> tweener = new Tweener<T1, T2> (getter, setter, endValue, duration);

            this.tweeners.Add (tweener);
        }

    }
}