﻿using Base;
using Base.Core;
using Base.Entities.Statuses;
using Base.UI;
using HarmonyLib;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Geoscape.Levels.Factions;
using PhoenixPoint.Geoscape.Levels.Factions.FesteringSkies;
using PhoenixPoint.Tactical.View.ViewStates;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static PhoenixPoint.Geoscape.Entities.GeoBehemothActor;

namespace TFTV
{
    internal class TFTVBehemothAndRaids
    {

        // private static readonly DefRepository Repo = TFTVMain.Repo;
        public static Dictionary<int, List<int>> flyersAndHavens = new Dictionary<int, List<int>>();
        public static List<int> targetsForBehemoth = new List<int>();
        //  public static List<int> targetsVisitedByBehemoth = new List<int>();

        public static List<int> behemothScenicRoute = new List<int>();
        public static int behemothTarget = 0;
        public static int behemothWaitHours = 12;
        // public static int roaming = 0;
        //public static bool firstPandoranFlyerSpawned = false;

        public static bool checkHammerfall = false;
        private static readonly string BehemothRoamings = "BehemothRoamings";

        internal class Hammerfall
        {
            //Hammerfall
            [HarmonyPatch(typeof(GeoAlienFaction), "SpawnEgg", new Type[] { typeof(Vector3) })]
            public static class GeoAlienFaction_SpawnEgg_DestroyHavens_Patch
            {

