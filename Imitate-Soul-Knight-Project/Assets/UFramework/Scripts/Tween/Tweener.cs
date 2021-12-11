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

        public void setEase (EaseType easeType) {
            this.tweenerCore.easeTye = easeType;
        }
    }

}

namespace UFramework.Tween.Core {

    public delegate T TweenGetter<out T> ();

    public delegate void TweenSetter<in T> (T newValue);

    public class TweenerCore<T> {
        public TweenGetter<T> getter;

        public TweenSetter<T> setter;

        public EaseType easeTye;

        public T endValue;

        public T beginValue;

        public T changeValue;

        public float duration;

        public TweenerCore (TweenGetter<T> getter, TweenSetter<T> setter, float duration) {
            this.getter = getter;
            this.setter = setter;
            this.duration = duration;
            this.beginValue = getter ();
            easeTye = EaseType.LINER;
        }
    }
}