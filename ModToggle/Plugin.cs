using BepInEx;
using BoplFixedMath;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BepInEx.Bootstrap;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace ModToggle
{
    [BepInPlugin("com.Obelous.ModToggle", "ModToggle", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private bool isMenuVisible = false;
        private Rect windowRect = new Rect(60, 20, 180, 500);

        private void Awake()
        {
            Logger.LogInfo("Plugin ModToggle is loaded!");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnGUI()
        {
            if (isMenuVisible)
            {
                windowRect = GUI.Window(0, windowRect, DrawModList, "Mod List");
            }
        }

        private Dictionary<string, bool> modEnabledStates = new Dictionary<string, bool>();

        private void DrawModList(int windowID)
        {
            int plugin_num = 0;
            List<string> toggledMods = new List<string>();
            foreach (var plugin in Chainloader.PluginInfos)
            {
                string pluginName;
                if (plugin.Key.Split('.').Length == 3)
                {
                    pluginName = "";
                    if (plugin.Key.Split('.')[2] == "ModToggle")
                    {
                        pluginName = "(This mod) ";
                    }
                    pluginName += $"{plugin.Value.Metadata.Name} by \"{plugin.Key.Split('.')[1]}\"";
                }
                else if (plugin.Key.Split('.').Length == 2)
                {
                    pluginName = $"{plugin.Value.Metadata.Name} by \"{plugin.Key.Split('.')[0]}\"";
                }
                else
                {
                    pluginName = "Invalid BepInEx plugin info format.";
                }

                if (!modEnabledStates.ContainsKey(plugin.Value.Metadata.GUID))
                {
                    modEnabledStates[plugin.Value.Metadata.GUID] = plugin.Value.Instance != null;
                }

                bool isEnabled = GUI.Toggle(new Rect(10, 20 + 30 * plugin_num, 160, 20), modEnabledStates[plugin.Value.Metadata.GUID], pluginName);
                if (isEnabled != modEnabledStates[plugin.Value.Metadata.GUID])
                {
                    modEnabledStates[plugin.Value.Metadata.GUID] = isEnabled;
                    // Toggle the mod enabled/disabled state
                }

                plugin_num++;
            }

            if (GUI.Button(new Rect(10, 20 + 30 * plugin_num, 160, 20), "Apply"))
            {
                ApplyChanges(toggledMods);
            }

            GUI.DragWindow();
        }
        private void ApplyChanges(List<string> toggledMods)
        {
            string args = string.Join(" ", toggledMods.Select(mod => $"\"{mod}\""));
            Process.Start(new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-File \"{Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "toggler.ps1")}\" {args}",
                UseShellExecute = false,
            });
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "MainMenu")
            {
                isMenuVisible = true;
            }
            else
            {
                isMenuVisible = false;
            }
        }
    }
}
