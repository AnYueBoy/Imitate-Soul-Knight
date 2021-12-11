/*
 * @Author: l hy 
 * @Date: 2021-12-10 18:19:54 
 * @Description: 缓动函数管理
 */
namespace UFramework.Tween {
    using System.Collections.Generic;
    using System;
    public class EaseManager {

        private static Dictionary<EaseType, Func<float, float, float>> easeDic =
            new Dictionary<EaseType, Func<float, float, float>> () { { EaseType.LINER, liner }, { EaseType.InQuad, inQuad }
            };

        private static float liner (float time, float duration) {
            float ratioTime = time / duration;
            return ratioTime;
        }

        private static float inQuad (float time, float duration) {
            float ratioTime = time / duration;
            return ratioTime * ratioTime;
        }

        public static float getEaseFuncValue (EaseType ease, float time, float duration) {
            Func<float, float, float> easeFunc = easeDic[ease];
            return easeFunc.Invoke (time, duration);
        }
    }

    public enum EaseType {
        LINER,
        InQuad
    }
}