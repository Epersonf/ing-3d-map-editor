using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ObjectInspector : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] TMP_InputField posX;
    [SerializeField] TMP_InputField posY;
    [SerializeField] TMP_InputField posZ;

    [SerializeField] TMP_InputField rotX;
    [SerializeField] TMP_InputField rotY;
    [SerializeField] TMP_InputField rotZ;

    [SerializeField] TMP_InputField sclX;
    [SerializeField] TMP_InputField sclY;
    [SerializeField] TMP_InputField sclZ;

    [Header("Tags UI")]
    [SerializeField] RectTransform tagContainer;
    [SerializeField] TagRow tagRowPrefab;
    [SerializeField] TMP_InputField newKey;
    [SerializeField] TMP_InputField newValue;
    [SerializeField] Button addTagBtn;

    TransformableObject current;
    readonly List<TagRow> rows = new();

    void Awake()
    {
        addTagBtn.onClick.AddListener(AddTag);
    }

    void Update()
    {
        if (GizmoSettings.Current != current)
        {
            current = GizmoSettings.Current;
            Refresh();
        }

        if (!current) return;

        var t = current.TF;

        posX.SetTextWithoutNotify(t.position.x.ToString());
        posY.SetTextWithoutNotify(t.position.y.ToString());
        posZ.SetTextWithoutNotify(t.position.z.ToString());

        rotX.SetTextWithoutNotify(t.eulerAngles.x.ToString());
        rotY.SetTextWithoutNotify(t.eulerAngles.y.ToString());
        rotZ.SetTextWithoutNotify(t.eulerAngles.z.ToString());

        sclX.SetTextWithoutNotify(t.localScale.x.ToString());
        sclY.SetTextWithoutNotify(t.localScale.y.ToString());
        sclZ.SetTextWithoutNotify(t.localScale.z.ToString());
    }

    void Refresh()
    {
        ClearTagRows();
        if (!current) return;
    
        foreach (var kv in current.Tags.ToList())
            CreateTagRow(kv.Key, kv.Value);
    }

    void CreateTagRow(string key, string value)
    {
        var go = Instantiate(tagRowPrefab, tagContainer);
        rows.Add(go);

        var row = go.GetComponent<TagRow>();
        if (row == null)
        {
            Debug.LogError("TagRow prefab is missing TagRow component.");
            return;
        }

        row.Init(current, key, value);
    }

    void ClearTagRows()
    {
        foreach (var r in rows) Destroy(r);
        rows.Clear();
    }

    void AddTag()
    {
        if (!current) return;
        if (string.IsNullOrWhiteSpace(newKey.text)) return;

        current.Tags[newKey.text.Trim()] = newValue.text;
        newKey.text = "";
        newValue.text = "";
        Refresh();
    }
}
