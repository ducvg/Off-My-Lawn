using Sirenix.OdinInspector.Editor;
using UnityEngine;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using System.Runtime.CompilerServices;

public class LevelEditorWindow : OdinMenuEditorWindow
{
    const string MONSTERS_PATH = "Assets/_Project/DataObject/Entity Config/Monsters";
    const string HEROES_PATH = "Assets/_Project/DataObject/Entity Config/Heroes";
    [SerializeField, LevelDataDrawer] LevelData currentLevelData;

    public void SetLevelData(LevelData levelData){
        currentLevelData = levelData;
        GUIHelper.RequestRepaint();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.Selection.SupportsMultiSelect = false;
        tree.Config.DrawSearchToolbar = true;

        tree.Add("Edit Spawns Window", this);
        tree.AddAllAssetsAtPath("Monsters", MONSTERS_PATH, typeof(EntityConfigSO)).ForEach(SetupMonsterItem);

        tree.EnumerateTree().AddIcons<EntityConfigSO>(x => x.Icon);

        return tree;
    }

    private void SetupMonsterItem(OdinMenuItem item)
    {
        item.IsSelectable = false;
        item.OnDrawItem += x => DragAndDropUtilities.DragZone(item.Rect, item.Value, true, false);
    }
}
