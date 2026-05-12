using UnityEngine;
using TMPro;

public class SineWaveTextAnimation : MonoBehaviour
{
    const float amplitude = 5f; // How far the letters float
    const float frequency = 2f; // Speed of the float
    const float waveOffset = 0.2f; // Offset between each letter
    private const int CHARACTER_VERTICES = 4;
    private TMP_Text textMesh;
    private Vector3[] originalVertices;
    private void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        textMesh.ForceMeshUpdate();
        originalVertices = textMesh.mesh.vertices;
    }
    private void Update()
    {
        AnimateText();
    }
    private void AnimateText()
    {
        textMesh.ForceMeshUpdate();
        var mesh = textMesh.mesh;
        var vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            int charIndex = i / CHARACTER_VERTICES; 
            float wave = Mathf.Sin(Time.time * frequency + charIndex * waveOffset);
            vertices[i].y = originalVertices[i].y + wave * amplitude;
        }
        mesh.vertices = vertices;
        textMesh.canvasRenderer.SetMesh(mesh);
    }
}
