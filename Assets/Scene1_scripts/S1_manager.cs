using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class S1_manager : MonoBehaviour
{
    public int nbParticles;
    public ComputeShader computeShader;

    public Shader shader;
    private Material material;

    public MeshTopology meshTopology;
    public float noiseAmplitude;
    public float noiseSpeed;
    struct Particle
    {
        public Vector3 position;
        public Vector4 color;
        public float speed;
    }

    ComputeBuffer particles;

    private void Start()
    {
        material = new Material(shader);
        particles = new ComputeBuffer(nbParticles, Marshal.SizeOf<Particle>());
        var particleData = new Particle[nbParticles];

        for (int i = 0; i < nbParticles; i++)
        {
            particleData[i] = new Particle
            {
                position = Random.onUnitSphere * 90.0f,
                color = GetRandomColor(),
                speed = Random.Range(0.1f, 0.2f),
            };
        }
        particles.SetData(particleData);
    }
    private Vector4 GetRandomColor()
    {
        float r = Random.Range(0.0f, 255.0f);
        float g = Random.Range(0.0f, 255.0f);
        float b = Random.Range(0.0f, 255.0f);

        return new Vector4(r, g, b);
    }

    private void Update()
    {
        int kernel = computeShader.FindKernel("UpdateParticles");
        computeShader.SetBuffer(kernel, "_particles", particles);

        computeShader.SetFloat("_time", Time.time);
        computeShader.SetFloat("_noiseAmplitude", noiseAmplitude);
        computeShader.SetFloat("_noiseSpeed", noiseSpeed);

        var count = (nbParticles + 1024 - 1) / 1024;
        computeShader.Dispatch(kernel, count, 1, 1);

        material.SetBuffer("_particles", particles);
        Graphics.DrawProcedural(material, new Bounds(transform.position, Vector3.one * 200.0f),
                                meshTopology, nbParticles);
    }

    private void OnDestroy()
    {
        particles?.Release();
    }
}