                public static void Postfix(GeoAlienFaction __instance, Vector3 worldPos)
                {
                    try
                    {
                        if (!checkHammerfall)
                        {

                            List<GeoHaven> geoHavens = __instance.GeoLevel.AnuFaction.Havens.ToList();
                            geoHavens.AddRange(__instance.GeoLevel.NewJerichoFaction.Havens.ToList());
                            geoHavens.AddRange(__instance.GeoLevel.SynedrionFaction.Havens.ToList());
                            int count = 0;
                            int damage = UnityEngine.Random.Range(25, 200);

                            string destroyedByHammerfall = new LocalizedTextBind() { LocalizationKey = "KEY_DESTROYED_HAMMERFALL" }.Localize();
                            string heavyDamageByHammerfall = new LocalizedTextBind() { LocalizationKey = "KEY_HEAVY_DAMAGE_HAMMERFALL" }.Localize();
                            string damageByHammerfall = new LocalizedTextBind() { LocalizationKey = "KEY_DAMAGE_HAMMERFALL" }.Localize();

                            foreach (GeoHaven haven in geoHavens)
                            {
                                //TFTVLogger.Always("");
                                if (Vector3.Distance(haven.Site.WorldPosition, worldPos) <= 1)

                                {
                                    // TFTVLogger.Always("This haven " + haven.Site.LocalizedSiteName + "is getting whacked by the asteroid");
                                    if (!haven.Site.HasActiveMission && count < 3 && Vector3.Distance(haven.Site.WorldPosition, worldPos) <= 0.4)
                                    {
                                        GeoscapeLogEntry entry = new GeoscapeLogEntry
                                        {
                                            Text = new LocalizedTextBind($"{haven.Site.Owner} {haven.Site.LocalizedSiteName} {destroyedByHammerfall}", true)
                                        };
                                        typeof(GeoscapeLog).GetMethod("AddEntry", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance.GeoLevel.Log, new object[] { entry, null });
                                        haven.Site.DestroySite();
                                        count++;
                                    }
                                    else
                                    {
                                        int startingPopulation = haven.Population;
                                        float havenPopulation = haven.Population * (float)(Vector3.Distance(haven.Site.WorldPosition, worldPos));
                                        haven.Population = Mathf.CeilToInt(havenPopulation);
                                        int damageToZones = Mathf.CeilToInt(150 / (Vector3.Distance(haven.Site.WorldPosition, worldPos)));
                                        haven.Zones.ToArray().ForEach(zone => zone.AddDamage(UnityEngine.Random.Range(damageToZones - 25, damageToZones + 25)));
                                        string destructionDescription;
                                        if (haven.Zones.First().Health <= 500 || startingPopulation >= haven.Population + 1000)
                                        {
                                            destructionDescription = heavyDamageByHammerfall;
                                        }
                                        else
                                        {
                                            destructionDescription = damageByHammerfall;

                                        }
                                        GeoscapeLogEntry entry = new GeoscapeLogEntry
                                        {
                                            Text = new LocalizedTextBind($"{haven.Site.Owner} {haven.Site.LocalizedSiteName} {destructionDescription}", true)
                                        };
                                        typeof(GeoscapeLog).GetMethod("AddEntry", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance.GeoLevel.Log, new object[] { entry, null });
                                        checkHammerfall = true;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                    }
                }

            }


        }

        internal class Flyers
        {
            // 
            private static AlienRaidBand AlienRaidBandGenerator(GeoLevelController controller)
            {
                try
                {
                    int difficulty = controller.CurrentDifficultyLevel.Order;
                    int numberOfRoamings = controller.EventSystem.GetVariable(BehemothRoamings);

                    AlienRaidBand alienRaidBand = new AlienRaidBand() { RaidType = AlienRaidType.BombHaven, AircraftTypesAllowed = AircraftType.Small, RollResultMax = 9999 };

                    if (numberOfRoamings > 1)
                    {
                        UnityEngine.Random.InitState((int)Stopwatch.GetTimestamp());

                        int roll = UnityEngine.Random.Range(0, 1 + difficulty);

                        if (roll > 0)
                        {
                            alienRaidBand.AircraftTypesAllowed = AircraftType.Medium;

                            if (numberOfRoamings > 2)
                            {
                                UnityEngine.Random.InitState((int)Stopwatch.GetTimestamp());

                                int roll2 = UnityEngine.Random.Range(0, 1 + difficulty);

                                if (roll > 1)
                                {
                                    alienRaidBand.AircraftTypesAllowed = AircraftType.Large;
                                }
                            }
                        }
                    }

                    return alienRaidBand;
                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }


       /*  [HarmonyPatch(typeof(AlienRaidManager), "UpdateHourly")]
            public static class AlienRaidManager_UpdateHourly_patch
            {
                public static void Postfix(AlienRaidManager __instance)
                {
                    try
                    {
                        TFTVLogger.Always($"Running AlienRaidManager.UpdateHourly, NextUpdateCountdownHrs is {__instance.NextUpdateCountdownHrs} ");

                    }
                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                    }
                }
            }*/

            [HarmonyPatch(typeof(AlienRaidManager), "RollForRaid")]
            public static class AlienRaidManager_RollForRaid_patch
            {
                public static bool Prefix(AlienRaidManager __instance)
                {
                    try
                    {
                        //  TFTVLogger.Always($"AlienRaidManager.RollForRaid running");
                        // 
                        if (__instance.AlienFaction.Behemoth == null || __instance.AlienFaction.Behemoth.CurrentBehemothStatus == BehemothStatus.Dormant || __instance.AlienFaction.Behemoth.IsSubmerging)//__instance.AlienFaction!=null && __instance.AlienFaction.Behemoth!=null && )
                        {
                           // TFTVLogger.Always($"AlienRaidManager.RollForRaid running, Behemoth not dormant");
                            return false;
                        }

                        TFTVLogger.Always($"AlienRaidManager.RollForRaid running, Behemoth not dormant");

                        // Get the MethodInfo for TryGenerateRaid method
                        MethodInfo tryGenerateRaidMethod = typeof(AlienRaidManager).GetMethod("TryGenerateRaid", BindingFlags.NonPublic | BindingFlags.Instance);
                        MethodInfo onRaidGeneratedMethod = typeof(AlienRaidManager).GetMethod("OnRaidGenerated", BindingFlags.NonPublic | BindingFlags.Instance);

                        List<InfestedHavenAircraftData> dummyList = new List<InfestedHavenAircraftData>();

                        GeoLevelController controller = __instance.AlienFaction.GeoLevel;

                        int behemothRoamings = controller.EventSystem.GetVariable(BehemothRoamings);
                        int flyers = Math.Min(behemothRoamings, 2);

                        for (int x = 0; x < flyers+1; x++)
                        {
                            AlienRaidBand raidBand = AlienRaidBandGenerator(__instance.AlienFaction.GeoLevel);
                            TFTVLogger.Always($"generated raidband for {raidBand.AircraftTypesAllowed}");
                            GeoscapeRaid geoscapeRaid = (GeoscapeRaid)tryGenerateRaidMethod.Invoke(__instance, new object[] { raidBand, dummyList });

                            if (geoscapeRaid != null)
                            {
                                onRaidGeneratedMethod.Invoke(__instance, new object[] { geoscapeRaid });
                            }
                        }

                        return false;
                    }
                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                        throw;
                    }
                }
            }





            //Trigger event on first Pandoran flyer
            [HarmonyPatch(typeof(GeoFaction), "CreateVehicleAtPosition")]
            public static class GeoFaction_CreateVehicleAtPosition_patch
            {
                public static void Postfix(ComponentSetDef vehicleDef, GeoFaction __instance)
                {
                    try
                    {
                        if (vehicleDef.name == "ALN_GeoscapeFlyer_Small" && __instance.GeoLevel.EventSystem.GetVariable("FirstPandoranFlyerSpawned") != 1)
                        {
                            GeoscapeEventContext geoscapeEventContext = new GeoscapeEventContext(__instance, __instance.GeoLevel.ViewerFaction);
                            __instance.GeoLevel.EventSystem.TriggerGeoscapeEvent("OlenaOnFirstFlyer", geoscapeEventContext);
                            __instance.GeoLevel.EventSystem.SetVariable("FirstPandoranFlyerSpawned", 1);
                        }
                    }
                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                    }
                }
            }

            //patch to reveal havens under attack
            [HarmonyPatch(typeof(GeoscapeRaid), "StartAttackEffect")]
            public static class GeoscapeRaid_StartAttackEffect_patch
            {
                public static void Postfix(GeoscapeRaid __instance)
                {
                    try
                    {
                        TFTVCommonMethods.RevealHavenUnderAttack(__instance.GeoVehicle.CurrentSite, __instance.GeoVehicle.GeoLevel);
                    }

                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                    }
                }
            }

            //Controlling Pandoran flyers visiting havens
            [HarmonyPatch(typeof(GeoVehicle), "OnArrivedAtDestination")]
            public static class GeoVehicle_OnArrivedAtDestination
            {

                public static void Postfix(GeoVehicle __instance, bool justPassing)
                {
                    try
                    {
                        if (!justPassing && __instance.Owner.IsAlienFaction && __instance.CurrentSite.Type == GeoSiteType.Haven)
                        {

                            if (flyersAndHavens.ContainsKey(__instance.VehicleID))
                            {
                                flyersAndHavens[__instance.VehicleID].Add(__instance.CurrentSite.SiteId);
                            }
                            else
                            {
                                flyersAndHavens.Add(__instance.VehicleID, new List<int> { __instance.CurrentSite.SiteId });
                            }
                            TFTVLogger.Always("Added to list of havens visited by flyer " + __instance.CurrentSite.LocalizedSiteName);
                        }
                    }

                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                    }
                }
            }

