/*
 * @Author: l hy 
 * @Date: 2021-12-08 18:24:38 
 * @Description: Tweener
 */
namespace UFramework.Tween {
    using System;
    using UFramework.Tween.Core;
    public class Tweener<T1, T2> : TweenerCore<T1, T2>, ITweener {

        protected float timer;

        protected float duration;

        protected Action<float> executeHandler;

        protected T1 beginValue;

        protected T2 changeValue;

        public void setProperty (TweenGetter<T1> getter, TweenSetter<T1> setter, T2 endValue, float duration) {
            this.getter = getter;
            this.setter = setter;
            this.endValue = endValue;
            this.duration = duration;
            this.beginValue = this.getter();
        }

        public void setAction (Action<float> actionHandler) {
            this.executeHandler = actionHandler;
        }

        public void localUpdate (float dt) {
            executeHandler?.Invoke (dt);
        }
    }

}

namespace UFramework.Tween.Core {

    public delegate T TweenGetter<out T> ();

    public delegate void TweenSetter<in T> (T newValue);

    public class TweenerCore<T1, T2> {
        public TweenGetter<T1> getter;

        public TweenSetter<T1> setter;

        public T2 endValue;
    }
}