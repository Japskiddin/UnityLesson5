using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    [SerializeField] private GameObject targerObject; // ссылка на целевой объект
    [SerializeField] private string targetMessage;
    public Color highlightColor = Color.cyan;

    public void OnMouseEnter() {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null) {
            sprite.color = highlightColor; // меняем цвет при наведении указателя мыши
        }
    }

    public void OnMouseExit() {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null) {
            sprite.color = Color.white;
        }
    }

    public void OnMouseDown() {
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f); // в момент щелчка размер кнопки увеличивается
    }

    public void OnMouseUp() {
        transform.localScale = Vector3.one;
        if (targerObject != null) {
            targerObject.SendMessage(targetMessage); // после щелчка по кнопке отправляем сообщение целевому объекту
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
