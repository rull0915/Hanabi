//====================================================//
// ファイル名   : Easing.cs
// 作成者       : Hoshino Ryunosuke
// 作成日       : 2026/07/27
//
// 概要 : イージング関数をまとめたヘッダ
//====================================================//

using System;

// 独自の数学関連の処理をまとめた名前空間
namespace MyMath
{
    // イージング関数を持つ静的クラス
    public static class Easing
    {
        // 補助変数
        private static float c1 = 1.70158f;
        private static float c2 = c1 * 1.525f;
        private static float c3 = c1 + 1.0f;
        private static float c4 = (2.0f * MathF.PI) / 3.0f;
        private static float c5 = (2.0f * MathF.PI) / 4.5f;

        //========================================
        // 関数宣言
        //========================================

		// ==== Linear ==== //
		public static float Linear(float t) { return t; }

        // ===== Sine ===== //
        public static float InSine(float t)
        {
            return 1.0f - MathF.Cos((t * MathF.PI) / 2.0f);
        }

        public static float OutSine(float t)
        {
            return MathF.Sin((t * MathF.PI) / 2.0f);
        }

        public static float InOutSine(float t)
        {
            return -(MathF.Cos(MathF.PI * t) - 1.0f) / 2.0f;
        }

        // ===== Quad ===== //
        public static float InQuad(float t)
        {
            return t * t;
        }

        public static float OutQuad(float t)
        {
            return 1 - (1 - t) * (1 - t);
        }

        public static float InOutQuad(float t)
        {
            return t < 0.5f
                ? 2 * t * t
                : 1 - MathF.Pow(-2 * t + 2, 2) / 2;
        }

        // ===== Cubic ===== //
        public static float InCubic(float t)
        {
            return t * t * t;
        }

        public static float OutCubic(float t)
        {
            return 1 - MathF.Pow(1 - t, 3);
        }

        public static float InOutCubic(float t)
        {
            return t < 0.5f
                ? 4 * t * t * t
                : 1 - MathF.Pow(-2 * t + 2, 3) / 2;
        }

        // ===== Quart ===== //
        public static float InQuart(float t)
        {
            return t * t * t * t;
        }

        public static float OutQuart(float t)
        {
            return 1 - MathF.Pow(1 - t, 4);
        }

        public static float InOutQuart(float t)
        {
            return t < 0.5f
                ? 8 * t * t * t * t
                : 1 - MathF.Pow(-2 * t + 2, 4) / 2;
        }

        // ===== Quint ===== //
        public static float InQuint(float t)
        {
            return t * t * t * t * t;
        }

        public static float OutQuint(float t)
        {
            return 1 - MathF.Pow(1 - t, 5);
        }

        public static float InOutQuint(float t)
        {
            return t < 0.5f
                ? 16 * t * t * t * t * t
                : 1 - MathF.Pow(-2 * t + 2, 5) / 2;
        }

        // ===== Expo ===== //
        public static float InExpo(float t)
        {
            return t == 0 ? 0 : MathF.Pow(2, 10 * t - 10);
        }

        public static float OutExpo(float t)
        {
            return t == 1 ? 1 : 1 - MathF.Pow(2, -10 * t);
        }

        public static float InOutExpo(float t)
        {
            if (t == 0) return 0;
            if (t == 1) return 1;
            return t < 0.5f
                ? MathF.Pow(2, 20 * t - 10) / 2
                : (2 - MathF.Pow(2, -20 * t + 10)) / 2;
        }

        // ===== Circ ===== //
        public static float InCirc(float t)
        {
            return 1 - MathF.Sqrt(1 - t * t);
        }

        public static float OutCirc(float t)
        {
            return MathF.Sqrt(1 - MathF.Pow(t - 1, 2));
        }

        public static float InOutCirc(float t)
        {
            return t < 0.5f
                ? (1 - MathF.Sqrt(1 - MathF.Pow(2 * t, 2))) / 2
                : (MathF.Sqrt(1 - MathF.Pow(-2 * t + 2, 2)) + 1) / 2;
        }

        // ===== Back ===== //
        public static float InBack(float t)
        {
            return c3 * t * t * t - c1 * t * t;
        }

        public static float OutBack(float t)
        {
            return 1 + c3 * MathF.Pow(t - 1, 3) + c1 * MathF.Pow(t - 1, 2);
        }

        public static float InOutBack(float t)
        {
            return t < 0.5f
                ? (MathF.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2)) / 2
                : (MathF.Pow(2 * t - 2, 2) * ((c2 + 1) * (2 * t - 2) + c2) + 2) / 2;
        }

        // ===== Elastic ===== //
        public static float InElastic(float t)
        {
            if (t == 0) return 0;
            if (t == 1) return 1;
            return -MathF.Pow(2, 10 * t - 10) * MathF.Sin((t * 10 - 10.75f) * c4);
        }

        public static float OutElastic(float t)
        {
            if (t == 0) return 0;
            if (t == 1) return 1;
            return MathF.Pow(2, -10 * t) * MathF.Sin((t * 10 - 0.75f) * c4) + 1;
        }

        public static float InOutElastic(float t)
        {
            if (t == 0) return 0;
            if (t == 1) return 1;
            return t < 0.5f
                ? -(MathF.Pow(2, 20 * t - 10) * MathF.Sin((20 * t - 11.125f) * c5)) / 2
                : (MathF.Pow(2, -20 * t + 10) * MathF.Sin((20 * t - 11.125f) * c5)) / 2 + 1;
        }

        // ===== Bounce ===== //
        public static float OutBounce(float t)
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;

            if (t < 1 / d1)
                return n1 * t * t;
            else if (t < 2 / d1)
            {
                t -= 1.5f / d1;
                return n1 * t * t + 0.75f;
            }
            else if (t < 2.5f / d1)
            {
                t -= 2.25f / d1;
                return n1 * t * t + 0.9375f;
            }
            else
            {
                t -= 2.625f / d1;
                return n1 * t * t + 0.984375f;
            }
        }

        public static float InBounce(float t)
        {
            return 1 - OutBounce(1 - t);
        }

        public static float InOutBounce(float t)
        {
            return t < 0.5f
                ? (1 - OutBounce(1 - 2 * t)) / 2
                : (1 + OutBounce(2 * t - 1)) / 2;
        }

        // ===== Power ===== //

        public static float InPower(float t, float power)
        {
            if (power <= 0) return t;
            return t == 0 ? 0 : MathF.Pow(t, power);
        }
        public static float OutPower(float t)
        {
            return 1 - OutPower(1 - t);
        }

        public static float InOutPower(float t)
        {
            return t < 0.5f
                ? (1 - OutPower(1 - 2 * t)) / 2
                : (1 + OutPower(2 * t - 1)) / 2;
        }

        // ===== Back2 ===== //

    }
}
