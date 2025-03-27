using EFT;
using System.Reflection;
using SPT.Reflection.Patching;
using RaidOverhaul.Controllers;

namespace RaidOverhaul.Patches
{
    public class KeycardDoorUnlockPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Target the UnlockOperation method in KeycardDoor
            return typeof(KeycardDoor).GetMethod("UnlockOperation", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPrefix]
        private static bool PatchPrefix(
            ref GStruct457<GClass3424> __result,
            KeyComponent key,
            Player player,
            KeycardDoor __instance)
        {
            // Check if the player can interact
            Error canInteract = player.MovementContext.CanInteract;
            if (canInteract != null)
            {
                __result = canInteract;
                return false;
            }

            bool isAuthorized = key.Template.KeyId == __instance.KeyId || key.Template.KeyId == Utils.VipKeycard;
            if (!isAuthorized)
            {
                __result = new GClass3424(key, null, false);
                return false;
            }

            __result = new GClass3424(key, null, true);
            return false;
        }
    }

    public class KeyDoorUnlockPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(Door).GetMethod("UnlockOperation", BindingFlags.Public | BindingFlags.Instance);
        }
        
        [PatchPrefix]
        private static bool PatchPrefix(
            ref GStruct457<GClass3424> __result,
            KeyComponent key,
            Player player,
            Door __instance)
        {
            // Check if the player can interact
            Error canInteract = player.MovementContext.CanInteract;
            if (canInteract != null)
            {
                __result = canInteract;
                return false;
            }
            
            bool isAuthorized = key.Template.KeyId == __instance.KeyId || key.Template.KeyId == Utils.SkeletonKey;
            if (!isAuthorized)
            {
                __result = new GClass3424(key, null, false);
                return false;
            }

            __result = new GClass3424(key, null, true);
            return false;
        }
    }
}