            //Verifiying if flyer returning to Behemoth has visited a haven
            [HarmonyPatch(typeof(GeoscapeRaid), "StopBehemothFollowing")]
            public static class GeoscapeRaid_StopBehemothFollowing_patch
            {

                public static void Prefix(GeoscapeRaid __instance)
                {
                    try
                    {
                        GeoBehemothActor behemoth = (GeoBehemothActor)UnityEngine.Object.FindObjectOfType(typeof(GeoBehemothActor));
                        // TFTVLogger.Always("Behemoth is submerging? " + behemoth.IsSubmerging);

                        if (flyersAndHavens.ContainsKey(__instance.GeoVehicle.VehicleID))
                        {
                            TFTVLogger.Always("Flyer returning to B passed first check");

                            if (targetsForBehemoth.Count > 1000) 
                            {
                                targetsForBehemoth.Clear();                            
                            }

                            foreach (int haven in flyersAndHavens[__instance.GeoVehicle.VehicleID])
                            {  
                                GeoSite geoSite = Behemoth.ConvertIntIDToGeosite(behemoth.GeoLevel, haven);

                                TFTVLogger.Always($"Checking {geoSite?.LocalizedSiteName} visited by the flyer. Is it now destroyed? {geoSite?.State == GeoSiteState.Destroyed} Does it have an Active Mission? {geoSite.HasActiveMission}");

                                if (!targetsForBehemoth.Contains(haven) && geoSite!=null && geoSite.State!=GeoSiteState.Destroyed && !geoSite.HasActiveMission) //&& !targetsVisitedByBehemoth.Contains(haven)) //&& (behemoth != null && !behemoth.IsSubmerging && behemoth.CurrentBehemothStatus != GeoBehemothActor.BehemothStatus.Dormant))
                                {
                                    targetsForBehemoth.Add(haven);

                                    TFTVLogger.Always($"{geoSite?.LocalizedSiteName} added to the list of targets");
                                }
                            }              

                            flyersAndHavens.Remove(__instance.GeoVehicle.VehicleID);
                        }
                    }

                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                    }
                }
            }

        }

        internal class Behemoth
        {

