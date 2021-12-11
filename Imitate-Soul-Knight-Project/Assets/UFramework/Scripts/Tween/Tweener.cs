/*
 * @Author: l hy 
 * @Date: 2021-12-08 18:24:38 
 * @Description: Tweener
 */
namespace UFramework.Tween {
    using System;
    using UFramework.Tween.Core;
    public class Tweener<T> : ITweener {

        protected float timer;

        protected Action<float, TweenerCore<T>> executeHandler;

        protected TweenerCore<T> tweenerCore;

        public void setAction (Action<float, TweenerCore<T>> actionHandler) {
            this.executeHandler = actionHandler;
        }

        public void localUpdate (float dt) {
            executeHandler?.Invoke (dt, this.tweenerCore);
        }

        public void setTweenCore (TweenerCore<T> tweenCore) {
            this.tweenerCore = tweenCore;
        }
    }

}

namespace UFramework.Tween.Core {

    public delegate T TweenGetter<out T> ();

    public delegate void TweenSetter<in T> (T newValue);

    public class TweenerCore<T> {
        public TweenGetter<T> getter;

        public TweenSetter<T> setter;

        public T endValue;

        public float duration;

        public TweenerCore (TweenGetter<T> getter, TweenSetter<T> setter, T endValue, float duration) {
            this.getter = getter;
            this.setter = setter;
            this.endValue = endValue;
        }
    }
}