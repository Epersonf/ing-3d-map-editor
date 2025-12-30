using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.FileSystem
{
    public class FileSystemNavigator : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private string rootDirectory = "Assets";
        
        [Header("Settings")]
        [SerializeField] private bool showHiddenFiles = false;
        [SerializeField] private string[] allowedExtensions = { ".fbx", ".obj", ".png", ".jpg", ".mat", ".prefab" };
        
        private TreeView fileTreeView;
        private TextField pathField;
        private Button refreshButton;
        private Button goToAssetsButton;
        
        private FileSystemItem rootItem;
        private Dictionary<int, FileSystemItem> treeIdToItem = new Dictionary<int, FileSystemItem>();
        private int nextId = 0;
        
        void Start()
        {
            if (uiDocument == null)
                uiDocument = GetComponent<UIDocument>();
            
            InitializeUI();
            NavigateTo(rootDirectory);
        }
        
        void InitializeUI()
        {
            var root = uiDocument.rootVisualElement;
            
            // Get UI elements
            fileTreeView = root.Q<TreeView>("fileTreeView");
            pathField = root.Q<TextField>("pathField");
            refreshButton = root.Q<Button>("refreshButton");
            goToAssetsButton = root.Q<Button>("goToAssetsButton");
            
            // Configure TreeView
            fileTreeView.makeItem = MakeTreeItem;
            fileTreeView.bindItem = BindTreeItem;
            fileTreeView.selectionType = SelectionType.Single;
            fileTreeView.SetRootItems(new List<TreeViewItemData<FileSystemItem>>());
            
            // Register events
            fileTreeView.selectedIndicesChanged += OnTreeSelectionChanged;
            refreshButton.clicked += RefreshCurrentDirectory;
            goToAssetsButton.clicked += () => NavigateTo("Assets");
            
            // Enable drag and drop for models
            fileTreeView.RegisterCallback<PointerDownEvent>(evt =>
            {
                if (evt.button == 0 && evt.clickCount == 2) // Double click
                {
                    OnItemDoubleClick();
                }
            });
        }
        
        VisualElement MakeTreeItem()
        {
            var item = new VisualElement();
            item.style.flexDirection = FlexDirection.Row;
            item.style.alignItems = Align.Center;
            item.style.paddingLeft = 5;
            item.style.height = 24;
            
            var icon = new VisualElement();
            icon.name = "icon";
            icon.style.width = 16;
            icon.style.height = 16;
            icon.style.marginRight = 5;
            icon.AddToClassList("file-icon");
            
            var label = new Label();
            label.name = "label";
            label.style.flexGrow = 1;
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            
            item.Add(icon);
            item.Add(label);
            
            return item;
        }
        
        void BindTreeItem(VisualElement element, int index)
        {
            var itemData = fileTreeView.GetItemDataForIndex<FileSystemItem>(index);
            if (itemData == null) return;
            
            var icon = element.Q<VisualElement>("icon");
            var label = element.Q<Label>("label");
            
            // Clear existing icon classes
            icon.ClearClassList();
            icon.AddToClassList(itemData.GetIconClass());
            
            label.text = itemData.GetDisplayName();
            
            // Set different colors based on type
            switch (itemData.Type)
            {
                case FileSystemItemType.Directory:
                    label.style.color = new StyleColor(new Color(0.8f, 0.8f, 1f));
                    break;
                case FileSystemItemType.Model:
                    label.style.color = new StyleColor(new Color(0.7f, 1f, 0.7f));
                    break;
                default:
                    label.style.color = new StyleColor(new Color(0.9f, 0.9f, 0.9f));
                    break;
            }
        }
        
        void NavigateTo(string path)
        {
            if (!Directory.Exists(path))
            {
                Debug.LogError($"Directory does not exist: {path}");
                return;
            }
            
            rootItem = new FileSystemItem(path);
            RefreshTreeView();
            pathField.value = path;
        }
        
        void RefreshTreeView()
        {
            treeIdToItem.Clear();
            nextId = 0;
            
            var rootData = BuildTreeData(rootItem, 0);
            fileTreeView.SetRootItems(new List<TreeViewItemData<FileSystemItem>> { rootData });
            fileTreeView.Rebuild();
        }
        
        TreeViewItemData<FileSystemItem> BuildTreeData(FileSystemItem item, int depth)
        {
            var id = nextId++;
            treeIdToItem[id] = item;
            
            var children = new List<TreeViewItemData<FileSystemItem>>();
            
            if (item.Type == FileSystemItemType.Directory)
            {
                try
                {
                    var childItems = item.GetChildren();
                    foreach (var child in childItems)
                    {
                        if (showHiddenFiles || !child.Name.StartsWith("."))
                        {
                            children.Add(BuildTreeData(child, depth + 1));
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"Cannot access directory {item.Path}: {e.Message}");
                }
            }
            
            return new TreeViewItemData<FileSystemItem>(id, item, children);
        }
        
        void OnTreeSelectionChanged(IEnumerable<int> selectedIds)
        {
            foreach (var id in selectedIds)
            {
                if (treeIdToItem.TryGetValue(id, out var item))
                {
                    // Handle item selection (could show preview, etc.)
                    Debug.Log($"Selected: {item.Path}");
                }
            }
        }
        
        void OnItemDoubleClick()
        {
            var selectedId = fileTreeView.selectedIndex;
            if (selectedId >= 0 && treeIdToItem.TryGetValue(selectedId, out var item))
            {
                if (item.Type == FileSystemItemType.Directory)
                {
                    item.IsExpanded = !item.IsExpanded;
                    RefreshTreeView();
                }
                else if (item.Type == FileSystemItemType.Model)
                {
                    Debug.Log($"Would load model: {item.RelativePath}");
                    // TODO: Implement model loading
                }
            }
        }
        
        void RefreshCurrentDirectory()
        {
            if (rootItem != null)
            {
                RefreshTreeView();
            }
        }
        
        // Public method to get selected model path for dragging
        public string GetSelectedModelPath()
        {
            var selectedId = fileTreeView.selectedIndex;
            if (selectedId >= 0 && treeIdToItem.TryGetValue(selectedId, out var item))
            {
                if (item.Type == FileSystemItemType.Model)
                {
                    return item.RelativePath;
                }
            }
            return null;
        }
    }
}
