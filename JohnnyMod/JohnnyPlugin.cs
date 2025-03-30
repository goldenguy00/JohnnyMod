using BepInEx;
using JohnnyMod.Survivors.Johnny;
using JohnnyMod.Survivors.Johnny.Components;
using R2API.Utils;
using RoR2;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using UnityEngine.Networking;

[module: UnverifiableCode]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete

//rename this namespace
namespace JohnnyMod
{
    //[BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    public class JohnnyPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.HasteReapr.JohnnyMod";
        public const string MODNAME = "JohnnyMod";
        public const string MODVERSION = "1.1.0";

        public const string DEVELOPER_PREFIX = "HASTEREAPR";

        public static JohnnyPlugin instance;

        public static bool EmoteApiInstalled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI");

        void Awake()
        {
            instance = this;

            Log.Init(Logger);

            Modules.Language.Init();

            new JohnnySurvivor().Initialize();
            new Modules.ContentPacks().Initialize();

            Hook();
        }

        private void Hook()
        {
            On.RoR2.MapZone.TryZoneStart += MapZone_TryZoneStart;
            //Run.onClientGameOverGlobal += Run_onClientGameOverGlobal;
            On.RoR2.Run.OnClientGameOver += Run_OnClientGameOver;

            //handles all of the emoteAPI compatability stuff
            On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
        }

        private void Run_OnClientGameOver(On.RoR2.Run.orig_OnClientGameOver orig, Run self, RunReport runReport)
        {
            orig(self, runReport);
            try
            {
                if (NetworkServer.active)
                {
                    //dont jumpscare me please
                    Util.PlaySound("PlayWinVoice", self.gameObject);
                }
            }
            catch (System.Exception e)
            {
                Log.Error(e);
                Log.Error("Had issue with RunOnClientGameOver call. But seeing this means the vanilla version ran.");
            }
        }

        private void Run_onClientGameOverGlobal(Run run, RunReport runReport)
        {
            bool isJohgn = false;
            for (int x = 0; x < runReport.playerInfoCount; x++)
            {
                Log.Message("Scanning for Johgnny");
                if (runReport.playerInfos[x].bodyName.Equals("JohnnyBody"))
                    isJohgn = true;
            }

            if (isJohgn)
            {
                if (runReport.gameEnding.isWin)
                {
                    Util.PlaySound("PlayWinVoice", run.gameObject);
                }
                else
                {
                    Util.PlaySound("PlayLostVoice", run.gameObject);
                }
            }
            Log.Message("Trying to play the win voice");
            Util.PlaySound("PlayWinVoice", run.gameObject);

            if (runReport.gameEnding.isWin)
            {
                Util.PlaySound("PlayWinVoice", run.gameObject);
            }
            else
            {
                Util.PlaySound("PlayLostVoice", run.gameObject);
            }
        }

        private void MapZone_TryZoneStart(On.RoR2.MapZone.orig_TryZoneStart orig, MapZone self, UnityEngine.Collider other)
        {
            // if we have the card component get out of this method and dont kys
            if (other.GetComponent<CardController>() && other.GetComponent<TeamComponent>().teamIndex != TeamIndex.Player)
            {
                return;
            }

            orig(self, other);
        }

        // idk i really didnt trust the previous implementation
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void SurvivorCatalog_Init(On.RoR2.SurvivorCatalog.orig_Init orig)
        {
            orig();

            if (JohnnyPlugin.EmoteApiInstalled)
                EmoteAPICompat.EmoteHook();
        }
    }
}
