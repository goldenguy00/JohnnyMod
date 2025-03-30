using JohnnyMod.Survivors.Johnny;
using RoR2;
using System.Runtime.CompilerServices;

namespace JohnnyMod
{
    public static class EmoteAPICompat
    {
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void EmoteHook()
        {
            foreach (var item in SurvivorCatalog.allSurvivorDefs)
            {
                if (item.bodyPrefab.name == "JohnnyBody")
                {
                    var skele = JohnnyAssets.emoteAPISkeleton;
                    EmotesAPI.CustomEmotesAPI.ImportArmature(item.bodyPrefab, skele, jank: false);
                    skele.GetComponentInChildren<BoneMapper>().scale = 1f;
                }
            }

            EmotesAPI.CustomEmotesAPI.animChanged += CustomEmotesAPI_animChanged;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void CustomEmotesAPI_animChanged(string newAnimation, BoneMapper mapper)
        {
            if (mapper.transform.name == "JohnnyEmoteSkeleton")
            {
                var childLoc = mapper.transform.parent.GetComponent<ChildLocator>();
                var enableWeapons = newAnimation is "none";

                childLoc.FindChildGameObject("KatanaBlade")?.SetActive(enableWeapons);
                childLoc.FindChildGameObject("KatanaHilt")?.SetActive(enableWeapons);
                childLoc.FindChildGameObject("KatanaSheath")?.SetActive(enableWeapons);
            }
        }
    }

}
