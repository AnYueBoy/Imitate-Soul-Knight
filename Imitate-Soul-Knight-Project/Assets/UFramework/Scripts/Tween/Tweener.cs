/*
 * @Author: l hy 
 * @Date: 2021-12-08 18:24:38 
 * @Description: Tweener
 */
namespace UFramework.Tween {
    using UFramework.Tween.Core;
    public class Tweener<T1, T2> : TweenerCore<T1> {

        public T2 endValue;

        public float duration;

        public Tweener (TweenGetter<T1> getter, TweenSetter<T1> setter, T2 endValue, float duration) {
            this.getter = getter;
            this.setter = setter;
            this.endValue = endValue;
            this.duration = duration;
        }

        public void localUpdate (float dt) {

        }

        public virtual void action (float dt) {

        }
    }

}

namespace UFramework.Tween.Core {

    public delegate T TweenGetter<out T> ();

    public delegate void TweenSetter<in T> (T newValue);

    public class TweenerCore<T> {
        public TweenGetter<T> getter;

        public TweenSetter<T> setter;

    }

}