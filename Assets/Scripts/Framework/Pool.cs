//====================================================//
// ファイル名   : Pool.cs
// 作成者       : Hoshino Ryunosuke
// 作成日       : 2026/07/29
//
// 概要 : 特定の型をプール化して再利用できるようにするクラス
//
// 更新履歴 :
// 2026/07/29 新規作成
//====================================================//
using System;
using System.Collections.Generic;

//====================================================//
// クラス宣言
//====================================================//

public class Pool<T>
{
    // 未使用リスト
    private List<int> m_freeList = new List<int>();

    // プール本体
    private List<T> m_objectPool = new List<T>();

    // 生成方法をまとめた関数
    private readonly Func<T> m_createFunc;

    // コンストラクタで生成関数をもらう
    public Pool(Func<T> createFunc)
    {
        // nullの例外チェック
        m_createFunc = createFunc ?? throw new ArgumentNullException(nameof(createFunc));
    }

    // 未使用のオブジェクトを取得する関数
    public T GetUnUsedObject()
    {
        int index = -1;

        // 未使用リストがあれば
        if (m_freeList.Count > 0)
        {
            // 最後の要素をインデックスに取得
            int lastIndex = m_freeList.Count - 1;
            index = m_freeList[lastIndex];

            // 使用するため未使用リストから除外
            m_freeList.RemoveAt(lastIndex);
        }
        // なければ
        else
        {
            // 末尾をインデックスに
            index = m_objectPool.Count;

            // 新たに要素を増やす
            m_objectPool.Add(m_createFunc.Invoke());
        }

        return m_objectPool[index];
    }

    /// <summary>
    /// オブジェクトをプールに返却する
    /// </summary>
    /// <param name="item">返却するオブジェクト</param>
    public void Return(T item)
    {
        // 返却されたオブジェクトのインデックスを取得
        int index = m_objectPool.IndexOf(item);

        // なければ
        if (index == -1)
        {
            throw new ArgumentException("このオブジェクトはプールに属していません。");
        }

        // 未使用のインデックスが指定されたら
        if (m_freeList.Contains(index))
        {
            // 二重返却の防止
            return;
        }

        // 未使用にする
        m_freeList.Add(index);
    }
}
