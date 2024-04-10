using BepInEx;
using BoplFixedMath;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace ModName
{
    [BepInPlugin("com.Obelous.OPBeam", "OP Beam", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("Plugin OP Beam is loaded!");

            Harmony harmony = new Harmony("com.Obelous.OPBeam");


            MethodInfo original = AccessTools.Method(typeof(Beam), "Awake");
            MethodInfo patch = AccessTools.Method(typeof(myPatches), "patch");
            harmony.Patch(original, new HarmonyMethod(patch));
        }

        public class myPatches
        {
            public static void patch(Beam __instance)
            {
                __instance.angularAimSpeed = (Fix)1f;
                __instance.maxTime = (Fix)5000f;
                __instance.maxTimeAir = (Fix)5000f;
            }
        }
    }
}