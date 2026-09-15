using UnityEngine;
using System.Collections;

public class MoveResult : MonoBehaviour
{
    // どのオブジェクト（キャンバス）を出すかを指定する穴
    [SerializeField] private GameObject popupTarget;

    // 呼び出し
    public void OpenPopup()
    {
        // 指定したオブジェクトを表示
        popupTarget.SetActive(true);

        StartCoroutine(ScaleUp());
    }

    private IEnumerator ScaleUp()
    {
        float time = 0f;
        float duration = 0.3f; // 広がる時間（秒）※0.5〜0.7秒あたりがキレイに見えます

        while (time < duration)
        {
            time += Time.deltaTime;

            // 0〜1の進捗率を計算
            float progress = time / duration;

            // 【イージングの追加】SmoothStepを使うことで、動き始めと終わりが滑らか（減速）になります
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);

            // 0から1へ、横幅だけを大きくする
            float scaleX = Mathf.Lerp(0f, 1f, easedProgress);

            // 【変更点】X（横幅）だけ変化させ、Y（縦）は「1f」で固定して元の長さをキープします
            popupTarget.transform.localScale = new Vector3(scaleX, 1f, 1f);

            yield return null;
        }

        // 最後にサイズを完全に（1, 1, 1）に固定
        popupTarget.transform.localScale = Vector3.one;
    }
}
