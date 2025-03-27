using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Image imageUI;
    [SerializeField] private TextMeshProUGUI nameUI;
    [SerializeField] private TextMeshProUGUI qualityUI;

    [Header("Default Values")]
    [SerializeField] private Sprite defaultImageUI;
    [SerializeField] private string defaultNameUI;
    [SerializeField] private string defaultQualityUI;

    private Animator animator;

    public bool empty { get; private set; }

    private void Awake()
    {
        empty = true;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool("Empty", empty); // анимируем слот
    }

    public void SetItem(Dish item)
    {
        empty = false;

        imageUI.sprite = item.GetImage();
        nameUI.text = item.GetName();
        qualityUI.text = item.GetStringDishQuality();
    }

    public void ClearSlot()
    {
        empty = true;

        imageUI.sprite = defaultImageUI;
        nameUI.text = defaultNameUI;
        qualityUI.text = defaultQualityUI;
    }

}