            [HarmonyPatch(typeof(GeoBehemothActor), "UpdateHourly")]
            public static class GeoBehemothActor_UpdateHourly_Patch
            {
                public static bool Prefix(GeoBehemothActor __instance, ref int ____disruptionThreshhold, int ____disruptionPoints, int ____nextActionHoursLeft)
                {
                    try
                    {
                        //TFTVLogger.Always("Total sites count is " + __instance.GeoLevel.Map.AllSites.Count);

                        GeoLevelController controller = __instance.GeoLevel;

                        if (__instance.CurrentBehemothStatus == BehemothStatus.Dormant)//first check
                        {
                            //   TFTVLogger.Always("Behemoth's target lists are cleared because he is sleeping");
                            targetsForBehemoth.Clear();
                            //  targetsVisitedByBehemoth.Clear();
                            behemothScenicRoute.Clear();
                            behemothTarget = 0;
                            return true;
                        }

                        if (behemothTarget <0) 
                        {
                            TFTVLogger.Always($"Somehow Behemoth Target was -1, so setting it to 0");
                            behemothTarget = 0;
                        }

                      /*  foreach(int targetId in targetsForBehemoth) 
                        {
                            TFTVLogger.Always($"{targetId}");
                        
                        }*/

                        if (targetsForBehemoth.Count > 1000)
                        {
                            TFTVLogger.Always($"Somehow Behemoth Targets were at more than 1k, setting them to 0");
                            targetsForBehemoth.Clear();
                        }

                        if (behemothScenicRoute.Count > 1000)
                        {
                            TFTVLogger.Always($"Somehow Behemoth scenic route were at more than 1k, setting them to 0");
                            behemothScenicRoute.Clear();
                        }

                        /*  if (__instance.GeoLevel.EventSystem.GetVariable("ThirdActStarted") == 1)
                          {
                              ____disruptionThreshhold = 200;
                          }*/

                        //   if (____disruptionThreshhold <= 0)
                        //   {
                        MethodInfo CalculateDisruptionThreshholdMethod = AccessTools.Method(typeof(GeoBehemothActor), "CalculateDisruptionThreshhold");

                            ____disruptionThreshhold = (int)CalculateDisruptionThreshholdMethod.Invoke(__instance, null);

                            TFTVLogger.Always($"Behemoth hourly update, disruption threshold set to {____disruptionThreshhold}, disruption points are {____disruptionPoints}");
                      //  }

                        if (!__instance.IsSubmerging && ____disruptionPoints >= ____disruptionThreshhold)
                        {
                            if (__instance.CurrentSite != null)
                            {
                                MethodInfo method_GenerateTargetData = AccessTools.Method(typeof(GeoBehemothActor), "PickSubmergeLocation");

                                method_GenerateTargetData.Invoke(__instance, null);
                                TFTVLogger.Always($"Behemoth hourly update, disruption points at {____disruptionPoints}, while threshold set to {____disruptionThreshhold}. Behemoth should submerge");
                                return false;
                            }
                        }

                        ____nextActionHoursLeft = Mathf.Clamp(____nextActionHoursLeft - 1, 0, int.MaxValue);

                        if (____nextActionHoursLeft <= 0)
                        {
                            MethodInfo method_GenerateTargetData = AccessTools.Method(typeof(GeoBehemothActor), "PerformAction");
                            method_GenerateTargetData.Invoke(__instance, null);
                            TFTVLogger.Always($"Behemoth hourly update{____nextActionHoursLeft} hours left to move, so time to move");

                        }


                        if (__instance.IsSubmerging)//second check
                        {
                            // TFTVLogger.Always("Behemoth's target lists are cleared because he is going to sleep");
                            targetsForBehemoth.Clear();
                            behemothScenicRoute.Clear();
                            behemothTarget = 0;
                            return false;
                        }

                        GeoSite targetHaven = ConvertIntIDToGeosite(__instance.GeoLevel, behemothTarget);


                        if (behemothTarget != 0 && targetHaven != null && (targetHaven.State == GeoSiteState.Destroyed || targetHaven.ActiveMission!=null))
                        {
                            behemothTarget = 0;
                        }
                        else if (behemothTarget != 0)
                        {
                            // TFTVLogger.Always("TargetHavenEvent should trigger");
                            if (__instance.GeoLevel.EventSystem.GetVariable("BehemothTargettedFirstHaven") != 1)
                            {
                                GeoscapeEventContext geoscapeEventContext = new GeoscapeEventContext(__instance.GeoLevel.AlienFaction, __instance.GeoLevel.ViewerFaction);
                                __instance.GeoLevel.EventSystem.TriggerGeoscapeEvent("OlenaOnFirstHavenTarget", geoscapeEventContext);
                                __instance.GeoLevel.EventSystem.SetVariable("BehemothTargettedFirstHaven", 1);
                                TFTVLogger.Always("OlenaOnFirstHavenTarget event triggered");
                            }
                        }

                        CullTargetList(controller);

                        if (behemothTarget == 0 && targetsForBehemoth.Count > 0)
                        {
                            TFTVLogger.Always("Behemoth has no current target and there are " + targetsForBehemoth.Count() + " available targets");

                            GeoSite chosenHaven = GetTargetHaven(__instance.GeoLevel);
                            targetsForBehemoth.Remove(chosenHaven.SiteId);
                            behemothTarget = chosenHaven.SiteId;
                            if (__instance.CurrentSite != null && __instance.CurrentSite == chosenHaven && targetsForBehemoth.Count > 0)
                            {

                                chosenHaven = GetTargetHaven(__instance.GeoLevel);
                                targetsForBehemoth.Remove(chosenHaven.SiteId);
                                behemothTarget = chosenHaven.SiteId;

                            }
                            else if (__instance.CurrentSite != null && __instance.CurrentSite == chosenHaven && targetsForBehemoth.Count == 0)
                            {
                                TFTVLogger.Always("Behemoth is at a haven, the target is the haven and has no other targets: has to move somewhere");
                                typeof(GeoBehemothActor).GetMethod("TargetHaven", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { GetSiteForBehemothToMoveTo(__instance) });
                                return false;
                            }

                            typeof(GeoBehemothActor).GetMethod("TargetHaven", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { chosenHaven });
                            return false;

                        }
                        else if (behemothTarget == 0 && targetsForBehemoth.Count == 0) // no potential targets, set Behemoth to roam
                        {
                            if (behemothWaitHours == 0)
                            {
                                TFTVLogger.Always("No targets, waiting time is up, Behemoth moves somewhere");
                                if (GetSiteForBehemothToMoveTo(__instance) != null)
                                {
                                    typeof(GeoBehemothActor).GetMethod("TargetHaven", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { GetSiteForBehemothToMoveTo(__instance) });
                                    behemothWaitHours = 12;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                behemothWaitHours--;
                            }
                        }
                        return false;
                    }//end of try

                    catch //(Exception e)
                    {
                        // TFTVLogger.Error(e);
                    }
                    return true;
                }
            }

