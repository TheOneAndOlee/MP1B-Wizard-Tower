using UnityEngine;

public static class RishiPuzzleEffects
{
    private static Mesh shardMesh;
    public static void Burst(Vector3 position, Material material, bool stone = false)
    {
        if (shardMesh == null)
        {
            var source = GameObject.CreatePrimitive(PrimitiveType.Cube);
            shardMesh = source.GetComponent<MeshFilter>().sharedMesh;
            source.SetActive(false);
            Object.Destroy(source);
        }
        var obj = new GameObject(stone ? "Stone fragments" : "Rune sparks");
        obj.transform.position = position;
        var particles = obj.AddComponent<ParticleSystem>();
        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        var main = particles.main;
        main.loop = false; main.playOnAwake = false; main.duration = .15f;
        main.startLifetime = new ParticleSystem.MinMaxCurve(.45f, stone ? 1.7f : 1.1f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(1.2f, stone ? 4.5f : 2.4f);
        main.startSize = new ParticleSystem.MinMaxCurve(.04f, stone ? .18f : .10f);
        main.gravityModifier = stone ? 1f : .12f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 90;
        var emission = particles.emission; emission.rateOverTime = 0;
        var shape = particles.shape; shape.shapeType = ParticleSystemShapeType.Sphere; shape.radius = .12f;
        var collision = particles.collision;
        collision.enabled = stone; collision.type = ParticleSystemCollisionType.World;
        collision.bounce = .3f; collision.lifetimeLoss = .25f;
        var renderer = particles.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Mesh;
        renderer.mesh = shardMesh;
        renderer.sharedMaterial = material;
        particles.Play(); particles.Emit(stone ? 55 : 30);
        Object.Destroy(obj, 3f);
    }
}
