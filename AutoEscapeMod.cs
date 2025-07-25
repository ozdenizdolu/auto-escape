using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts;
using Il2CppAssets.Scripts.Unity.UI_New.Upgrade;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using AutoEscape;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.ModOptions;
using BTD_Mod_Helper.Api.Helpers;
using BTD_Mod_Helper.Extensions;
using MelonLoader;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

[assembly: MelonInfo(typeof(AutoEscapeMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace AutoEscape;

public class AutoEscapeMod : BloonsTD6Mod
{   
    private static bool isEnabled = true;
    private static readonly ModSettingHotkey Hotkey = new(KeyCode.F6)
    {
        displayName = "Toggle this mod with hotkey:"
    };
    public override void OnUpdate()
    {
        if (Hotkey.JustPressed())
        {
            ToggleMod();
        }
    }

    private static void ToggleMod() {
        isEnabled = !isEnabled;
        var message = $"AutoEscape is now {(isEnabled ? "enabled!" : "disabled!")}";

        if (InGame.instance == null || InGame.Bridge == null)
        {
            ModHelper.Msg<AutoEscapeMod>(message);
        }
        else
        {
            Game.instance.ShowMessage(message, 1f);
        }
    }
    public override bool PreBloonLeaked(Bloon bloon)
    {   
        if (isEnabled 
        && bloon.GetModifiedTotalLeakDamage() >= InGame.instance.bridge.GetHealth() + InGame.Bridge.simulation.Shield
        && !InGameData.CurrentGame.IsSandbox && !bloon.bloonModel.HasBehavior<GoldenBloonModel>())
        {
            if (!InGame.instance.quitting)
            {
                InGame.instance.Quit();
                
                ModHelper.Msg<AutoEscapeMod>("You're Welcome.");
            }
            return false;
        }
        return true;
    }
        
}