using BepInEx;
using BoplFixedMath;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace ModName
{
    [BepInPlugin("com.Obelous.RemoteExplosives", "Remote Explosives", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("Plugin Remote Explosives is loaded!");

            Harmony harmony = new Harmony("com.Obelous.RemoteExplosives");

            MethodInfo engine = AccessTools.Method(typeof(RocketEngine), "Awake");
            MethodInfo enginePatched = AccessTools.Method(typeof(enginePatches), "patch");
            harmony.Patch(engine, new HarmonyMethod(enginePatched));

        }

        public class enginePatches
        {
            public static void patch(RocketEngine __instance)
            {
                __instance.radius = (Fix)100f;
            }
        }
    }
}