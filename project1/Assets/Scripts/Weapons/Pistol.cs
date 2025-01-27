
using System.Collections;
using UnityEngine;


    

public class Pistol : Gun
{
    public override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Fire1"))
        {
            TryShoot();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            isShooting = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload();
        }


    }
    public override void Shoot()
    {
        RaycastHit hit;
        Vector3 target = Vector3.zero;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward + currentRecoil, out hit, gunData.shootingRange))
        {
            Debug.Log(gunData.gunName + " hit " + hit.collider.name);
            target = hit.point;
            TryKillEnemy(hit.collider);
        }
        else
        {
            target = cameraTransform.position + (cameraTransform.forward + currentRecoil) * gunData.shootingRange;
        }

        StartCoroutine(BulletFire(target, hit));
    }

    private void TryKillEnemy(Collider collider)
    {
        AgentScript enemyScript = collider.GetComponent<AgentScript>();

        if (enemyScript != null)
        {
            enemyScript.isDead = true;
        }
    }



    private IEnumerator BulletFire(Vector3 target, RaycastHit hit)
    {
        GameObject bulletTrail = Instantiate(gunData.bulletTrailPrefab, gunMuzzle.position, Quaternion.identity);

        while (bulletTrail != null && Vector3.Distance(bulletTrail.transform.position, target) > 0.1f)
        {
            bulletTrail.transform.position = Vector3.MoveTowards(bulletTrail.transform.position, target, Time.deltaTime * gunData.bulletSpeed);
            yield return null;
        }

        Destroy(bulletTrail); // Destroy before instantiating bullet hole
        yield return new WaitForSeconds(0.1f); // Small delay to ensure visibility

        if (hit.collider != null)
        {
            BulletHitFX(hit);
        }
    }


    private void BulletHitFX(RaycastHit hit)
    {
        Vector3 hitPosition = hit.point + hit.normal * 0.02f ;
        GameObject hitParticle = Instantiate(bulletHitParticlePrefab, hit.point, Quaternion.LookRotation(hit.normal));
        GameObject bulletHole = Instantiate(bulletHolePrefab, hitPosition, Quaternion.LookRotation(hit.normal));
        
        bulletHole.transform.parent = hit.collider.transform;
        hitParticle.transform.parent = hit.collider.transform;
        
        Destroy(hitParticle,2f);
        Destroy(bulletHole,2f);
        

    }



}
