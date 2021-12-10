/*
 * @Author: l hy 
 * @Date: 2021-12-08 18:15:12 
 * @Description: TweenExtension
 */
namespace UFramework.Tween {
    using System.Collections.Generic;
    using UnityEngine;
    public static class TweenExtension {
        public static void pathTween (this Transform target, List<Vector3> pathList, float duration) {
            TweenerTransform<Vector3, List<Vector3>> tweener = TweenManager.spawnTweener<Vector3, List<Vector3>, TweenerTransform<Vector3, List<Vector3>>> (
                () => {
                    return target.transform.position;
                },
                (value) => {
                    target.transform.position = value;
                },
                pathList,
                duration
            );

            tweener.setAction (tweener.pathTween);
        }

        public static void moveXTween (this Transform target, Vector3 endPos, float duration) {
            TweenerTransform<Vector3, Vector3> tweener = TweenManager.spawnTweener<Vector3, Vector3, TweenerTransform<Vector3, Vector3>> (
                () => {
                    return target.transform.position;
                },
                (value) => {
                    target.transform.position = value;
                },
                endPos,
                duration
            );

            tweener.endValue = endPos;
            tweener.changeValue = endPos - tweener.beginValue;
            tweener.setAction (tweener.moveXTween);
        }
    }

}