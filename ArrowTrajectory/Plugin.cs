using BepInEx;
using BoplFixedMath;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace ModName
{
    [BepInPlugin("com.Obelous.ArrowTrajectories", "ArrowTrajectories", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private Dictionary<GameObject, GameObject> lineObjects = new Dictionary<GameObject, GameObject>();

        private void Start()
        {
            string warning = "Please Don't Cheat!";
            if (warning != null)
            {

            }
        }

        public void drawLine(GameObject obj, float force, float angle) {
            if (obj == null || obj.transform == null) return;
            if (!lineObjects.ContainsKey(obj))
            {
                GameObject lineObject = new GameObject("LineObject");
                LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.widthMultiplier = 0.1f;
                lineRenderer.positionCount = 100;
                for (int i = 0; i < 100; i++)
                {
                    lineRenderer.SetPosition(i, new Vector3(0, 0, 0));
                }
                lineObjects[obj] = lineObject;
            }

            LineRenderer lr = lineObjects[obj].GetComponent<LineRenderer>();
            float g = 9.8f;

            for (int i = 0; i < 100; i++)
            {
                float t = i / 10.0f;
                float x = force * Mathf.Cos(angle * Mathf.Deg2Rad) * t;
                float y = force * Mathf.Sin(angle * Mathf.Deg2Rad) * t - 0.5f * g * t * t;
                lr.SetPosition(i, new Vector3(obj.transform.position.x + x, obj.transform.position.y + y, 0));
            }
        }

        private void Update()
        {

            if (SceneManager.GetActiveScene().name != null && SceneManager.GetActiveScene().name.Contains("Level") && GameLobby.isOnlineGame == false)
            {
                GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
                if (objects == null) return;
                foreach (GameObject obj in objects)
                {
                    if (obj.name.StartsWith("Bow"))
                    {
                        float angle = obj.transform.rotation.eulerAngles.z + 90;
                        drawLine(obj, 37.0f, angle);
                    }
                }

                List<GameObject> deactivatedBows = new List<GameObject>();
                foreach (var pair in lineObjects)
                {
                    if (!pair.Key.activeInHierarchy)
                    {
                        deactivatedBows.Add(pair.Key);
                        StartCoroutine(DeactivateLine(pair.Value));
                    }
                }

                foreach (GameObject bow in deactivatedBows)
                {
                    lineObjects.Remove(bow);
                }
            }
        }

        private IEnumerator DeactivateLine(GameObject lineObject)
        {
            yield return new WaitForSeconds(10);
            Destroy(lineObject);
        }
    }
}
