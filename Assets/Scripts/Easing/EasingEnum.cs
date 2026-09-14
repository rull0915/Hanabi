//====================================================//
// ファイル名   : EasingEnum.cs
// 作成者       : Hoshino Ryunosuke
// 作成日       : 2026/07/21
//
// 概要 : イージングを列挙型から選べるようにするためのヘッダ
//====================================================//

using System;
using UnityEngine;

/// <summary>
/// イージングの種類を宣言した列挙型
/// </summary>
public enum EasingKind
{
    Linear,
    Quad,
    Cubic,
    Sine,
    Circ,
    Back,
    Bounce,
    Elastic,

    Pow,
    Back2,
}

/// <summary>
/// イージングのタイプを宣言した列挙型
/// </summary>
public enum EasingType
{
    In,
    Out,
    InOut,
}

// Inspectorで表示可能にする属性
[Serializable]
// イージングをまとめたクラス
public class EasingConfig
{
    public EasingKind kind = EasingKind.Linear;
    public EasingType type = EasingType.Out;

    /// <summary>
    /// 割合に応じたイージング後の値を返す関数
    /// </summary>
    public float Get(float ratio)
    {
        // 0~1にクランプ
        float t = Mathf.Clamp01(ratio);

        // LinearはTypeに関わらず一定
        if (kind == EasingKind.Linear) return MyMath.Easing.Linear(t);

        // Funcを使ってイージング関数を呼び出す
        Func<float, float> easeFunc = (kind, type) switch
        {
            (EasingKind.Quad, EasingType.In) => MyMath.Easing.InQuad,
            (EasingKind.Quad, EasingType.Out) => MyMath.Easing.OutQuad,
            (EasingKind.Quad, EasingType.InOut) => MyMath.Easing.InOutQuad,

            (EasingKind.Cubic, EasingType.In) => MyMath.Easing.InCubic,
            (EasingKind.Cubic, EasingType.Out) => MyMath.Easing.OutCubic,
            (EasingKind.Cubic, EasingType.InOut) => MyMath.Easing.InOutCubic,

            (EasingKind.Sine, EasingType.In) => MyMath.Easing.InSine,
            (EasingKind.Sine, EasingType.Out) => MyMath.Easing.OutSine,
            (EasingKind.Sine, EasingType.InOut) => MyMath.Easing.InOutSine,

            (EasingKind.Circ, EasingType.In) => MyMath.Easing.InCirc,
            (EasingKind.Circ, EasingType.Out) => MyMath.Easing.OutCirc,
            (EasingKind.Circ, EasingType.InOut) => MyMath.Easing.InOutCirc,

            (EasingKind.Back, EasingType.In) => MyMath.Easing.InBack,
            (EasingKind.Back, EasingType.Out) => MyMath.Easing.OutBack,
            (EasingKind.Back, EasingType.InOut) => MyMath.Easing.InOutBack,

            (EasingKind.Bounce, EasingType.In) => MyMath.Easing.InBounce,
            (EasingKind.Bounce, EasingType.Out) => MyMath.Easing.OutBounce,
            (EasingKind.Bounce, EasingType.InOut) => MyMath.Easing.InOutBounce,

            (EasingKind.Elastic, EasingType.In) => MyMath.Easing.InElastic,
            (EasingKind.Elastic, EasingType.Out) => MyMath.Easing.OutElastic,
            (EasingKind.Elastic, EasingType.InOut) => MyMath.Easing.InOutElastic,

            _ => MyMath.Easing.Linear
        };

        return easeFunc(t);
    }
}