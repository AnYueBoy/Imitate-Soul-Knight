/*
 * @Author: l hy 
 * @Date: 2021-12-08 18:15:12 
 * @Description: TweenExtension
 */
namespace UFramework.Tween {
    using System.Collections.Generic;
    using UFramework.Tween.Core;
    using UnityEngine;
    public static class TweenExtension {
        public static void pathTween (this Transform target, List<Vector3> pathList, float duration) {
            TweenerTransform<Vector3> tweener = TweenManager.spawnTweener<Vector3, TweenerTransform<Vector3>> ();
            TweenerCore<Vector3> tweenerCore = new TweenerCore<Vector3> (
                () => {
                    return target.transform.position;
                },
                (Vector3 value) => {
                    target.transform.position = value;
                },
                duration
            );
        }

        public static TweenerTransform<Vector3> moveTween (this Transform target, Vector3 endPos, float duration) {
            TweenerTransform<Vector3> tweener = TweenManager.spawnTweener<Vector3, TweenerTransform<Vector3>> ();

            TweenerCore<Vector3> tweenerCore = new TweenerCore<Vector3> (
                () => {
                    return target.transform.position;
                },
                (Vector3 value) => {
                    target.transform.position = value;
                },
                duration
            );

            tweenerCore.changeValue = endPos - tweenerCore.beginValue;

            tweener.setTweenCore (tweenerCore);
            tweener.setAction (tweener.moveXTween);
            return tweener;
        }
    }

}