            //Clear lists of internal variables on Behemoth submerge + add Berith and Abbadon researches depending on number of roamings
            [HarmonyPatch(typeof(GeoBehemothActor), "PickSubmergeLocation")]
            public static class GeoBehemothActor_PickSubmergeLocation_patch
            {
                public static void Postfix(GeoBehemothActor __instance)
                {
                    try
                    {
                        TFTVLogger.Always("Behemoth submerging");
                        //  BehemothSubmerging = true;
                        flyersAndHavens.Clear();
                        targetsForBehemoth.Clear();
                        behemothScenicRoute.Clear();

                        //  BehemothSubmerging = true;
                        if (__instance.GeoLevel.EventSystem.GetVariable(BehemothRoamings) < 1)//4 - __instance.GeoLevel.CurrentDifficultyLevel.Order <= roaming) 
                        {
                            __instance.GeoLevel.EventSystem.SetVariable("BerithResearchVariable", 1);
                            TFTVLogger.Always("Aliens should now have Beriths");
                        }
                        else if (__instance.GeoLevel.EventSystem.GetVariable(BehemothRoamings) == 2)//4 - __instance.GeoLevel.CurrentDifficultyLevel.Order <= roaming) 
                        {
                            __instance.GeoLevel.EventSystem.SetVariable("AbbadonResearchVariable", 1);
                            TFTVLogger.Always("Aliens should now have Abbadons");
                        }
                        else if (__instance.GeoLevel.EventSystem.GetVariable(BehemothRoamings) == 3)//4 - __instance.GeoLevel.CurrentDifficultyLevel.Order <= roaming) 
                        {

                            if (__instance.GeoLevel.EventSystem.GetVariable("BehemothPatternEventTriggered") == 0)
                            {
                                GeoscapeEventContext context = new GeoscapeEventContext(__instance.GeoLevel.AlienFaction, __instance.GeoLevel.PhoenixFaction);
                                __instance.GeoLevel.EventSystem.TriggerGeoscapeEvent("OlenaOnBehemothPattern", context);
                                __instance.GeoLevel.EventSystem.SetVariable("BehemothPatternEventTriggered", 1);
                                TFTVLogger.Always("Event on Behemoth pattern should trigger");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                    }
                }
            }

            //Patch to adjust disprution threshold
            [HarmonyPatch(typeof(GeoBehemothActor), "CalculateDisruptionThreshhold")]
            public static class GeoBehemothActor_CalculateDisruptionThreshhold_patch
            {

                public static void Postfix(GeoBehemothActor __instance, ref int __result, int ____disruptionPoints)
                {
                    try
                    {
                        FesteringSkiesSettingsDef festeringSkiesSettings = __instance.GeoLevel.FesteringSkiesSettings;
                        GameDifficultyLevelDef currentDifficultyLevel = __instance.GeoLevel.CurrentDifficultyLevel;
                        int num = festeringSkiesSettings.DisruptionThreshholdBaseValue + currentDifficultyLevel.DisruptionDueToDifficulty;
                        num += __instance.GeoLevel.EventSystem.GetVariable(BehemothRoamings) * 2;

                        //  TFTVLogger.Always("The num is " + num);

                        TFTVLogger.Always($"Calculating Disruption Threshold for Big B. " +
                            $"Base value: {festeringSkiesSettings.DisruptionThreshholdBaseValue} " +
                            $"From Difficulty: {currentDifficultyLevel.DisruptionDueToDifficulty}  " +
                            $"Roaming: {__instance.GeoLevel.EventSystem.GetVariable(BehemothRoamings)} " +
                            $"Total: {num}");

                        int[] voidOmensInEffect = TFTVVoidOmens.CheckFordVoidOmensInPlay(__instance.GeoLevel);
                        if (voidOmensInEffect.Contains(11))
                        {
                            num += 3 * TFTVReleaseOnly.DifficultyOrderConverter(currentDifficultyLevel.Order);
                            TFTVLogger.Always($"And with VO# 11 in effect, total is now {num}");
                        }

                        __result = num;
                        //  TFTVLogger.Always($"calculate disruption Threshhold result it {__result}");
                    }

                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                    }
                }
            }

