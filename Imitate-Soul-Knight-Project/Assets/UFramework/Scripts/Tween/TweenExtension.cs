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
            // TweenerTransform<Vector3, List<Vector3>> tweener = TweenManager.spawnTweener<Vector3, List<Vector3>, TweenerTransform<Vector3, List<Vector3>>> (
            //     () => {
            //         return target.transform.position;
            //     },
            //     (value) => {
            //         target.transform.position = value;
            //     },
            //     pathList,
            //     duration
            // );

            // tweener.setAction (tweener.pathTween);
        }

        public static void moveXTween (this Transform target, Vector3 endPos, float duration) {
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

            tweenerCore.endValue = endPos;
            tweenerCore.changeValue = endPos - tweenerCore.beginValue;

            tweener.setTweenCore (tweenerCore);
            tweener.setAction (tweener.moveXTween);
        }
    }

}