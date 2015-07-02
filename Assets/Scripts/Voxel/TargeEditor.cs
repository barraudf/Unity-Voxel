using UnityEngine;

public class TargeEditor : MonoBehaviour
{
    public float TargetMaxDistance = 100f;
    public Color BoxColor = Color.black;

    protected Vector3[] vertices;
    protected RaycastHit Hit;
    protected bool TargetHit = false;
    protected Vector3i BlockPosition;
    protected static Material SelectionBoxMaterial = null;

    protected void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out Hit, TargetMaxDistance))
        {
            Chunk chunk = Hit.collider.GetComponent<Chunk>();
            if (chunk != null)
            {
                TargetHit = true;
                BlockPosition = World.GetBlockPosition(Hit);

                if (Input.GetMouseButtonDown(0))
                    World.SetBlock(Hit, new BlockAir());
                else if (Input.GetMouseButtonDown(1))
                    World.SetBlock(Hit, new BlockGrass(), true);
            }
        }
        else
        {
            TargetHit = false;
        }
    }

    protected void Awake()
    {
        vertices = BlockSelection.BuildBox();
        CreateMaterial();
    }

    protected void OnPostRender()
    {
        if (TargetHit)
        {
            SelectionBoxMaterial.SetPass(0);

            GL.PushMatrix();
            GL.Begin(GL.LINES);
            GL.Color(BoxColor);

            for (int i = 1; i < vertices.Length; i++)
            {
                GL.Vertex(BlockPosition + vertices[i - 1]);
                GL.Vertex(BlockPosition + vertices[i]);
            }

            GL.End();
            GL.PopMatrix();
        }
    }

    protected static void CreateMaterial()
    {
        // from : http://docs.unity3d.com/ScriptReference/GL.html
        if (SelectionBoxMaterial == null)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            SelectionBoxMaterial = new Material(shader);
            SelectionBoxMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            SelectionBoxMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            SelectionBoxMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            SelectionBoxMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            SelectionBoxMaterial.SetInt("_ZWrite", 0);
        }
    }
}
