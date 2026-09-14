//====================================================//
// ファイル名   : SoundList.cs
// 作成者       : Hoshino Ryunosuke
// 作成日       : 2026/07/29
//
// 概要 : 音のリソースリスト
//
// 更新履歴 :
// 2026/07/29 新規作成
//====================================================//
using UnityEngine;
using System.Collections.Generic;

//====================================================//
// クラス宣言
//====================================================//

[CreateAssetMenu(fileName = "SoundList", menuName = "Scriptable Objects/SoundList")]
public class SoundList : ScriptableObject
{
    // SEのリスト
    public List<SoundManager.AudioData> m_seList;

    // BGMのリスト
    public List<SoundManager.AudioData> m_bgmList;
}
