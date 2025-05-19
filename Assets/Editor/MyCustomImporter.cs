using UnityEngine;
using UnityEditor;
using SuperTiled2Unity;
using SuperTiled2Unity.Editor;
using UnityEngine.Tilemaps;

[AutoCustomTmxImporter()]
public class MyCustomImporter : CustomTmxImporter {
    public override void TmxAssetImported(TmxAssetImportedArgs args) {
        var root = args.ImportedSuperMap;
        var grid = root.GetComponentInChildren<Grid>();
        if (grid != null && grid.GetComponent<LevelBoundsGenerator>() == null) {
            grid.gameObject.AddComponent<LevelBoundsGenerator>();
        }
        var generator = grid.gameObject.GetComponent<LevelBoundsGenerator>();

        GameObject colliderGameObject = new GameObject("LevelBounds");
        colliderGameObject.transform.SetParent(root.transform);
        colliderGameObject.transform.localPosition = Vector3.zero;

        colliderGameObject.AddComponent<BoxCollider2D>();
        var collider = colliderGameObject.GetComponent<BoxCollider2D>();
        collider.isTrigger = true;

        generator.tilemap = grid.transform.Find("Ground").GetComponent<Tilemap>();
        generator.boundsCollider = collider;
    }
}