            //Patch to ensure that Behemoth emerges near exploration sites, written with the help of my new best friend, chatgpt
            [HarmonyPatch(typeof(GeoBehemothActor), "OnBehemothEmerged")]
            class TFTV_OnBehemothEmerged_Patch
            {

                public static bool Prefix(GeoBehemothActor __instance, ref int ____disruptionPoints, ref int ____disruptionThreshhold, ref BehemothStatus ____currentBehemothStatus,
                     ref int ____nextActionHoursLeft, ref Vector3 ____submergeEmergeEndPoint, ref Vector3 ____submergeEmergeStartPoint,
                     IUpdateable ____submergeOrEmergeCrt, GameObject ____submergeOrEmergeVfx)
                {
                    try
                    {
                        GeoLevelController controller = __instance.GeoLevel;

                        __instance.GeoLevel.EventSystem.SetVariable(BehemothRoamings, __instance.GeoLevel.EventSystem.GetVariable(BehemothRoamings) + 1);

                        TFTVLogger.Always($"Behemoth emerging, this is romaing # {__instance.GeoLevel.EventSystem.GetVariable(BehemothRoamings)}");


                        if (__instance.GeoLevel.PhoenixFaction.Research.HasCompleted("PX_YuggothianEntity_ResearchDef")
                            && __instance.GeoLevel.PhoenixFaction.Research.HasCompleted("PX_Alien_Citadel_ResearchDef")
                            && __instance.GeoLevel.EventSystem.GetVariable("BehemothPatternEventTriggered") != 1)
                        {
                            GeoscapeEventContext context = new GeoscapeEventContext(__instance.GeoLevel.AlienFaction, __instance.GeoLevel.PhoenixFaction);
                            __instance.GeoLevel.EventSystem.TriggerGeoscapeEvent("OlenaOnBehemothPattern", context);
                            __instance.GeoLevel.EventSystem.SetVariable("BehemothPatternEventTriggered", 1);
                            TFTVLogger.Always("Event on Behemoth pattern should trigger");
                        }


                        GeoSite randomElement = __instance.GeoLevel.Map.SitesByType[GeoSiteType.MistGenerator]
                            .Where(s => s != __instance.CurrentSite)
                            .Where(s => controller.Map.SitesByType[GeoSiteType.Exploration]
                                .Count(e => Vector3.Distance(e.WorldPosition, s.WorldPosition) <= 5) >= 5)
                             .Where(s => controller.Map.SitesByType[GeoSiteType.Haven]
                                 .Count(e => Vector3.Distance(e.WorldPosition, s.WorldPosition) <= 5) >= 5)
                            .ToList().GetRandomElement();


                        Type targetType = typeof(GeoBehemothActor);
                        FieldInfo eventField = targetType.GetField("OnEmerged", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        BehemothSiteEventHandler OnEmerged = (BehemothSiteEventHandler)eventField.GetValue(__instance);

                        if (randomElement == null) return false;

                        __instance.TeleportToSite(randomElement);

                        // rest of the original method, adapted for patch
                        __instance.ModelRoot.transform.localPosition = new Vector3(0f, -0.375f, 0f);
                        __instance.VisualsRoot.gameObject.SetActive(value: true);
                        OnEmerged?.Invoke(__instance.CurrentSite);
                        __instance.GeoLevel.View.ChaseTarget(__instance.CurrentSite, instant: true);
                        ____currentBehemothStatus = BehemothStatus.None;
                        ____nextActionHoursLeft = 0;

                        MethodInfo performActionMethod = AccessTools.Method(typeof(GeoBehemothActor), "PerformAction");
                        performActionMethod.Invoke(__instance, null);

                        MethodInfo getEmergePointMethod = AccessTools.Method(typeof(GeoBehemothActor), "GetEmergePoint");
                        ____submergeEmergeEndPoint = (Vector3)getEmergePointMethod.Invoke(__instance, null);
                        ____submergeEmergeStartPoint = __instance.WorldPosition;

                        MethodInfo EmergeCrtMethod = AccessTools.Method(typeof(GeoBehemothActor), "EmergeCrt");
                        object emergeCrtResult = EmergeCrtMethod.Invoke(__instance, new object[] { new Timing() });

                        if (__instance.BehemothDef.EmergeVFX != null)
                        {
                            ____submergeOrEmergeVfx = UnityEngine.Object.Instantiate(__instance.BehemothDef.EmergeVFX, __instance.VFXRoot);
                            ____submergeOrEmergeVfx.transform.localPosition = Vector3.zero;
                        }

                        MethodInfo CalculateDisruptionThreshholdMethod = AccessTools.Method(typeof(GeoBehemothActor), "CalculateDisruptionThreshhold");

                        ____disruptionPoints = 0;
                        ____disruptionThreshhold = (int)CalculateDisruptionThreshholdMethod.Invoke(__instance, null);

                        TFTVLogger.Always($"disruption threshold set at {____disruptionThreshhold}");

                        return false;
                    }
                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                    }
                    return true;
                }
            }

