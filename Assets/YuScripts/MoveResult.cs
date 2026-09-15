using UnityEngine;
using System.Collections;

public class MoveResult : MonoBehaviour
{
    //出現させるポップ
    [SerializeField] private GameObject popupTarget;

    //呼び出し
    public void OpenPopup()
    {
        // 指定したオブジェクトを表示
        popupTarget.SetActive(true);

        StartCoroutine(ScaleUp());
    }

    private IEnumerator ScaleUp()
    {
        float time = 0f;

        //出現時間
        float duration = 0.3f;

        while (time < duration)
        {
            time += Time.deltaTime;

            //0〜1の進捗率を計算
            float progress = time / duration;

            //イージング
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);

            //横幅を大きく
            float scaleX = Mathf.Lerp(0f, 1f, easedProgress);
            popupTarget.transform.localScale = new Vector3(scaleX, 1f, 1f);

            yield return null;
        }

        //サイズを固定
        popupTarget.transform.localScale = Vector3.one;
    }
}
