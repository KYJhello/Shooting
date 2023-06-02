using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] ParticleSystem muzzleEffect;
    [SerializeField] TrailRenderer bulletTrail;
    [SerializeField] float bulletSpeed;
    [SerializeField] float maxDistance;
    [SerializeField] int damage;


    // �� ���� �ٲٷ��� virtual�� ����
    public void Fire()
    {
        RaycastHit hit;
        // ī�޶� ��ġ���� ī�޶� ���� �������� ���
        // �ѱ����� �����°Ŷ�� �����ؾ���
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
        {
            IHittable hittable = hit.transform.GetComponent<IHittable>();
            //ParticleSystem effect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            // get �Լ��� ��ġ�� �����̼� �ֱ�(�����ε�����)
            ParticleSystem effect = GameManager.Pool.Get(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            
            //effect.transform.position = hit.point;
            //effect.transform.rotation = Quaternion.LookRotation(hit.normal);
            effect.transform.parent = hit.transform;
            //Destroy(effect.gameObject, 3f);
            StartCoroutine(ReleaseRoutine(effect.gameObject));

            StartCoroutine(TrailRoutine(muzzleEffect.transform.position, hit.point));

            hittable?.Hit(hit, damage);
        }
        else
        {
            StartCoroutine(TrailRoutine(muzzleEffect.transform.position, Camera.main.transform.forward * maxDistance));
        }

        IEnumerator ReleaseRoutine(GameObject effect)
        {
            yield return new WaitForSeconds(3f);
            GameManager.Resource.Destroy(effect);
        }


        IEnumerator TrailRoutine(Vector3 startPoint, Vector3 endPoint)
        {
            //TrailRenderer trail = Instantiate(bulletTrail, muzzleEffect.transform.position, Quaternion.identity);
            TrailRenderer trail = GameManager.Resource.Instantiate(bulletTrail, muzzleEffect.transform.position, Quaternion.identity);
            //trail.transform.position = startPoint;
            //trail.transform.rotation = Quaternion.identity;

            //trail.GetComponent<TrailRenderer>().Clear();
            // �ʱ�ȭ �ʼ�!!
            trail.Clear();

            float totalTime = Vector2.Distance(startPoint, endPoint) / bulletSpeed;
            
            
            float rate = 0f;

            while(rate< 1)
            {
                trail.transform.position = Vector3.Lerp(startPoint, endPoint, rate);
                rate += Time.deltaTime / totalTime;

                yield return null;
            }
            GameManager.Resource.Destroy(trail.gameObject);

            // pooling ���� == null ��� ����
            //  if(trail == null || trail.gameObject.activeSelf == false) 
            if (!trail.IsValid())
            {

            }
            else
            {

            }
            
        }
    }
}
