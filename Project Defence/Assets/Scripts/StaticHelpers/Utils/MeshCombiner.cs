using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StaticHelpers.MapCreationUtils.MeshCombiner
{
    [ExecuteInEditMode]
    public class MeshCombiner : MonoBehaviour
    {
        public MeshFilter[] meshFilters;

        public MeshFilter targetMesh;


        [ContextMenu("CombineMeshes")]
        public void CombineMeshes()
        {
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            for (int i = 0; i < meshFilters.Length; i++)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            }

            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combine);
            targetMesh.mesh = mesh;
            SaveMesh(targetMesh.sharedMesh, gameObject.name, false, true);
        }

        public void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
        {
            string path = EditorUtility.SaveFilePanel("Save Mesh Asset", "Assest/", name, "asset");
            if (string.IsNullOrEmpty(path)) return;
            path = FileUtil.GetProjectRelativePath(path);
            Mesh meshToSave = (makeNewInstance) ? Instantiate(mesh) as Mesh : mesh;

            if (optimizeMesh)
                MeshUtility.Optimize(meshToSave);

            AssetDatabase.CreateAsset(meshToSave, path);
            AssetDatabase.SaveAssets();

        }
    }
}

