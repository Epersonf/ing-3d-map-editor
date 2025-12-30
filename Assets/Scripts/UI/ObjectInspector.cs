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
    List<KeyValuePair<string, string>> tagItems; // Lista mantida em memória

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
        tagItems = new List<KeyValuePair<string, string>>();
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

        // Atualizar a lista de tags
        UpdateTagList();
    }

    void ConfigureTagList()
    {
        // Configurar altura fixa para o ListView (opcional, mas ajuda na visualização)
        tagList.style.height = 150;

        tagList.makeItem = () =>
        {
            var container = new VisualElement();
            container.style.flexDirection = FlexDirection.Row;
            container.style.justifyContent = Justify.SpaceBetween;
            container.style.paddingTop = 2;
            container.style.paddingBottom = 2;
            
            var keyLabel = new Label();
            keyLabel.name = "key";
            keyLabel.style.width = 120;
            keyLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
            
            var valueLabel = new Label();
            valueLabel.name = "value";
            valueLabel.style.flexGrow = 1;
            valueLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
            
            var deleteButton = new Button();
            deleteButton.name = "delete";
            deleteButton.text = "×";
            deleteButton.style.width = 30;
            
            container.Add(keyLabel);
            container.Add(valueLabel);
            container.Add(deleteButton);
            return container;
        };
        
        tagList.bindItem = (element, i) =>
        {
            if (i < tagItems.Count)
            {
                var kv = tagItems[i];
                var keyLabel = element.Q<Label>("key");
                var valueLabel = element.Q<Label>("value");
                var deleteButton = element.Q<Button>("delete");
                
                if (keyLabel != null) keyLabel.text = kv.Key;
                if (valueLabel != null) valueLabel.text = kv.Value;

                if (deleteButton != null)
                {
                    // remove old handlers to avoid multiple subscriptions
                    deleteButton.clicked -= null;
                    deleteButton.clicked += () =>
                    {
                        if (i >= 0 && i < tagItems.Count && current != null)
                        {
                            var key = tagItems[i].Key;
                            current.RemoveTag(key);
                            UpdateTagList();
                        }
                    };
                }
            }
        };

        tagList.selectionType = SelectionType.Single;
        tagList.itemsSource = tagItems;
    }

    void UpdateTagList()
    {
        if (current == null)
        {
            tagItems.Clear();
            tagList.Rebuild();
            return;
        }

        // Atualizar a lista mantida em memória
        tagItems.Clear();
        tagItems.AddRange(current.Tags.Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value)));
        
        // Notificar o ListView que os dados mudaram
        tagList.itemsSource = tagItems;
        tagList.Rebuild();
    }

    void HookTransformEvents()
    {
        // Remover callbacks antigos primeiro (evitar múltiplas inscrições)
        posX.UnregisterValueChangedCallback(OnPosChanged);
        posY.UnregisterValueChangedCallback(OnPosChanged);
        posZ.UnregisterValueChangedCallback(OnPosChanged);

        rotX.UnregisterValueChangedCallback(OnRotChanged);
        rotY.UnregisterValueChangedCallback(OnRotChanged);
        rotZ.UnregisterValueChangedCallback(OnRotChanged);

        sclX.UnregisterValueChangedCallback(OnScaleChanged);
        sclY.UnregisterValueChangedCallback(OnScaleChanged);
        sclZ.UnregisterValueChangedCallback(OnScaleChanged);

        // Adicionar novos callbacks
        posX.RegisterValueChangedCallback(OnPosChanged);
        posY.RegisterValueChangedCallback(OnPosChanged);
        posZ.RegisterValueChangedCallback(OnPosChanged);

        rotX.RegisterValueChangedCallback(OnRotChanged);
        rotY.RegisterValueChangedCallback(OnRotChanged);
        rotZ.RegisterValueChangedCallback(OnRotChanged);

        sclX.RegisterValueChangedCallback(OnScaleChanged);
        sclY.RegisterValueChangedCallback(OnScaleChanged);
        sclZ.RegisterValueChangedCallback(OnScaleChanged);
    }

    void OnPosChanged(ChangeEvent<float> evt) => ApplyPos();
    void OnRotChanged(ChangeEvent<float> evt) => ApplyRot();
    void OnScaleChanged(ChangeEvent<float> evt) => ApplyScale();

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
        if (string.IsNullOrWhiteSpace(newKey.value)) return;

        current.Tags[newKey.value.Trim()] = newValue.value;
        
        // Limpar campos e atualizar lista
        newKey.value = "";
        newValue.value = "";
        
        UpdateTagList();
    }

    void SetEnabled(bool enabled)
    {
        doc.rootVisualElement.SetEnabled(enabled);
    }
}
