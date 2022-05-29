using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Edit : SingletonPattern<Edit>
{
    private Button editButton;

    [SerializeField] private Image editModeImage;
    [SerializeField] private TextMeshProUGUI title;
    private string originalTitle;

    public bool EditMode { get; private set; }

    private void Start()
    {
        editButton = GetComponent<Button>();
        editButton.onClick.AddListener(SetEditMode);
        originalTitle = title.text;
    }

    /// <summary>
    /// Switches the mode between edit and original
    /// </summary>
    private void SetEditMode()
    {
        if (EditMode)
        {
            title.text = originalTitle;
            editModeImage.gameObject.SetActive(false);
            EditMode = false;
        }
        else if (!EditMode)
        {
            title.text = "Edit" + originalTitle;
            editModeImage.gameObject.SetActive(true);
            EditMode = true;
        }
    }


}
