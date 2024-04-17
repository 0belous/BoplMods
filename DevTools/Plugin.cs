using BepInEx;
using BepInEx.Logging;
using BoplFixedMath;
using HarmonyLib;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DevTools
{
    [HarmonyPatch]
    public class Patches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameSession), nameof(GameSession.RandomBagLevel))]
        public static void Fixes(GameSession __instance, ref byte ___currentLevel, ref int __result)
        {
                    int mapId = Plugin.MapId;
                    ___currentLevel = (byte)mapId;
                    __result = ___currentLevel;
                    Debug.Log("setting mapid to " + mapId);
        }

    }

    [BepInPlugin("com.Obelous.DevTools", "DevTools", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static int MapId = 0;
        private void Awake()
        {
            Harmony harmony = new("com.Obelous.DevTools");
            harmony.PatchAll();
        }

        private void Update()
        {

            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                SceneManager.LoadScene("CharacterSelect");
            }

            if (SceneManager.GetActiveScene().name.Contains("Level"))
            {
                if (Keyboard.current[Key.F1].wasPressedThisFrame)
                {
                    MapId = 0;
                }
                if (Keyboard.current[Key.F2].wasPressedThisFrame)
                {
                    MapId = 21;
                }
                if (Keyboard.current[Key.F3].wasPressedThisFrame)
                {
                    MapId = 37;
                }
                if (Keyboard.current[Key.F4].wasPressedThisFrame)
                {
                    MapId = 50;
                }
            }
        }
     }
}
