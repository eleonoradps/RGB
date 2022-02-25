using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public class ColorEvent : UnityEvent<Color> { }


public class RGB : MonoBehaviour
{

    private AudioSource audioSource;
    public TextMeshProUGUI DebugText;
    public ColorEvent OnColorPreview;
    public ColorEvent OnColorSelect;
    RectTransform Rect;

    Texture2D ColorTexture;

    // Start is called before the first frame update
    void Start()
    {
        Rect = GetComponent<RectTransform>();

        ColorTexture = GetComponent<Image>().mainTexture as Texture2D;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(Rect, Input.mousePosition))
        {
            Vector2 delta;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Rect, Input.mousePosition, null, out delta);

            string debug = " mouse_position = " + Input.mousePosition;
            debug += "<br>delta = " + delta;

            float width = Rect.rect.width;
            float height = Rect.rect.height;
            delta += new Vector2(width * .5f, height * .5f);
            debug += "<br>offset delta = " + delta;

            float x = Mathf.Clamp(delta.x / width, 0f, 1f);
            float y = Mathf.Clamp(delta.y / height, 0f, 1f);
            debug += "<br>x = " + x + " y = " + y;

            int textureX = Mathf.RoundToInt(x * ColorTexture.width);
            int textureY = Mathf.RoundToInt(y * ColorTexture.height);
            debug += "<br>Texture X = " + textureX + " Texture Y = " + textureY;

            Color color = ColorTexture.GetPixel(textureX, textureY);
            Debug.Log(color);
            debug += "<br> color = " + color;

            DebugText.color = color;
            DebugText.text = debug;

            OnColorPreview?.Invoke(color);

            if (Input.GetMouseButtonDown(0))
            {
                OnColorSelect?.Invoke(color);
                audioSource.Play();
            }
        }
    }
}
