/*
 * @Author: l hy 
 * @Date: 2021-12-08 18:04:44 
 * @Description: Tween管理
 */

namespace UFramework.Tween {
    using System.Collections.Generic;

    public static class TweenManager {

        private static HashSet<ITweener> tweeners = new HashSet<ITweener> ();

        public static void localUpdate (float dt) {
            foreach (ITweener tweener in tweeners) {
                tweener.localUpdate (dt);
            }
        }

        public static T2 spawnTweener<T1, T2> () where T2 : Tweener<T1>, new () {
            T2 tweener = new T2 ();

            tweeners.Add (tweener);
            return tweener;
        }
    }
}