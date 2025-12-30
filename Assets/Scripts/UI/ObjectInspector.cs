using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectInspector : MonoBehaviour
{
    UIDocument doc;

    FloatField posX, posY, posZ;
    FloatField rotX, rotY, rotZ;
    FloatField sclX, sclY, sclZ;

    ListView tagList;
    TextField newKey;
    TextField newValue;
    Button addTag;

    TransformableObject current;

    void Awake()
    {
        doc = GetComponent<UIDocument>();

        // layout handled in UXML (no runtime style adjustments)

        posX = doc.rootVisualElement.Q<FloatField>("posX");
        posY = doc.rootVisualElement.Q<FloatField>("posY");
        posZ = doc.rootVisualElement.Q<FloatField>("posZ");

        rotX = doc.rootVisualElement.Q<FloatField>("rotX");
        rotY = doc.rootVisualElement.Q<FloatField>("rotY");
        rotZ = doc.rootVisualElement.Q<FloatField>("rotZ");

        sclX = doc.rootVisualElement.Q<FloatField>("sclX");
        sclY = doc.rootVisualElement.Q<FloatField>("sclY");
        sclZ = doc.rootVisualElement.Q<FloatField>("sclZ");

        tagList = doc.rootVisualElement.Q<ListView>("tagList");
        newKey = doc.rootVisualElement.Q<TextField>("newKey");
        newValue = doc.rootVisualElement.Q<TextField>("newValue");
        addTag = doc.rootVisualElement.Q<Button>("addTag");

        addTag.clicked += AddTag;

        ConfigureTagList();
    }

    void Update()
    {
        // troca de objeto selecionado
        if (GizmoSettings.Current != current)
        {
            current = GizmoSettings.Current;
            Refresh();
        }

        if (!current)
            return;

        // atualização contínua sem disparar eventos
        var t = current.TF;

        posX.SetValueWithoutNotify(t.position.x);
        posY.SetValueWithoutNotify(t.position.y);
        posZ.SetValueWithoutNotify(t.position.z);

        rotX.SetValueWithoutNotify(t.eulerAngles.x);
        rotY.SetValueWithoutNotify(t.eulerAngles.y);
        rotZ.SetValueWithoutNotify(t.eulerAngles.z);

        sclX.SetValueWithoutNotify(t.localScale.x);
        sclY.SetValueWithoutNotify(t.localScale.y);
        sclZ.SetValueWithoutNotify(t.localScale.z);
    }

    void Refresh()
    {
        if (current == null)
        {
            SetEnabled(false);
            return;
        }

        SetEnabled(true);

        var t = current.TF;

        posX.value = t.position.x;
        posY.value = t.position.y;
        posZ.value = t.position.z;

        rotX.value = t.eulerAngles.x;
        rotY.value = t.eulerAngles.y;
        rotZ.value = t.eulerAngles.z;

        sclX.value = t.localScale.x;
        sclY.value = t.localScale.y;
        sclZ.value = t.localScale.z;

        HookTransformEvents();

        // Provide an array copy of KeyValuePair so ListView can index
        var items = current.Tags.Select(kv => new KeyValuePair<string,string>(kv.Key, kv.Value)).ToList();
        tagList.itemsSource = items;
    }

    void ConfigureTagList()
    {
        tagList.makeItem = () => new Label();
        tagList.bindItem = (element, i) =>
        {
            var kv = (KeyValuePair<string,string>)tagList.itemsSource[i];
            (element as Label).text = $"{kv.Key} = {kv.Value}";
        };

        tagList.selectionType = SelectionType.Single;
    }

    void HookTransformEvents()
    {
        posX.RegisterValueChangedCallback(v => ApplyPos());
        posY.RegisterValueChangedCallback(v => ApplyPos());
        posZ.RegisterValueChangedCallback(v => ApplyPos());

        rotX.RegisterValueChangedCallback(v => ApplyRot());
        rotY.RegisterValueChangedCallback(v => ApplyRot());
        rotZ.RegisterValueChangedCallback(v => ApplyRot());

        sclX.RegisterValueChangedCallback(v => ApplyScale());
        sclY.RegisterValueChangedCallback(v => ApplyScale());
        sclZ.RegisterValueChangedCallback(v => ApplyScale());
    }

    void ApplyPos()
    {
        if (!current) return;

        current.TF.position = new Vector3(
            posX.value, posY.value, posZ.value);
    }

    void ApplyRot()
    {
        if (!current) return;

        current.TF.eulerAngles = new Vector3(
            rotX.value, rotY.value, rotZ.value);
    }

    void ApplyScale()
    {
        if (!current) return;

        current.TF.localScale = new Vector3(
            sclX.value, sclY.value, sclZ.value);
    }

    void AddTag()
    {
        if (!current) return;
        if (string.IsNullOrEmpty(newKey.value)) return;

        current.Tags[newKey.value] = newValue.value;

        Refresh();
    }

    void SetEnabled(bool enabled)
    {
        doc.rootVisualElement.SetEnabled(enabled);
    }
}
