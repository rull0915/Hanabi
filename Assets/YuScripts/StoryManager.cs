using UnityEngine;

public class StoryManager : MonoBehaviour
{
    //ストーリーの最大数
    private int MAX_STORY = 36;

    //現在のストーリーページ
    public int m_storyPage = 0;

    //ストーリー本体
    public GameObject m_Story;

    //一ページ移動するのにかかる時間
    public float MOVE_TIME = 0.5f;

    //移動中か
    private bool m_isMoving = false;

    //移動開始位置
    private Vector3 m_startPosition; 

    //移動先
    private Vector3 m_targetPosition; 

    //移動開始からの時間
    private float m_moveTimer = 0.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //現在のページの初期化
        m_storyPage = 0;
    }

    //ページの更新
    private void Update()
    {
        //移動中なら
        if (m_isMoving)
        {
            //タイマーの加算
            m_moveTimer += Time.deltaTime;

            //変換
            float t = m_moveTimer / MOVE_TIME;
            
            //移動
            m_Story.transform.localPosition = Vector3.Lerp(m_startPosition, m_targetPosition, t);

            //移動完了
            if (t >= 1.0f)
            {
                m_Story.transform.localPosition = m_targetPosition;
                m_isMoving = false;
            }

        }
    }

    //進む
    public void PushMove()
    {
        //移動中ならreturn
        if (m_isMoving) return;

        //最後のページか
        if (m_storyPage >= MAX_STORY - 1) return;

        //ページを進める
        m_storyPage++;

        //移動開始
        StartMove();


    }

    //戻る
    public void PushBack()
    {
        //移動中ならreturn
        if (m_isMoving) return;

        //最初のページなら
        if (m_storyPage <= 0) return;

        //ページを戻す
        m_storyPage--;

        //移動開始
        StartMove();
    }

    //スキップ
    public void PushSkip()
    {
        //移動中なら
        if (m_isMoving) return;

        //最後のページへ
        m_storyPage = MAX_STORY - 1;

        //移動開始
        StartMove();
    }

    //移動
    private void StartMove()
    {
        //現在位置
        m_startPosition = m_Story.transform.localPosition;

        //目標位置
        m_targetPosition = new Vector3(-1920.0f * m_storyPage, m_startPosition.y, m_startPosition.z);

        //タイマーの初期化
        m_moveTimer = 0.0f;

        //移動開始
        m_isMoving = true;

    }

}
