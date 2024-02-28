using UnityEngine;

//Bend a modified quad to the surface to prevent floating overlays
public class SurfaceMapping : MonoBehaviour
{
    //4 corners of the mesh, in a Z shape
    public Transform[] corners;
    public MeshFilter filter;
    public float range;
    public bool activateOnSpawn;
    public LayerMask spatialLayer;

    //Mesh data
    private Mesh mesh;
    private int[] fixedTris = new int[] { 0, 1, 2, 0, 2, 4, 0, 4, 3, 0, 3, 1 };
    private Vector3[] vertices = new Vector3[5];
    private Vector2[] UVs = new Vector2[] { new(0.5f, 0.5f), Vector2.up, Vector2.one, Vector2.zero, Vector2.right };

    void Start()
    {
        //Establish basis for the mesh
        mesh = new Mesh();
        vertices[0] = Vector3.zero;

        //Perform if active on spawn in
        if (activateOnSpawn) Calculate();
    }

    //Move corners to surface
    private void Calculate()
    {
        for (int i = 0; i < corners.Length; i++) {
            //Ray pointing towards center and backwards
            Vector3 inward = transform.forward - (corners[i].localPosition.x * transform.right) - (corners[i].localPosition.y * transform.up);

            //If surface is not found resort to original position
            if (Physics.Raycast(corners[i].position, inward, out RaycastHit surface, range, spatialLayer)) vertices[i + 1] = transform.InverseTransformPoint(surface.point);
            else vertices[i + 1] = new Vector3(corners[i].localPosition.x, corners[i].localPosition.y, 0);
        }

        UpdateMesh();
    }

    //Returns mesh to quad state for easier visualization while moving
    public void Baseline()
    {
        for (int i = 0; i < corners.Length; i++) vertices[i + 1] = new Vector3(corners[i].localPosition.x, corners[i].localPosition.y, 0);
        UpdateMesh();
    }

    //Recalculate the center along with the corners
    public void Recalculate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range, spatialLayer)) transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(-hit.normal));
        Calculate();
    }

    //Update mesh with new data
    private void UpdateMesh()
    {
        mesh.Clear();
        filter.mesh = mesh;
        mesh.vertices = vertices;
        mesh.uv = UVs;
        mesh.triangles = fixedTris;
        mesh.RecalculateNormals();

        //If a mesh collider property exists assign the mesh to it
        if (TryGetComponent<MeshCollider>(out MeshCollider collider)) collider.sharedMesh = mesh;
    }
}