            [HarmonyPatch(typeof(GeoBehemothActor), "DamageHavenOutcome")]
            public static class GeoBehemothActor_DamageHavenOutcome_Patch
            {

                public static void Postfix(GeoBehemothActor __instance, ref int ____disruptionPoints)
                {
                    try
                    {
                        TFTVLogger.Always("DamageHavenOutcome method invoked");

                        if (__instance.GeoLevel.EventSystem.GetVariable("BehemothAttackedFirstHaven") != 1)
                        {
                            GeoscapeEventContext geoscapeEventContext = new GeoscapeEventContext(__instance.GeoLevel.PhoenixFaction, __instance.GeoLevel.ViewerFaction);
                            __instance.GeoLevel.EventSystem.TriggerGeoscapeEvent("OlenaOnFirstHavenAttack", geoscapeEventContext);
                            __instance.GeoLevel.EventSystem.SetVariable("BehemothAttackedFirstHaven", 1);
                            TFTVLogger.Always("FirstHavenTarget event triggered");
                        }


                        behemothTarget = 0;
                        ____disruptionPoints += 4; //increase on 28/12 from 1 
                        TFTVLogger.Always("The DP are " + ____disruptionPoints);
                        // TFTVLogger.Always("DamageHavenOutcome method invoked and Behemoth target is now " + behemothTarget);
                    }

                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                    }

                }
            }

            [HarmonyPatch(typeof(GeoBehemothActor), "ChooseNextHavenTarget")]
            public static class GeoBehemothActor_ChooseNextHavenTarget_Patch
            {


                public static bool Prefix(GeoBehemothActor __instance)
                {
                    try
                    {
                        if (targetsForBehemoth.Count == 0 && behemothTarget == 0)
                        {
                            GeoSite site = GetSiteForBehemothToMoveTo(__instance);
                            typeof(GeoBehemothActor).GetMethod("TargetHaven", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] { site });

                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                    }
                    return false;
                }
            }

            private static void CullTargetList(GeoLevelController controller)
            {
                try 
                {
                    List<int> targetsToBeCulled = new List<int>();
                    
                    foreach(int siteId in targetsForBehemoth) 
                    {
                        GeoSite targetHaven = ConvertIntIDToGeosite(controller, siteId);
                        
                        if(targetHaven.State == GeoSiteState.Destroyed || targetHaven.ActiveMission != null) 
                        {
                            targetsToBeCulled.Add(targetHaven.SiteId);
                        }
                    
                    }

                    if (targetsToBeCulled.Count > 0) 
                    {
                        targetsForBehemoth.RemoveRange(targetsToBeCulled);
                    
                    }
                   
                
                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                }
            }

            private static GeoSite GetTargetHaven(GeoLevelController level)
            {
                try
                {
                    List<GeoHaven> geoHavens = level.AnuFaction.Havens.ToList();
                    geoHavens.AddRange(level.NewJerichoFaction.Havens.ToList());
                    geoHavens.AddRange(level.SynedrionFaction.Havens.ToList());

                    int idOfHaven = targetsForBehemoth.First();
                    GeoSite target = new GeoSite();
                    foreach (GeoHaven haven in geoHavens)
                    {
                        if (haven.Site.SiteId == idOfHaven)
                        {
                            target = haven.Site;
                        }
                    }
                    return target;


                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                }
                throw new InvalidOperationException();
            }

            internal static GeoSite ConvertIntIDToGeosite(GeoLevelController controller, int siteID)
            {
                try
                {
                    List<GeoSite> allGeoSites = controller.Map.AllSites.ToList();
                    foreach (GeoSite site in allGeoSites)
                    {
                        if (site != null && site.SiteId == siteID)
                        {
                            return site;
                        }
                    }
                    behemothTarget = 0;
                    return null;
                }

                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                }
                throw new InvalidOperationException();
            }

