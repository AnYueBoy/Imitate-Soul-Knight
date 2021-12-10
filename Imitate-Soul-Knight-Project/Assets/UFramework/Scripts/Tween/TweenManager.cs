/*
 * @Author: l hy 
 * @Date: 2021-12-08 18:04:44 
 * @Description: Tween管理
 */

namespace UFramework.Tween {
    using System.Collections.Generic;
    using UFramework.Tween.Core;

    public static class TweenManager {

        private static HashSet<Tweener<T1, T2>> tweeners = new HashSet<Tweener<T1, T2>> ();

        public static void localUpdate (float dt) {
            foreach (Tweener<T1, T2> tweener in tweeners) {
                tweener.localUpdate (dt);
            }
        }

        public static T3 spawnTweener<T1, T2, T3> (TweenGetter<T1> getter, TweenSetter<T1> setter, T2 endValue, float duration)
        where T3 : Tweener<T1, T2>,
            new () {
                T3 tweener = new T3 ();
                tweener.setProperty (getter, setter, endValue, duration);

                tweeners.Add (tweener);
                return tweener;
            }
    }
}