using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public Texture2D CursorTexture;

    // Start is called before the first frame update
    void Start()
    {
        if (CursorTexture != null)
        {
            var hotSpot = new Vector2(CursorTexture.width / 2, CursorTexture.height / 2);
            Cursor.SetCursor(CursorTexture, hotSpot, CursorMode.Auto);
        }
    }
}