            public static GeoSite GetSiteForBehemothToMoveTo(GeoBehemothActor geoBehemothActor)
            {
                try
                {
                    GeoLevelController controller = geoBehemothActor.GeoLevel;

                    TFTVLogger.Always($"TargetsForBehemoth counts {targetsForBehemoth.Count()} and/but counted as 0, so here we are");

                    //Get site closest to Behemoth  
                    GeoSite behemothSite = geoBehemothActor.GeoLevel.Map.GetClosestSite_Land(geoBehemothActor.WorldPosition, false);

                    if (behemothSite != null)
                    {
                        TFTVLogger.Always("Found fallback site on same landmass near the Behemoth");
                    }
                    else
                    {
                        TFTVLogger.Always("No site connected to land near the Behemoth!!!");
                        IOrderedEnumerable<GeoSite> closestSiteAnywhere = from s in controller.Map.AllSites.Where(gs => !controller.Map.IsInWater(gs.Pos))
                                                                          orderby GeoMap.Distance(geoBehemothActor.Pos, s.Pos)
                                                                          select s;
                        behemothSite = closestSiteAnywhere.First();

                        TFTVLogger.Always("Found closest site anywhere that is not in water");
                    }


                    GeoSite chosenTarget = behemothSite; //by default, in case no other sites are found

                    if (behemothScenicRoute.Count > controller.Map.AllSites.Where(s => s.Type == GeoSiteType.Exploration).Count())
                    {
                        TFTVLogger.Always($"scenic route has {behemothScenicRoute.Count} sites, while there are only {controller.Map.AllSites.Where(s => s.Type == GeoSiteType.Exploration).Count()} sites in total! Clearing the list.");
                        behemothScenicRoute.Clear();
                    }

                    if (behemothScenicRoute.Count == 0)
                    {

                        //Get all sites connected to it, excluding the BehemothSite, ordered by distance from it
                        IOrderedEnumerable<GeoSite> sitesBehemothCanVisit = from s in controller.Map.GetConnectedSitesOfType_Land(behemothSite, GeoSiteType.Exploration).Where(gs => gs != behemothSite && gs.SiteTags.Count == 0)
                                                                            orderby GeoMap.Distance(behemothSite, s)
                                                                            select s;
                        if (sitesBehemothCanVisit.Count() > 0)
                        {
                            foreach (GeoSite target in sitesBehemothCanVisit)
                            {
                                if (!behemothScenicRoute.Contains(target.SiteId))
                                {
                                    behemothScenicRoute.Add(target.SiteId);
                                }
                            }
                        }
                    }

                    if (behemothScenicRoute.Count > 0)
                    {
                        TFTVLogger.Always("Actually got to the scenic Route count, and it's " + behemothScenicRoute.Count);

                        foreach (GeoSite site in geoBehemothActor.GeoLevel.Map.AllSites)
                        {
                            if (behemothScenicRoute.Contains(site.SiteId))
                            {
                                chosenTarget = site;
                                // TFTVLogger.Always("The site is " + site.Name);
                                behemothScenicRoute.Remove(site.SiteId);
                                return chosenTarget;
                            }
                        }
                    }
                    TFTVLogger.Always("Didn't find a site on the scenic route, so defaulting to nearest site, which is " + chosenTarget.Name);
                    return chosenTarget;
                }

                catch //(Exception e)
                {
                    //  TFTVLogger.Error(e);
                }
                throw new InvalidOperationException();
            }
        }

        public static void SetBehemothOnRampageMod(GeoLevelController geoLevel)
        {
            try
            {

                if (geoLevel.EventSystem.GetVariable("BehemothRoamings") >= 3)
                {
                    geoLevel.CurrentDifficultyLevel.DestroyHavenOutcomeChance = 50;
                    geoLevel.FesteringSkiesSettings.NumOfHavensToDestroyBeforeSubmerge = 4;
                    geoLevel.CurrentDifficultyLevel.DamageHavenOutcomeChance = 50;
                }

                else
                {
                    geoLevel.CurrentDifficultyLevel.DestroyHavenOutcomeChance = 0;
                    geoLevel.CurrentDifficultyLevel.DamageHavenOutcomeChance = 100;

                }
            }
            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }
        }

        /// <summary>
        /// To ensure that mission critical sites do not get blocked by Behemoth targetting them.
        /// </summary>

        [HarmonyPatch(typeof(GeoSite), "get_IsFreeForEncounter")]
        public static class GeoSite_IsFreeForEncounter_TImeVaultBugHung_patch
        {
            public static void Postfix(GeoSite __instance, ref bool __result)
            {
                try
                {

                    if (__result == false && __instance.Type == GeoSiteType.Exploration && __instance.IsTargetedByBehemoth)
                    {
                        TFTVLogger.Always($"reverting result of IsFreeForEncounter for site {__instance.SiteId}");
                        __result = true;
                    }

                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }
        }
    }
}


