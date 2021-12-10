/*
 * @Author: l hy 
 * @Date: 2021-12-10 08:51:06 
 * @Description: 
 */

using UFramework.Tween.Core;

namespace UFramework.Tween {
    public class TweenerTransform<T1, T2> : Tweener<T1, T2> {
        public TweenerTransform (TweenGetter<T1> getter, TweenSetter<T1> setter, T2 endValue, float duration) : base (getter, setter, endValue, duration) { }

        public override void action(float dt)
        {
            throw new System.NotImplementedException();
        }
    }
}