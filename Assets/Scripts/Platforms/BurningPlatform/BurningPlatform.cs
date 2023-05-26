using UnityEngine;
public class BurningPlatform : MonoBehaviour
{
    [Header("Burn Speed")]
    [SerializeField] private float BurnSpeed = 5f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;

        
    }
    private void OnCollisionExit(Collision collision)
    {
        GameObject go = collision.gameObject;


    }

    /*
    private void SpawnDestroyedParticle()
    {
        try
        {
            GameObject DestructionParticle = Instantiate(DestroyParticle, CalculateCenter(gameObject), transform.rotation) as GameObject;
            DestructionParticle.transform.localScale = transform.localScale;

            ParticleSystem Particle = DestructionParticle.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule mainModule = Particle.main;

            ParticleSystemRenderer renderer = DestructionParticle.GetComponent<ParticleSystemRenderer>();
            Material ParticleMaterial = renderer.sharedMaterial;
            MeshFilter meshfilter = GetComponentInParent<MeshFilter>();

            Shader shader = render.material.shader;
            Material particleMaterial = new(shader);

            if (meshfilter == null)
            {
                particleMaterial.SetTexture("_BaseMap", renderer.material.mainTexture);
                renderer.material = particleMaterial;
            }
            else
            {
                Mesh[] mesh = { meshfilter.sharedMesh };
                renderer.SetMeshes(mesh);
            }

            float newSize = (transform.localScale.z == 0 ? (transform.localScale.x + transform.localScale.y) / 4 : (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 6);
            mainModule.startSize = newSize;

            ParticleMaterial.SetColor("_Color", render.material.color);

            Debug.Log(gameObject.name + " burned");
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    */
}
