using UnityEngine;
using UnityEngine.InputSystem;

public class CommonHoverCursorManager : MonoBehaviour
{
    //左右に表示する画像
    public RectTransform leftCursor;
    public RectTransform rightCursor;

    //ボタンから画像の間隔
    private const int PADDING = 80;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //非表示にする
        SetCursorsActive(false);
    }

    //マウスがボタンに乗った時
    //When the mouse hovers over the button
    public void OnButtonEnter(GameObject buttonObj)
    {
        if (buttonObj == null) return;

        //ボタンからサイズを測る
        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        if (buttonRect == null) return;

        Debug.Log("check");

        //画像の親（Parent）を、今マウスが乗ったボタンに変更する
        //Change the parent of the image to the button the mouse is currently hovering over.
        if (leftCursor != null) leftCursor.SetParent(buttonRect, false);
        if (rightCursor != null) rightCursor.SetParent(buttonRect, false);

        //ボタンの横幅の半分を計算
        //Calculate half the width of the button.
        float halfWidth = buttonRect.rect.width * 0.5f;

        //ボタンの中心（0, 0）から左右に「ボタンの半幅 + 隙間」だけずらした位置に瞬間移動
        //Instantly teleport to a position shifted left or right from the button's center (0, 0)
        //by the distance of "half the button's width + the gap."
        if (leftCursor != null)
        {
            leftCursor.anchoredPosition = new Vector2(-(halfWidth + PADDING), 0f);
        }
        if (rightCursor != null)
        {
            rightCursor.anchoredPosition = new Vector2(halfWidth + PADDING, 0f);
        }

        //画像を表示する
        //Display the image.
        SetCursorsActive(true);
    }

    //マウスがボタンから離れたとき
    //When the mouse moves away from the button
    public void OnButtonExit()
    {
        //非表示にする Hide
        SetCursorsActive(false);
    }

    //表示・非表示の切り替え
    //Toggle visibility
    private void SetCursorsActive(bool isActive)
    {
        //表示の切り替え
        if (leftCursor != null) leftCursor.gameObject.SetActive(isActive);
        if (rightCursor != null) rightCursor.gameObject.SetActive(isActive);
    }
}
