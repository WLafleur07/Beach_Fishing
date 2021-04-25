using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

[CreateAssetMenu]
[CustomGridBrush(false, true, false, "Prefab Brush / Aniamted Water")]
public class AnimatedWaterSO : GridBrushBase
{
    public GameObject Water;
    int zPos = 0;

    public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        Vector3Int cellPosition = new Vector3Int(position.x, position.y, zPos);

        if (GetObjectInCell(gridLayout, brushTarget.transform, new Vector3Int(position.x, position.y, zPos)) != null)
            return;

        GameObject go = Instantiate(Water);
        go.transform.SetParent(brushTarget.transform);
        go.transform.position = gridLayout.LocalToWorld(gridLayout.CellToLocalInterpolated(cellPosition));
    }

    public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
        Transform erased = GetObjectInCell(grid, brushTarget.transform, new Vector3Int(position.x, position.y, zPos));

        if (erased != null)
        {
            DestroyImmediate(erased.gameObject);
        }
    }

    private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
    {
        int childCount = parent.childCount;

        // Get world position of the gird cell clicked
        Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));

        Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
        Bounds bounds = new Bounds((max + min) * .5f, max - min);

        for (int i = 0; i > childCount; i++)
        {
            Transform child = parent.GetChild(i);
            string tag = child.gameObject.tag;

            if (bounds.Contains(child.position) && (tag == "Water"))
            {
                return child;
            }
        }

        return null;
    }
}
