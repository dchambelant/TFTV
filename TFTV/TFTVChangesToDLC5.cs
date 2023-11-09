﻿using Assets.Code.PhoenixPoint.Geoscape.Entities.Sites.TheMarketplace;
using Base;
using Base.Core;
using Base.Defs;
using Base.UI;
using Epic.OnlineServices.P2P;
using HarmonyLib;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities.Addons;
using PhoenixPoint.Common.Entities.GameTags;
using PhoenixPoint.Common.Entities.GameTagsTypes;
using PhoenixPoint.Common.Entities.Items;
using PhoenixPoint.Common.Levels.Missions;
using PhoenixPoint.Common.View.ViewControllers;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Entities.Research;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Geoscape.View.ViewControllers;
using PhoenixPoint.Geoscape.View.ViewModules;
using PhoenixPoint.Geoscape.View.ViewStates;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Tactical.Entities.Equipments;
using PhoenixPoint.Tactical.Entities.Weapons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using static PhoenixPoint.Geoscape.Levels.GeoMissionGenerator;

namespace TFTV
{


    internal class TFTVChangesToDLC5
    {

        private static readonly DefRepository Repo = TFTVMain.Repo;
        private static readonly DefCache DefCache = TFTVMain.Main.DefCache;
        private static readonly SharedData Shared = TFTVMain.Shared;
        public static GameTagDef MercenaryTag;

        internal class TFTVMercenaries
        {

            private static readonly DefCache DefCache = TFTVMain.Main.DefCache;
            private static readonly DefRepository Repo = TFTVMain.Repo;
            private static readonly SharedData Shared = TFTVMain.Shared;


            internal static ClassTagDef assaultTag = DefCache.GetDef<ClassTagDef>("Assault_ClassTagDef");
            internal static ClassTagDef heavyTag = DefCache.GetDef<ClassTagDef>("Heavy_ClassTagDef");
            internal static ClassTagDef infiltratorTag = DefCache.GetDef<ClassTagDef>("Infiltrator_ClassTagDef");
            internal static ClassTagDef sniperTag = DefCache.GetDef<ClassTagDef>("Sniper_ClassTagDef");
            internal static ClassTagDef priestTag = DefCache.GetDef<ClassTagDef>("Priest_ClassTagDef");
            internal static ClassTagDef technicianTag = DefCache.GetDef<ClassTagDef>("Technician_ClassTagDef");
            internal static ClassTagDef berserkerTag = DefCache.GetDef<ClassTagDef>("Berserker_ClassTagDef");

            internal static TacticalItemDef goldAssaultHelmet = DefCache.GetDef<TacticalItemDef>("PX_Assault_Helmet_Gold_BodyPartDef");
            internal static TacticalItemDef goldAssaultTorso = DefCache.GetDef<TacticalItemDef>("PX_Assault_Torso_Gold_BodyPartDef");
            internal static TacticalItemDef goldAssaultLegs = DefCache.GetDef<TacticalItemDef>("PX_Assault_Legs_Gold_ItemDef");

            internal static List<TacticalItemDef> goldAssaultArmor = new List<TacticalItemDef>() { goldAssaultHelmet, goldAssaultTorso, goldAssaultLegs };

            internal static TacticalItemDef goldHeavyHelmet = DefCache.GetDef<TacticalItemDef>("PX_Heavy_Helmet_Gold_BodyPartDef");
            internal static TacticalItemDef goldHeavyTorso = DefCache.GetDef<TacticalItemDef>("PX_Heavy_Torso_Gold_BodyPartDef");
            internal static TacticalItemDef goldHeavytLegs = DefCache.GetDef<TacticalItemDef>("PX_Heavy_Legs_Gold_ItemDef");
            internal static TacticalItemDef goldHeavytJetpack = DefCache.GetDef<TacticalItemDef>("PX_Heavy_Torso_JumpPack_Gold_BodyPartDef");

            internal static List<TacticalItemDef> goldHeavyArmor = new List<TacticalItemDef>() { goldHeavyHelmet, goldHeavytJetpack, goldHeavytLegs, goldHeavyTorso };

            internal static TacticalItemDef goldSniperHelmet = DefCache.GetDef<TacticalItemDef>("PX_Sniper_Helmet_Gold_BodyPartDef");
            internal static TacticalItemDef goldSniperTorso = DefCache.GetDef<TacticalItemDef>("PX_Sniper_Torso_Gold_BodyPartDef");
            internal static TacticalItemDef goldSniperLegs = DefCache.GetDef<TacticalItemDef>("PX_Sniper_Legs_Gold_ItemDef");

            internal static List<TacticalItemDef> goldSniperArmor = new List<TacticalItemDef>() { goldSniperHelmet, goldSniperTorso, goldSniperLegs };

            internal static TacticalItemDef spyMasterTorso = DefCache.GetDef<TacticalItemDef>("SY_Infiltrator_Bonus_Torso_BodyPartDef");
            internal static TacticalItemDef spyMasterHelmet = DefCache.GetDef<TacticalItemDef>("SY_Infiltrator_Bonus_Helmet_BodyPartDef");
            internal static TacticalItemDef spyMasterLegs = DefCache.GetDef<TacticalItemDef>("SY_Infiltrator_Bonus_Legs_ItemDef");

            internal static List<TacticalItemDef> spyMasterArmor = new List<TacticalItemDef>() { spyMasterHelmet, spyMasterLegs, spyMasterTorso };

            internal static TacticalItemDef sectarianHelmet = DefCache.GetDef<TacticalItemDef>("AN_Berserker_Helmet_Viking_BodyPartDef");
            internal static TacticalItemDef sectarianTorso = DefCache.GetDef<TacticalItemDef>("AN_Berserker_Torso_Viking_BodyPartDef");
            internal static TacticalItemDef sectarianLegs = DefCache.GetDef<TacticalItemDef>("AN_Berserker_Legs_Viking_ItemDef");

            internal static List<TacticalItemDef> sectarianArmor = new List<TacticalItemDef>() { sectarianHelmet, sectarianLegs, sectarianTorso };

            internal static TacticalItemDef ghostTorso = DefCache.GetDef<TacticalItemDef>("SY_Assault_Torso_Neon_BodyPartDef");
            internal static TacticalItemDef ghostHelmet = DefCache.GetDef<TacticalItemDef>("SY_Assault_Helmet_Neon_BodyPartDef");
            internal static TacticalItemDef ghostLegs = DefCache.GetDef<TacticalItemDef>("SY_Assault_Legs_Neon_ItemDef");

            internal static List<TacticalItemDef> ghostArmor = new List<TacticalItemDef>() { ghostHelmet, ghostTorso, ghostLegs };

            internal static TacticalItemDef exileHelmet = DefCache.GetDef<TacticalItemDef>("SY_Assault_Helmet_WhiteNeon_BodyPartDef");
            internal static TacticalItemDef exileTorso = DefCache.GetDef<TacticalItemDef>("SY_Assault_Torso_WhiteNeon_BodyPartDef");
            internal static TacticalItemDef exileLegs = DefCache.GetDef<TacticalItemDef>("SY_Assault_Legs_WhiteNeon_ItemDef");

            internal static List<TacticalItemDef> exileArmor = new List<TacticalItemDef>() { exileHelmet, exileLegs, exileTorso };

            internal static TacticalItemDef slugHelmet = DefCache.GetDef<TacticalItemDef>("NJ_Technician_Helmet_ALN_BodyPartDef");
            internal static TacticalItemDef slugLegs = DefCache.GetDef<TacticalItemDef>("NJ_Technician_Legs_ALN_ItemDef");
            internal static TacticalItemDef slugTorso = DefCache.GetDef<TacticalItemDef>("NJ_Technician_Torso_ALN_BodyPartDef");
            internal static TacticalItemDef slugMechArms = DefCache.GetDef<TacticalItemDef>("NJ_Technician_MechArms_ALN_WeaponDef");

            internal static List<TacticalItemDef> slugArmor = new List<TacticalItemDef>() { slugHelmet, slugLegs, slugTorso, slugMechArms };

            internal static TacticalItemDef doomLegs = DefCache.GetDef<TacticalItemDef>("PX_Heavy_Legs_Headhunter_ItemDef");
            internal static TacticalItemDef doomTorso = DefCache.GetDef<TacticalItemDef>("PX_Heavy_Torso_Headhunter_BodyPartDef");
            internal static TacticalItemDef doomJetpack = DefCache.GetDef<TacticalItemDef>("PX_Heavy_Torso_JumpPack_Headhunter_BodyPartDef");
            internal static TacticalItemDef doomHelmet = DefCache.GetDef<TacticalItemDef>("PX_Heavy_Helmet_Headhunter_BodyPartDef");

            internal static List<TacticalItemDef> doomArmor = new List<TacticalItemDef>() { doomHelmet, doomTorso, doomLegs };

            internal static WeaponDef spyMasterXbow = DefCache.GetDef<WeaponDef>("SY_Crossbow_Bonus_WeaponDef");
            internal static WeaponDef ghostSniperRifle = DefCache.GetDef<WeaponDef>("NE_SniperRifle_WeaponDef");
            internal static WeaponDef slugPistol = DefCache.GetDef<WeaponDef>("NE_Pistol_WeaponDef");
            internal static WeaponDef exileAssaultRifle = DefCache.GetDef<WeaponDef>("NE_AssaultRifle_WeaponDef");
            internal static WeaponDef doomAC = DefCache.GetDef<WeaponDef>("PX_HeavyCannon_Headhunter_WeaponDef");
            internal static WeaponDef sectarianAxe = DefCache.GetDef<WeaponDef>("AN_Blade_Viking_WeaponDef");



            //PX_AssaultRifle_Gold_WeaponDef
            //PX_SniperRifle_Gold_WeaponDef
            //

            [HarmonyPatch(typeof(UIModuleBionics), "OnNewCharacter")]//InitCharacterInfo")]
            public static class UIModuleBionics_InitCharacterInfo_Patch
            {
                public static void Postfix(UIModuleBionics __instance, Dictionary<AddonSlotDef, UIModuleMutationSection> ____augmentSections, GeoCharacter newCharacter)
                {
                    try
                    {

                        //  TFTVLogger.Always($"current character is {newCharacter.DisplayName} and it has mercenary tag? {newCharacter.TemplateDef.GetGameTags().Contains(_mercenaryTag)}");

                        if (newCharacter.TemplateDef.GetGameTags().Contains(MercenaryTag))
                        {

                            foreach (KeyValuePair<AddonSlotDef, UIModuleMutationSection> augmentSection in ____augmentSections)
                            {
                                augmentSection.Value.ResetContainer(AugumentSlotState.BlockedByPermenantAugument, "KEY_ABILITY_NOAUGMENTATONS");
                            }


                        }


                    }
                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                    }
                }
            }


            [HarmonyPatch(typeof(GeoUnitDescriptor), "FinishInitCharacter")]
            public static class GeoUnitDescriptor_FinishInitCharacter_patch
            {
                public static void Postfix(GeoUnitDescriptor __instance, GeoCharacter character)
                {
                    try
                    {
                        AdjustmentsToMercernariesOnHire(character, __instance);


                    }
                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                        throw;
                    }
                }
            }

            private static void AdjustmentsToMercernariesOnHire(GeoCharacter character, GeoUnitDescriptor geoUnitDescriptor)
            {
                try
                {
                    if (character.GameTags.Contains(MercenaryTag))
                    {
                        AdjustMercenaryProficiencyPerks(character, geoUnitDescriptor);

                        if (!character.GameTags.Contains(berserkerTag))
                        {
                            GiveAmmoToMercenaryOnCreation(character);
                        }
                    }


                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }



            }

            private static void AdjustMercenaryProficiencyPerks(GeoCharacter character, GeoUnitDescriptor geoUnitDescriptor)
            {
                try
                {
                    PassiveModifierAbilityDef handGunsProf = DefCache.GetDef<PassiveModifierAbilityDef>("HandgunsTalent_AbilityDef");
                    PassiveModifierAbilityDef heavyWepProf = DefCache.GetDef<PassiveModifierAbilityDef>("HeavyWeaponsTalent_AbilityDef");
                    PassiveModifierAbilityDef meleeProf = DefCache.GetDef<PassiveModifierAbilityDef>("MeleeWeaponTalent_AbilityDef");
                    PassiveModifierAbilityDef pDWProf = DefCache.GetDef<PassiveModifierAbilityDef>("PDWTalent_AbilityDef");
                    PassiveModifierAbilityDef shotgunProf = DefCache.GetDef<PassiveModifierAbilityDef>("ShotgunTalent_AbilityDef");
                    PassiveModifierAbilityDef sniperProf = DefCache.GetDef<PassiveModifierAbilityDef>("SniperTalent_AbilityDef");
                    PassiveModifierAbilityDef assaultRiflesProf = DefCache.GetDef<PassiveModifierAbilityDef>("AssaultRiflesTalent_AbilityDef");

                    List<PassiveModifierAbilityDef> proficiencies = new List<PassiveModifierAbilityDef>()
                                { handGunsProf, heavyWepProf, meleeProf, pDWProf, shotgunProf, sniperProf, assaultRiflesProf};


                    if (character.GameTags.Contains(priestTag))
                    {
                        character.Progression.AddAbility(sniperProf);

                        if (geoUnitDescriptor.Progression.PersonalAbilities[3] == DefCache.GetDef<PassiveModifierAbilityDef>("SniperTalent_AbilityDef"))
                        {
                            proficiencies.Remove(sniperProf);
                            geoUnitDescriptor.Progression.PersonalAbilities[3] = proficiencies.GetRandomElement();
                        }
                    }
                    else if (character.GameTags.Contains(technicianTag))
                    {
                        character.Progression.AddAbility(handGunsProf);

                        if (geoUnitDescriptor.Progression.PersonalAbilities[3] == DefCache.GetDef<PassiveModifierAbilityDef>("SniperTalent_AbilityDef"))
                        {
                            proficiencies.Remove(handGunsProf);
                            proficiencies.Remove(pDWProf);
                            geoUnitDescriptor.Progression.PersonalAbilities[3] = proficiencies.GetRandomElement();
                        }
                    }
                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }


            public static void GiveAmmoToMercenaryOnCreation(GeoCharacter character)
            {

                try
                {
                    ItemDef weapon = character.TemplateDef.Data.EquipmentItems[0];
                    ItemDef ammo = character.TemplateDef.Data.EquipmentItems[0].CompatibleAmmunition[0];

                    List<GeoItem> inventoryList = new List<GeoItem>();

                    if (character.ClassTag.Equals(infiltratorTag))
                    {
                        inventoryList = new List<GeoItem>() {
                                new GeoItem(new ItemUnit { ItemDef = ammo, Quantity = 1 }),
                             new GeoItem(new ItemUnit { ItemDef = ammo, Quantity = 1 })};
                    }

                    List<GeoItem> equipmentList = new List<GeoItem>() {
                            new GeoItem(new ItemUnit { ItemDef = weapon, Quantity = 1 }),
                            new GeoItem(new ItemUnit { ItemDef = ammo, Quantity = 1 }),
                            new GeoItem(new ItemUnit { ItemDef = ammo, Quantity = 1 })};

                    character.SetItems(null, equipmentList, inventoryList, true);

                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }

            private static TacCharacterDef CreateTacCharaterDef(ClassTagDef classTagDef, string name, string gUID,
            WeaponDef weaponDef, List<TacticalItemDef> armorSlots, List<GameTagDef> tags, int level, int[] stats)
            {
                try
                {
                    //  GeoUnitDescriptor

                    TacCharacterDef characterSource = DefCache.GetDef<TacCharacterDef>("AN_Assault1_CharacterTemplateDef");
                    TacCharacterDef newCharacter = Helper.CreateDefFromClone(characterSource, gUID, name);

                    newCharacter.Data.Name = name;
                    newCharacter.Data.GameTags = tags != null ? new List<GameTagDef>(tags) { classTagDef }.ToArray() : new List<GameTagDef>() { classTagDef }.ToArray();
                    newCharacter.Data.EquipmentItems = new ItemDef[] { weaponDef };
                    newCharacter.Data.BodypartItems = armorSlots?.ToArray() ?? new ItemDef[] { };
                    newCharacter.Data.LevelProgression.SetLevel(level);

                    if (stats != null)
                    {
                        newCharacter.Data.Strength = stats[0];
                        newCharacter.Data.Will = stats[1];
                        newCharacter.Data.Speed = stats[2];
                    }

                    return newCharacter;

                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }

            public static void CreateMercenaries()
            {
                try
                {
                    MakeSlugArmorNonRemovable();
                    CreateExpendableArchetypes();
                    AdjustMercenaryArmorsAndWeapons();
                    // CreateNoAugAbility();


                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }



            }

            private static void AdjustMercenaryArmorsAndWeapons()
            {
                try
                {
                    //From Belial's spreadsheet 

                    doomHelmet.Armor = 23;
                    doomTorso.Armor = 24;
                    doomLegs.Armor = 22;

                    doomHelmet.BodyPartAspectDef.Accuracy = -0.06f;
                    doomHelmet.BodyPartAspectDef.Perception = 0.0f;

                    doomTorso.BodyPartAspectDef.Accuracy = -0.04f;

                    doomLegs.BodyPartAspectDef.Speed = 0.0f;
                    doomLegs.BodyPartAspectDef.Accuracy = -0.04f;

                    sectarianHelmet.Armor = 14;
                    sectarianHelmet.Weight = 1;
                    sectarianHelmet.BodyPartAspectDef.WillPower = 1f;
                    sectarianHelmet.BodyPartAspectDef.Perception = -5f;
                    sectarianHelmet.BodyPartAspectDef.Stealth = -0.05f;

                    sectarianTorso.Armor = 18;
                    sectarianTorso.Weight = 3;
                    sectarianTorso.BodyPartAspectDef.Endurance = 2;
                    sectarianTorso.BodyPartAspectDef.Speed = 0f;
                    sectarianTorso.BodyPartAspectDef.Stealth = -0.1f;
                    sectarianTorso.BodyPartAspectDef.Accuracy = -0.05f;

                    sectarianLegs.Armor = 14;
                    sectarianLegs.Weight = 2;
                    sectarianLegs.BodyPartAspectDef.Endurance = 1;
                    sectarianLegs.BodyPartAspectDef.Speed = 1;
                    sectarianLegs.BodyPartAspectDef.Stealth = -0.1f;
                    sectarianLegs.BodyPartAspectDef.Accuracy = -0.05f;


                    ghostHelmet.Armor = 14;
                    ghostHelmet.BodyPartAspectDef.Stealth = 0.05f;

                    ghostTorso.Armor = 16;
                    ghostTorso.BodyPartAspectDef.Stealth = 0.1f;

                    ghostLegs.Armor = 14;
                    ghostLegs.BodyPartAspectDef.Stealth = 0.05f;

                    spyMasterHelmet.Weight = 1;
                    spyMasterHelmet.BodyPartAspectDef.Perception = 4;

                    spyMasterTorso.Weight = 2;
                    spyMasterTorso.BodyPartAspectDef.Speed = -1;

                    spyMasterLegs.Weight = 2;
                    spyMasterLegs.BodyPartAspectDef.Speed = -1;


                    slugHelmet.Tags.Add(MercenaryTag);
                    slugHelmet.Tags.Add(Shared.SharedGameTags.BionicalTag);
                    slugHelmet.Armor = 20;
                    slugHelmet.Weight = 0;
                    slugHelmet.BodyPartAspectDef.Endurance = 1;
                    slugHelmet.BodyPartAspectDef.Accuracy = 0;
                    slugTorso.BodyPartAspectDef.Stealth = 0;

                    slugTorso.Tags.Add(MercenaryTag);
                    slugTorso.Tags.Add(Shared.SharedGameTags.BionicalTag);

                    slugTorso.Armor = 20;
                    slugTorso.Weight = 0;
                    slugTorso.BodyPartAspectDef.Endurance = 1;
                    slugTorso.BodyPartAspectDef.Speed = 0;
                    slugTorso.BodyPartAspectDef.Accuracy = 0;
                    slugTorso.BodyPartAspectDef.Stealth = -0.1f;

                    slugLegs.Tags.Add(MercenaryTag);
                    slugLegs.Tags.Add(Shared.SharedGameTags.BionicalTag);

                    slugMechArms.Tags.Add(MercenaryTag);
                    slugMechArms.Tags.Add(Shared.SharedGameTags.BionicalTag);

                    slugLegs.Armor = 20;
                    slugLegs.Weight = 0;
                    slugLegs.BodyPartAspectDef.Endurance = 1;
                    slugLegs.BodyPartAspectDef.Speed = 0;
                    slugLegs.BodyPartAspectDef.Accuracy = 0;
                    slugLegs.BodyPartAspectDef.Stealth = -0.05f;


                    doomAC.CompatibleAmmunition = new TacticalItemDef[] { DefCache.GetDef<TacticalItemDef>("FS_Autocannon_AmmoClip_ItemDef") };
                    doomAC.DamagePayload.DamageKeywords = new List<PhoenixPoint.Tactical.Entities.DamageKeywords.DamageKeywordPair>()
                    {
                    new PhoenixPoint.Tactical.Entities.DamageKeywords.DamageKeywordPair()
                    {
                    DamageKeywordDef=Shared.SharedDamageKeywords.DamageKeyword, Value = 90

                    },
                    new PhoenixPoint.Tactical.Entities.DamageKeywords.DamageKeywordPair()
                    {
                    DamageKeywordDef=Shared.SharedDamageKeywords.ShockKeyword, Value = 120
                    }
                    };
                    doomAC.ChargesMax = 12;
                    doomAC.DamagePayload.AutoFireShotCount= 2;

                    sectarianAxe.DamagePayload.DamageKeywords = new List<PhoenixPoint.Tactical.Entities.DamageKeywords.DamageKeywordPair>()
                    {
                    new PhoenixPoint.Tactical.Entities.DamageKeywords.DamageKeywordPair()
                    {
                    DamageKeywordDef=Shared.SharedDamageKeywords.DamageKeyword, Value = 160

                    },
                    new PhoenixPoint.Tactical.Entities.DamageKeywords.DamageKeywordPair()
                    {
                    DamageKeywordDef=Shared.SharedDamageKeywords.PiercingKeyword, Value = 20
                    },
                     new PhoenixPoint.Tactical.Entities.DamageKeywords.DamageKeywordPair()
                    {
                    DamageKeywordDef=Shared.SharedDamageKeywords.BleedingKeyword, Value = 20
                    }
                    };

                }

                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }

            private static void MakeSlugArmorNonRemovable()
            {
                try
                {
                    foreach (TacticalItemDef itemDef in slugArmor)
                    {

                        itemDef.IsPermanentAugment = true;

                    }

                }

                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }

            }


            private static void CreateExpendableArchetypes()
            {

                try
                {


                    GameTagDef mercenaryTag = TFTVCommonMethods.CreateNewTag("Mercenary", "{49BDADBC-A411-48B2-8773-533EE9247F4C}");
                    MercenaryTag = mercenaryTag;

                    TacCharacterDef ghost = CreateTacCharaterDef(priestTag, "Mercenary_Ghost", "{05C7ED24-1300-4336-94FB-82AE09CC45AF}",
                        ghostSniperRifle, ghostArmor, new List<GameTagDef>() { mercenaryTag }, 1, null);

                    CreateMarketPlaceRecruit(ghost.name, 
                        "{FA72C430-158D-4F44-99B4-08AF9BF2493F}", "{F2BBE15C-54D1-44D0-9299-D10E56E7314F}", 
                        "{D01434F1-4A5E-4354-8272-58CF2CC1C41C}", "{65ACF823-241A-4EF5-890E-51F88FF0F6C6}", 
                        "KEY_EXPENDABLE_ARCHETYPE_GHOST_NAME", "KEY_EXPENDABLE_ARCHETYPE_GHOST_DESCRIPTION", "KEY_EXPENDABLE_ARCHETYPE_GHOST_QUOTE", 
                        ghost, 500, Helper.CreateSpriteFromImageFile("MERCENARY_GHOST.png"), 4);

                    TacCharacterDef doom = CreateTacCharaterDef(heavyTag, "Mercenary_Heavy", "{96628AFA-B8EF-4350-B451-72B24593993B}",
                        doomAC, doomArmor, new List<GameTagDef> { mercenaryTag }, 1, null);

                    CreateMarketPlaceRecruit(doom.name, "{4FC1981D-C5B5-40E2-83D7-238486503215}", "{546E79A5-FFBE-45A2-852E-9D83E41FFA61}", 
                        "{93FA4108-B4B3-45BA-9764-6069D1705228}", "{1ED716FA-EE9C-43C3-B68E-C851DB31BADF}",
                         "KEY_EXPENDABLE_ARCHETYPE_DOOM_NAME", "KEY_EXPENDABLE_ARCHETYPE_DOOM_DESCRIPTION", "KEY_EXPENDABLE_ARCHETYPE_DOOM_QUOTE", 
                         doom, 500, Helper.CreateSpriteFromImageFile("MERCENARY_DOOM.png"), 0);

                    TacCharacterDef slug =
                        CreateTacCharaterDef(technicianTag, "Mercenary_Slug", "{BFB4540F-CE02-4934-ACDC-FF2CC5B02DA9}",
                        slugPistol, slugArmor, new List<GameTagDef>() { mercenaryTag }, 1, null);

                    CreateMarketPlaceRecruit(slug.name, "{A8CBE9E4-7EA4-4AA9-93C0-09165C121F1F}", "{FC92AA97-F85A-46BD-9168-985578BF44B2}", 
                        "{66D149CB-8ADA-46B5-BEAA-102C18B1F83D}", "{21084D35-84CE-4AD1-AA5A-70EF27C1A247}",
                        "KEY_EXPENDABLE_ARCHETYPE_SLUG_NAME", "KEY_EXPENDABLE_ARCHETYPE_SLUG_DESCRIPTION", "KEY_EXPENDABLE_ARCHETYPE_SLUG_QUOTE", 
                        slug, 500, Helper.CreateSpriteFromImageFile("MERCENARY_SLUG.png"), 4);

                    TacCharacterDef spyMaster =
                         CreateTacCharaterDef(infiltratorTag, "Mercenary_Spymaster", "{BFB2B1E0-FA98-450E-83C0-F16EA953E7EB}",
                         spyMasterXbow, spyMasterArmor, new List<GameTagDef>() { mercenaryTag }, 1, null);

                    CreateMarketPlaceRecruit(spyMaster.name, "{BD808894-2F9C-4490-ABF3-7EC8B3815589}", "{7316428E-394A-424D-92B9-1DF621B4AAC9}", 
                        "{2FAD7A35-3444-4606-8951-6DB8A4BEA26E}", "{C145BEC5-CBCE-49C5-8BEA-ADDED4936E40}",
                       "KEY_EXPENDABLE_ARCHETYPE_SPYMASTER_NAME", "KEY_EXPENDABLE_ARCHETYPE_SPYMASTER_DESCRIPTION", "KEY_EXPENDABLE_ARCHETYPE_SPYMASTER_QUOTE", 
                       spyMaster, 500, Helper.CreateSpriteFromImageFile("MERCENARY_SPYMASTER.png"),3);

                    TacCharacterDef sectarian =
                        CreateTacCharaterDef(berserkerTag, "Mercenary_Sectarian", "{52C42AFC-F1A8-43FB-B1E1-DF1D68D71A7A}",
                        sectarianAxe, sectarianArmor, new List<GameTagDef>() { mercenaryTag }, 1, null);

                    CreateMarketPlaceRecruit(sectarian.name, "{BE81203F-F0C7-4C42-A556-09DDE55ED15F}", "{CDA90EDA-9FA7-4C68-A593-DBD7140D6820}", 
                        "{76EACAB3-3F2E-4A9C-AB3B-A6AEFDAB817D}", "{141EBDFD-7712-4357-8AEF-176F1C7DBD23}",
                       "KEY_EXPENDABLE_ARCHETYPE_SECTARIAN_NAME", "KEY_EXPENDABLE_ARCHETYPE_SECTARIAN_DESCRIPTION", "KEY_EXPENDABLE_ARCHETYPE_SECTARIAN_QUOTE", 
                       sectarian, 500, Helper.CreateSpriteFromImageFile("MERCENARY_SECTARIAN.png"), 0);

                    TacCharacterDef exile =
                       CreateTacCharaterDef(assaultTag, "Mercenary_Exile", "{3FBC2BB0-0235-41C7-BB28-6848A74858AB}",
                       exileAssaultRifle, exileArmor, new List<GameTagDef>() { mercenaryTag }, 1, null);

                    CreateMarketPlaceRecruit(exile.name, "{46D893B9-9DC7-4068-8348-6F66FBFF0AF7}", "{E93DFF70-669E-4699-B005-6A7F4FD42706}", 
                        "{00F16431-56A8-418E-9E28-C1F55B3A7AF7}", "{241F3A70-43B2-4771-87A8-06F735F8C8F5}",
                       "KEY_EXPENDABLE_ARCHETYPE_EXILE_NAME", "KEY_EXPENDABLE_ARCHETYPE_EXILE_DESCRIPTION", "KEY_EXPENDABLE_ARCHETYPE_EXILE_QUOTE", 
                       exile, 500, Helper.CreateSpriteFromImageFile("MERCENARY_EXILE.png"),0);



                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                }
            }

            private static void CreateMarketPlaceRecruit(string name, string gUID, string gUID2, string gUID3, string gUID4, string keyTitle, string keyDescription, string keyQuote, TacCharacterDef tacCharacterDef, int price, Sprite icon, int availability)
            {
                try
                {

                    GeoMarketplaceItemOptionDef sourceItemOption = DefCache.GetDef<GeoMarketplaceItemOptionDef>("KasoBuggy_MarketplaceItemOptionDef");
                    GeoMarketplaceItemOptionDef newOption = Helper.CreateDefFromClone(sourceItemOption, gUID, name);
                    GroundVehicleItemDef sourceVehicleItemDef = DefCache.GetDef<GroundVehicleItemDef>("KS_Kaos_Buggy_ItemDef");

                    GroundVehicleItemDef vehicleItemDef = Helper.CreateDefFromClone(sourceVehicleItemDef, gUID2, $"{name}_VehicleItemDef");
                    vehicleItemDef.ViewElementDef = Helper.CreateDefFromClone(sourceVehicleItemDef.ViewElementDef, gUID3, name);
                    vehicleItemDef.ViewElementDef.DisplayName1.LocalizationKey = keyTitle;
                    vehicleItemDef.ViewElementDef.Category.LocalizationKey = keyQuote;
                    vehicleItemDef.ViewElementDef.Description.LocalizationKey = keyDescription;
                    vehicleItemDef.DataDef = Helper.CreateDefFromClone(vehicleItemDef.DataDef, gUID4, name);
                    vehicleItemDef.Tags.Add(MercenaryTag);

                    vehicleItemDef.ViewElementDef.InventoryIcon = icon;

                    vehicleItemDef.VehicleTemplateDef = tacCharacterDef;

                    newOption.ItemDef = vehicleItemDef;
                    tacCharacterDef.Data.ViewElementDef = vehicleItemDef.ViewElementDef;
                    tacCharacterDef.ItemDef = vehicleItemDef;
                    newOption.MinPrice = price - price / 10;
                    newOption.MaxPrice = price + price / 10;
                    newOption.Availability = availability;

                    TheMarketplaceSettingsDef marketplaceSettings = DefCache.GetDef<TheMarketplaceSettingsDef>("TheMarketplaceSettingsDef");
                    List<GeoMarketplaceOptionDef> geoMarketplaceItemOptionDefs = marketplaceSettings.PossibleOptions.ToList();
                    geoMarketplaceItemOptionDefs.Add(newOption);
                    marketplaceSettings.PossibleOptions = geoMarketplaceItemOptionDefs.ToArray();

                    // TFTVLogger.Always($"{name}null? {DefCache.GetDef<GroundVehicleItemDef>($"{name}_VehicleItemDef") == null}");

                }

                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                }


            }
        }

        internal class TFTVKaosGuns
        {

            private static readonly WeaponDef _obliterator = DefCache.GetDef<WeaponDef>("KS_Obliterator_WeaponDef");
            private static readonly WeaponDef _subjector = DefCache.GetDef<WeaponDef>("KS_Subjector_WeaponDef");
            private static readonly WeaponDef _redemptor = DefCache.GetDef<WeaponDef>("KS_Redemptor_WeaponDef");
            private static readonly WeaponDef _devastator = DefCache.GetDef<WeaponDef>("KS_Devastator_WeaponDef");
            private static readonly WeaponDef _tormentor = DefCache.GetDef<WeaponDef>("KS_Tormentor_WeaponDef");

            internal static Dictionary<GeoMarketplaceItemOptionDef, GeoMarketplaceItemOptionDef> _kGWeaponsAndAmmo = new Dictionary<GeoMarketplaceItemOptionDef, GeoMarketplaceItemOptionDef>();
            internal static GameTagDef _kGTag;

            private static void CreateAmmoForKG(WeaponDef weaponDef, int amount, int minPrice, string gUID0, string gUID1, string gUID2, string spriteFileName)
            {
                try
                {
                    TacticalItemDef sourceAmmo = DefCache.GetDef<TacticalItemDef>("PX_AssaultRifle_AmmoClip_ItemDef");
                    string name = $"{weaponDef.name}_AmmoClipDef";

                    ClassTagDef classTagDef = weaponDef.Tags.FirstOrDefault<ClassTagDef>();

                    TacticalItemDef newAmmo = Helper.CreateDefFromClone(sourceAmmo, gUID0, name);
                    newAmmo.ViewElementDef = Helper.CreateDefFromClone(sourceAmmo.ViewElementDef, gUID1, name);
                    newAmmo.ViewElementDef.DisplayName1.LocalizationKey = $"KEY_KAOSGUNS_AMMO_{weaponDef.name}";
                    newAmmo.ViewElementDef.Description.LocalizationKey = $"KEY_KAOSGUNS_AMMO_DESCRIPTION_{weaponDef.name}";
                    newAmmo.ViewElementDef.InventoryIcon = Helper.CreateSpriteFromImageFile(spriteFileName);


                    newAmmo.ChargesMax = amount;
                    newAmmo.CrateSpawnWeight = 1000;
                    newAmmo.Tags.Remove(DefCache.GetDef<GameTagDef>("ManufacturableItem_TagDef"));
                    newAmmo.Tags.Remove(DefCache.GetDef<ClassTagDef>("Assault_ClassTagDef"));
                    newAmmo.Tags.Add(classTagDef);
                    //  newAmmo.CombineWhenStacking = false;
                    newAmmo.ManufactureTech = 0;
                    newAmmo.ManufactureMaterials = minPrice;
                    weaponDef.ChargesMax = amount;
                    weaponDef.CompatibleAmmunition = new TacticalItemDef[] { newAmmo };

                    weaponDef.Tags.Add(_kGTag);

                    GeoMarketplaceItemOptionDef newMarketplaceItem = Helper.CreateDefFromClone
                         (DefCache.GetDef<GeoMarketplaceItemOptionDef>("Obliterator_MarketplaceItemOptionDef"), gUID2, name);

                    newMarketplaceItem.MinPrice = minPrice;
                    newMarketplaceItem.MaxPrice = minPrice + minPrice * 1.25f;
                    newMarketplaceItem.ItemDef = newAmmo;
                    newMarketplaceItem.DisallowDuplicates = false;


                    TheMarketplaceSettingsDef marketplaceSettings = DefCache.GetDef<TheMarketplaceSettingsDef>("TheMarketplaceSettingsDef");

                    List<GeoMarketplaceOptionDef> geoMarketplaceItemOptionDefs = marketplaceSettings.PossibleOptions.ToList();

                    geoMarketplaceItemOptionDefs.Add(newMarketplaceItem);


                    marketplaceSettings.PossibleOptions = geoMarketplaceItemOptionDefs.ToArray();

                    weaponDef.WeaponMalfunction = DefCache.GetDef<WeaponDef>("PX_AssaultRifle_WeaponDef").WeaponMalfunction;


                    AmmoWeaponDatabase.AmmoToWeaponDictionary.Add(newAmmo, new List<TacticalItemDef>() { weaponDef });
                    GeoMarketplaceItemOptionDef weaponMarketPlaceOption = (GeoMarketplaceItemOptionDef)geoMarketplaceItemOptionDefs.Find(o => o is GeoMarketplaceItemOptionDef marketOption && marketOption.ItemDef == weaponDef);

                    _kGWeaponsAndAmmo.Add(weaponMarketPlaceOption, newMarketplaceItem);

                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                }



            }

            public static void CreateKaosWeaponAmmo()
            {
                try
                {


                    _obliterator.ManufactureMaterials = 100;
                    _subjector.ManufactureMaterials = 100;
                    _redemptor.ManufactureMaterials = 100;
                    _devastator.ManufactureMaterials = 100;
                    _tormentor.ManufactureMaterials = 100;

                    _kGTag = TFTVCommonMethods.CreateNewTag("KaosGun", "{2DA3F33A-8D39-4DA6-8BA5-38C3114A21F7}");

                    //KEY_KAOSGUNS_AMMO_
                    //KEY_KAOSGUNS_AMMO_DESCRIPTION_

                    CreateAmmoForKG(_tormentor, 8, 30, "e1875c26-0494-4d0f-9e5d-3c74a17c3b2d",
                    "79f6bb60-8ca3-4bbf-a0f1-c819f5ebf09e",
                    "ee89b5c3-6d06-4c5e-856b-96e7ff411c77", "KG_Pistol_Ammo.png");
                    CreateAmmoForKG(_subjector, 5, 30, "2e5be682-1f85-4610-bbb7-c2f2bf41d4c6",
                    "b03d78d4-c7e7-49c3-b097-3448e253a1e7",
                    "70a0a172-2b57-48d3-94c2-7cb4e428c3c4", "KG_Sniper_Ammo.png");
                    CreateAmmoForKG(_redemptor, 24, 30, "8f7ff5ca-4b8d-4677-86d3-7f21e41a3a70",
                    "d60e04a0-c873-4c16-9a83-2f9d6e1c163d",
                    "dc92d8ca-1b8d-4f85-9d90-d8eb9e63d5a3", "KG_Shotgun_Ammo.png");
                    CreateAmmoForKG(_devastator, 6, 30, "99aa40e5-5415-44b9-98ed-34d746a99b52",
                    "3b647fa3-1e06-4f2a-9d1c-82edf8a6dbff",
                    "605d3c8a-7b9c-481a-8c0d-7ff4be94901a", "KG_Cannon_Ammo.png");
                    CreateAmmoForKG(_obliterator, 32, 30, "2c86774f-4889-4c06-9f7a-8971e62ff267",
                    "587b1a5b-1665-48c9-8b9c-4156231712c1",
                    "1a1230fc-0e5d-4c4c-9be5-563879d2471f", "KG_Assault_Rifle_Ammo.png");


                }


                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                }
            }
        }

        internal class TFTVMarketPlaceItems
        {
            public static void AdjustMarketPlaceOptions()
            {
                try
                {
                    TheMarketplaceSettingsDef marketplaceSettings = DefCache.GetDef<TheMarketplaceSettingsDef>("TheMarketplaceSettingsDef");

                    List<GeoMarketplaceOptionDef> geoMarketplaceOptionDefs = new List<GeoMarketplaceOptionDef>(marketplaceSettings.PossibleOptions.ToList());

                    geoMarketplaceOptionDefs.Remove(DefCache.GetDef<GeoMarketplaceResearchOptionDef>("Random_MarketplaceResearchOptionDef"));

                    marketplaceSettings.PossibleOptions = geoMarketplaceOptionDefs.ToArray();

                    DefCache.GetDef<GeoMarketplaceOptionDef>("Redemptor_MarketplaceItemOptionDef").Availability = 3;
                    DefCache.GetDef<GeoMarketplaceOptionDef>("Subjector_MarketplaceItemOptionDef").Availability =1;
                    DefCache.GetDef<GeoMarketplaceOptionDef>("Tormentor_MarketplaceItemOptionDef").Availability =1;
                    DefCache.GetDef<GeoMarketplaceOptionDef>("Devastator_Redemptor_MarketplaceItemOptionDef").Availability = 2;
                /*    DefCache.GetDef<GeoMarketplaceOptionDef>("");
                    DefCache.GetDef<GeoMarketplaceOptionDef>("");
                    DefCache.GetDef<GeoMarketplaceOptionDef>("");
                    DefCache.GetDef<GeoMarketplaceOptionDef>("");
                    DefCache.GetDef<GeoMarketplaceOptionDef>("");*/

                    /*
                     * 
                     
[TFTV @ 11/9/2023 12:00:24 PM] AdvancedEngineMappingModule_MarketplaceItemOptionDef 1
[TFTV @ 11/9/2023 12:00:24 PM] Apollo_MarketplaceItemOptionDef 3
[TFTV @ 11/9/2023 12:00:24 PM] ArmadilloSuperchargerTechnology_MarketplaceItemOptionDef 2
[TFTV @ 11/9/2023 12:00:24 PM] Bi-TurboEngineUpgrade_MarketplaceItemOptionDef 2
[TFTV @ 11/9/2023 12:00:24 PM] CarbonFiberPlating_MarketplaceItemOptionDef 1
[TFTV @ 11/9/2023 12:00:24 PM] ExperimentalArmadilloTechnology_MarketplaceResearchOptionDef 2
[TFTV @ 11/9/2023 12:00:24 PM] ExperimentalExhaustSystem_MarketplaceItemOptionDef 4
[TFTV @ 11/9/2023 12:00:24 PM] ExperimentalThrustersTechnology_MarketplaceItemOptionDef 3
[TFTV @ 11/9/2023 12:00:24 PM] HybridEngineTechnology_MarketplaceItemOptionDef 3
[TFTV @ 11/9/2023 12:00:24 PM] ImprovedChassis_MarketplaceItemOptionDef 3
[TFTV @ 11/9/2023 12:00:24 PM] JetBoosters_MarketplaceItemOptionDef 4
[TFTV @ 11/9/2023 12:00:24 PM] LightweightAlloyPlating_MarketplaceItemOptionDef 2
[TFTV @ 11/9/2023 12:00:24 PM] Maphistopheles_MarketplaceItemOptionDef 2
[TFTV @ 11/9/2023 12:00:24 PM] PsychicJammer_MarketplaceItemOptionDef 3
[TFTV @ 11/9/2023 12:00:24 PM] Purgatory_MarketplaceItemOptionDef 2
[TFTV @ 11/9/2023 12:00:24 PM] ReinforcedCargoRacks_MarketplaceItemOptionDef 1
[TFTV @ 11/9/2023 12:00:24 PM] ReinforcedCaterpillarTracks_MarketplaceItemOptionDef 1
[TFTV @ 11/9/2023 12:00:24 PM] ReinforcedPlating_MarketplaceItemOptionDef 2
[TFTV @ 11/9/2023 12:00:24 PM] RevisedArmorPlating_MarketplaceItemOptionDef 4
[TFTV @ 11/9/2023 12:00:24 PM] Scorpio_MarketplaceItemOptionDef 1
[TFTV @ 11/9/2023 12:00:24 PM] SpikedArmorPlating_MarketplaceItemOptionDef 4
[TFTV @ 11/9/2023 12:00:24 PM] Taurus_MarketplaceItemOptionDef 1
[TFTV @ 11/9/2023 12:00:24 PM] TheFullstop_MarketplaceItemOptionDef 4
[TFTV @ 11/9/2023 12:00:24 PM] Themis_MarketplaceItemOptionDef 3
[TFTV @ 11/9/2023 12:00:24 PM] TheScreamer_MarketplaceItemOptionDef 4

                     */


                /*    foreach (GeoMarketplaceOptionDef geoMarketplaceOptionDef in marketplaceSettings.PossibleOptions) 
                    {
                        TFTVLogger.Always($"{geoMarketplaceOptionDef.name} {geoMarketplaceOptionDef.Availability}");
                    
                    
                    }*/


                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                }
            }




        }

        internal class TFTVMarketPlaceGenerateOffers 
        {
            private static readonly ClassTagDef _vehicle_ClassTagDef = DefCache.GetDef<ClassTagDef>("Vehicle_ClassTagDef");

            private static readonly string _marketPlaceStockRotated = "MarketPlaceRotations";
            private static string _currentMarketPlaceSpecial;
            private static readonly string _vehicleMarketPlaceSpecial = "KEY_MARKETPLACE_SPECIAL_VEHICLES";
            private static readonly string _weaponsMarketPlaceSpecial = "KEY_MARKETPLACE_SPECIAL_WEAPONS";
            private static readonly string _mercenaryMarketPlaceSpecial = "KEY_MARKETPLACE_SPECIAL_MERCENARY";
            private static readonly string _researchMarketPlaceSpecial = "KEY_MARKETPLACE_SPECIAL_RESEARCH";
            private static readonly string[] _marketPlaceSpecials = new string[] { _vehicleMarketPlaceSpecial, _researchMarketPlaceSpecial, _mercenaryMarketPlaceSpecial, _weaponsMarketPlaceSpecial };


            private static GeoEventChoice GenerateResearchChoice(ResearchDef researchDef, float price)
            {
                try
                {
                    GeoEventChoice geoEventChoice = GenerateChoice(price);
                    geoEventChoice.Outcome.GiveResearches.Add(researchDef.Id);
                    geoEventChoice.Text = researchDef.ViewElementDef?.ResearchName;
                    return geoEventChoice;
                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }

            private static GeoEventChoice GenerateItemChoice(ItemDef itemDef, float price)
            {
                try
                {
                    // TFTVLogger.Always($"item def is {itemDef.name}");

                    GeoEventChoice geoEventChoice = GenerateChoice(price);
                    GroundVehicleItemDef groundVehicleItemDef;
                    if ((object)(groundVehicleItemDef = itemDef as GroundVehicleItemDef) != null)
                    {
                        geoEventChoice.Outcome.Units.Add(groundVehicleItemDef.VehicleTemplateDef);
                    }
                    else
                    {
                        geoEventChoice.Outcome.Items.Add(new ItemUnit(itemDef, 1));
                    }

                    geoEventChoice.Text = itemDef.GetDisplayName();
                    return geoEventChoice;
                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }

            private static GeoEventChoice GenerateChoice(float price)
            {
                try
                {
                    GeoEventChoice geoEventChoice = new GeoEventChoice
                    {
                        Requirments = new GeoEventChoiceRequirements(),
                        Outcome = new GeoEventChoiceOutcome()
                    };
                    geoEventChoice.Requirments.Resources.Add(new ResourceUnit(ResourceType.Materials, price));
                    geoEventChoice.Outcome.ReEneableEvent = true;
                    return geoEventChoice;
                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }


            private static List<GeoMarketplaceOptionDef> GetOptionsByType(List<GeoMarketplaceOptionDef> currentlyPossibleOptions, GameTagDef itemTypeTag)
            {
                try
                {
                    return
                        new List<GeoMarketplaceOptionDef>(currentlyPossibleOptions).Where(o => o is GeoMarketplaceItemOptionDef item && item.ItemDef.Tags.Contains(itemTypeTag)).ToList();

                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }

            }

            private static GeoMarketplaceItemOptionDef[] NewJericho_Items()
            {
                GeoMarketplaceItemOptionDef[] Options = new GeoMarketplaceItemOptionDef[]
                {
                (GeoMarketplaceItemOptionDef)Repo.GetDef("a5833903-97b1-71f4-9b7c-b0755e8decf7"), //Purgatory
                (GeoMarketplaceItemOptionDef)Repo.GetDef("03ebb7ca-08d7-36a4-2bf6-851b47682476"), //Lightweight Alloy
                (GeoMarketplaceItemOptionDef)Repo.GetDef("46a57a6d-7163-8ef4-99b3-8167efb46edc"), //Supercharger
                };
                return Options;
            }

            private static GeoMarketplaceItemOptionDef[] Synedrion_Items()
            {
                GeoMarketplaceItemOptionDef[] Options = new GeoMarketplaceItemOptionDef[]
                {
                (GeoMarketplaceItemOptionDef)Repo.GetDef("017b69c2-8a8f-e784-6b36-70cc804ece5d"), //Apollo
                (GeoMarketplaceItemOptionDef)Repo.GetDef("456bf1a1-82ce-2f54-9a0a-27600107d5b4"), //Psychic Jammer
                (GeoMarketplaceItemOptionDef)Repo.GetDef("3e192929-51ba-29e4-7ac1-e9ab2836f076"), //Experimental Thrusters
                };
                return Options;
            }

         
            /// <summary>
            /// When MarketPlace is discovered, 
            /// 
            /// 1) NumberOfDLC5MissionsCompletedVariable is set to 4 (to remove everything connected to DLC5 mission generation).
            /// 
            /// 2) _updateOptionsNextTime is set to now and updateOptionsWithRespectToTime is forcefully run
            ///  
            /// When UpdateOptionsWithRespectToTime is run, it checks whether _updateOptionsNextTime is past now. 
            /// 
            /// If it is, UpdateOptions(Timing) is run. 
            /// </summary>


            [HarmonyPatch(typeof(GeoMarketplace), "UpdateOptionsWithRespectToTime")]
            public static class GeoMarketplace_UpdateOptionsWithRespectToTime_patch
            {
                public static bool Prefix(ref TimeUnit ____updateOptionsNextTime, GeoLevelController ____level, GeoMarketplace __instance)
                {
                    try
                    {
                        TFTVLogger.Always($"UpdateOptionsWithRespectToTime: ____updateOptionsNextTime is {____updateOptionsNextTime.DateTime}, ____level.Timing.Now is {____level.Timing.Now.DateTime} ");

                        if (____level.Timing.Now < ____updateOptionsNextTime)
                        {

                        }
                        else
                        {
                          /*  UnityEngine.Random.InitState((int)Stopwatch.GetTimestamp());
                            int hours = UnityEngine.Random.Range(65, 90);


                            ____updateOptionsNextTime = TimeUtils.GetNextTimeInHours(____level.Timing, hours);*/
                         
                            //TFTVLogger.Always($"After trigger: UpdateOptionsWithRespectToTime: ____updateOptionsNextTime is {____updateOptionsNextTime.DateTime}, ____level.Timing.Now is {____level.Timing.Now.DateTime} ");
                            __instance.UpdateOptions(____level.Timing);


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

            /// <summary>
            /// When UpdateOptions(Timing) is run, if 
            /// 
            /// 1) NumberOfDLC5MissionsCompletedVariable > 0 (that is, if MarketPlace has been explored) and
            /// 2) current time passed _updateOptionsNextTime
            /// 
            /// Then
            /// 
            /// 1) UpdateOptions is run;
            /// 2) _updateOptionsNextTime is to 65-90 hours from now
            /// 3) LogEntry is created
            /// 4) MarketRotation variable is increased by 1
            /// 
            /// </summary>

            [HarmonyPatch(typeof(GeoMarketplace), "UpdateOptions", new Type[] { typeof(Timing) })]
            public static class GeoMarketplace_UpdateOptionsTiming_patch
            {
                public static bool Prefix(ref TimeUnit ____updateOptionsNextTime, GeoLevelController ____level, Timing timing, GeoMarketplace __instance, TheMarketplaceSettingsDef ____settings)
                {
                    try
                    {

                        TFTVLogger.Always($"UpdateOptions(Timing) is called (Prefix) Current time: {____level.Timing.Now.DateTime}. Next update: {____updateOptionsNextTime.DateTime}");

                        if (timing.Now>= ____updateOptionsNextTime && ____level.EventSystem.GetVariable(____settings.NumberOfDLC5MissionsCompletedVariable) > 0)
                        {
                            MethodInfo updateOptionsMethod = typeof(GeoMarketplace).GetMethod("UpdateOptions", BindingFlags.NonPublic | BindingFlags.Instance);
                            updateOptionsMethod.Invoke(__instance, null);
                            UnityEngine.Random.InitState((int)Stopwatch.GetTimestamp());
                            int hours = UnityEngine.Random.Range(65, 90);
                            ____updateOptionsNextTime = TimeUtils.GetNextTimeInHours(____level.Timing, hours);
                           
                            CreateLogEntryAndRollSpecialsMarketplaceUpdated(____level);

                            ____level.EventSystem.SetVariable(_marketPlaceStockRotated, ____level.EventSystem.GetVariable(_marketPlaceStockRotated) + 1);
                            TFTVLogger.Always($"number of stock rotations is {____level.EventSystem.GetVariable(_marketPlaceStockRotated)}");
                        }


                        return false;
                    }
                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                        throw;
                    }
                }

                public static void Postfix(TimeUnit ____updateOptionsNextTime, GeoLevelController ____level)
                {
                    try
                    {
                        TFTVLogger.Always($"UpdateOptions(Timing) Postfix: Current time: {____level.Timing.Now.DateTime}. Next update: {____updateOptionsNextTime.DateTime}");


                    }
                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                        throw;
                    }
                }


            }


            [HarmonyPatch(typeof(GeoMarketplace), "UpdateOptions", new Type[] { })]

            public static class GeoMarketplace_UpdateOptions_MarketPlace_patch
            {
                public static bool Prefix(GeoMarketplace __instance, GeoLevelController ____level, TheMarketplaceSettingsDef ____settings, TimeUnit ____updateOptionsNextTime)
                {
                    try
                    {
                        GenerateMarketPlaceOptionsOnUpdateOptions(__instance, ____level, ____settings, ____updateOptionsNextTime);

                        return false;
                    }
                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                        throw;
                    }
                }
            }

            private static void GenerateMarketPlaceOptionsOnUpdateOptions(GeoMarketplace geoMarketPlace, GeoLevelController controller, TheMarketplaceSettingsDef marketPlaceSettings, TimeUnit updateOptionsNextTime)
            {
                try
                {
                    TFTVLogger.Always($"Updating marketplace options. Current time: {controller.Timing.Now.DateTime}. Next update: {updateOptionsNextTime.DateTime}");

                    if (controller.EventSystem.GetVariable(_marketPlaceStockRotated) > 2)
                    {
                        _currentMarketPlaceSpecial = _marketPlaceSpecials.GetRandomElement();
                    }


                    geoMarketPlace.MarketplaceChoices.Clear();

                    int numberOfStockRotations = controller.EventSystem.GetVariable(_marketPlaceStockRotated);

                    TFTVLogger.Always($"number of stock rotations is {controller.EventSystem.GetVariable(_marketPlaceStockRotated)}");

                    int numberOfOffers = Math.Min(8 + numberOfStockRotations * 4, 40);

                    TFTVLogger.Always($"Number of offers is {numberOfOffers}; divided by 4 {numberOfOffers/4}");

                    List<GeoMarketplaceOptionDef> currentlyPossibleOptions = new List<GeoMarketplaceOptionDef>();

                    foreach (GeoMarketplaceOptionDef geoMarketplaceOptionDef in marketPlaceSettings.PossibleOptions)
                    {
                        UnityEngine.Random.InitState((int)Stopwatch.GetTimestamp());
                        int coinToss = UnityEngine.Random.Range(0, 2);

                        if (geoMarketplaceOptionDef.Availability - coinToss <= numberOfStockRotations)
                        {
                            currentlyPossibleOptions.Add(geoMarketplaceOptionDef);
                        }
                    }

                    currentlyPossibleOptions = CullAvailableOptionsBasedOnExternals(controller, currentlyPossibleOptions);

                    float voPriceMultiplier = TFTVVoidOmens.CheckFordVoidOmensInPlay(controller).Contains(19) ? 0.5f : 1;

                    if (_currentMarketPlaceSpecial != null)
                    {
                        TFTVLogger.Always($"Marketspecial is {_currentMarketPlaceSpecial}");

                        if (_currentMarketPlaceSpecial == _weaponsMarketPlaceSpecial)
                        {
                            TFTVLogger.Always($"Marketspecial is {_currentMarketPlaceSpecial}, so generating more weapon choices");
                            GenerateWeaponChoices(currentlyPossibleOptions, Math.Min(numberOfOffers / 4, 5), geoMarketPlace, voPriceMultiplier * 0.75f);
                        }
                        else if (_currentMarketPlaceSpecial == _vehicleMarketPlaceSpecial)
                        {
                            TFTVLogger.Always($"Marketspecial is {_currentMarketPlaceSpecial}, so generating more vehicle choices");
                            GenerateVehicleChoices(currentlyPossibleOptions, Math.Min(numberOfOffers / 4, 20), geoMarketPlace, voPriceMultiplier * 0.75f);
                        }
                        else if (_currentMarketPlaceSpecial == _mercenaryMarketPlaceSpecial)
                        {
                            TFTVLogger.Always($"Marketspecial is {_currentMarketPlaceSpecial}, so generating more merc choices");
                            GenerateMercenaryChoices(currentlyPossibleOptions, Math.Min(numberOfOffers / 4, 6), geoMarketPlace, voPriceMultiplier * 0.75f);
                        }
                    }
                    GenerateWeaponChoices(currentlyPossibleOptions, Math.Min(numberOfOffers / 4, 5), geoMarketPlace, voPriceMultiplier);
                    GenerateVehicleChoices(currentlyPossibleOptions, Math.Min(numberOfOffers / 4, 10), geoMarketPlace, voPriceMultiplier);
                    GenerateMercenaryChoices(currentlyPossibleOptions, Math.Min(numberOfOffers / 4, 6), geoMarketPlace, voPriceMultiplier);
                    GenerateResearchChoices(currentlyPossibleOptions, Math.Min(numberOfOffers / 4, 8), geoMarketPlace, voPriceMultiplier);
                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }

            private static List<GeoMarketplaceOptionDef> CullAvailableOptionsBasedOnExternals(GeoLevelController controller, List<GeoMarketplaceOptionDef> options)
            {
                try
                {

                    if (controller != null && controller.NewJerichoFaction != null && controller.SynedrionFaction != null)
                    {
                        if (controller.NewJerichoFaction.Research.HasCompleted("NJ_VehicleTech_ResearchDef"))
                        {
                            //If complete, add more options
                            //   num += 3;
                        }
                        else
                        {
                            //Otherwise we remove NJ items from being rolled by GenerateRandomChoiceTFTV
                            options.RemoveRange(NewJericho_Items());
                        }
                        if (controller.SynedrionFaction.Research.HasCompleted("SYN_Rover_ResearchDef"))
                        {
                            // num += 3;
                        }
                        else
                        {
                            options.RemoveRange(Synedrion_Items());
                        }
                    }

                    return options;


                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }

            }

            private static List<GeoEventChoice> GenerateWeaponChoices(List<GeoMarketplaceOptionDef> availableOptions,
                int numberToGenerate, GeoMarketplace geoMarketplace, float priceModifier)
            {
                try
                {
                    List<GeoEventChoice> list = new List<GeoEventChoice>();

                    List<GeoMarketplaceOptionDef> weaponsAvailable = GetOptionsByType(availableOptions, TFTVKaosGuns._kGTag);

                    for (int x = 0; x < numberToGenerate; x++)
                    {
                        if (weaponsAvailable.Count() == 0)
                        {
                            break;
                        }

                        GeoMarketplaceItemOptionDef weaponOffer = (GeoMarketplaceItemOptionDef)weaponsAvailable.GetRandomElement();

                        TFTVLogger.Always($"weaponOffer is {weaponOffer.name}");

                        weaponsAvailable.Remove(weaponOffer);

                        int price = (int)(UnityEngine.Random.Range(weaponOffer.MinPrice, weaponOffer.MaxPrice) * priceModifier);

                        GeoEventChoice item = GenerateItemChoice(weaponOffer.ItemDef, price);
                        GeoMarketplaceItemOptionDef ammoOffer = TFTVKaosGuns._kGWeaponsAndAmmo[weaponOffer];

                        int ammoPrice = (int)(UnityEngine.Random.Range(ammoOffer.MinPrice, ammoOffer.MaxPrice) * priceModifier);

                        List<GeoEventChoice> ammo = new List<GeoEventChoice>()
                    {
                        GenerateItemChoice(ammoOffer.ItemDef, ammoPrice),
                        GenerateItemChoice(ammoOffer.ItemDef, ammoPrice),
                        GenerateItemChoice(ammoOffer.ItemDef, ammoPrice),
                    };

                        geoMarketplace.MarketplaceChoices.Add(item);
                        geoMarketplace.MarketplaceChoices.AddRange(ammo);
                        TFTVLogger.Always($"should have added {weaponOffer.name} and 3 ammo for it");
                    }

                    return list;

                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }

            private static List<GeoEventChoice> GenerateVehicleChoices(List<GeoMarketplaceOptionDef> availableOptions,
                int numberToGenerate, GeoMarketplace geoMarketplace, float priceModifier)
            {
                try
                {
                    List<GeoEventChoice> list = new List<GeoEventChoice>();

                    List<GeoMarketplaceOptionDef> vehicleItemsAvailable = GetOptionsByType(availableOptions, _vehicle_ClassTagDef);

                    for (int x = 0; x < numberToGenerate; x++)
                    {
                        if (vehicleItemsAvailable.Count() == 0)
                        {
                            break;
                        }

                        GeoMarketplaceItemOptionDef vehicleItemToOffer;
                        if (x == 0)
                        {

                            vehicleItemToOffer = DefCache.GetDef<GeoMarketplaceItemOptionDef>("KasoBuggy_MarketplaceItemOptionDef");

                        }
                        else
                        {
                            vehicleItemToOffer = (GeoMarketplaceItemOptionDef)vehicleItemsAvailable.GetRandomElement();
                        }

                        vehicleItemsAvailable.Remove(vehicleItemToOffer);

                        int price = (int)(UnityEngine.Random.Range(vehicleItemToOffer.MinPrice, vehicleItemToOffer.MaxPrice) * priceModifier);

                        GeoEventChoice item = GenerateItemChoice(vehicleItemToOffer.ItemDef, price);

                        geoMarketplace.MarketplaceChoices.Add(item);

                        TFTVLogger.Always($"should have added {vehicleItemToOffer.name}");
                    }

                    return list;

                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }

            private static List<GeoEventChoice> GenerateMercenaryChoices(List<GeoMarketplaceOptionDef> availableOptions,
               int numberToGenerate, GeoMarketplace geoMarketplace, float priceModifier)
            {
                try
                {
                    List<GeoMarketplaceOptionDef> mercernariesAvailable = GetOptionsByType(availableOptions, MercenaryTag);

                    List<GeoEventChoice> list = new List<GeoEventChoice>();

                    for (int x = 0; x < numberToGenerate; x++)
                    {
                        if (mercernariesAvailable.Count() == 0)
                        {
                            break;
                        }

                        GeoMarketplaceItemOptionDef mercenaryToOffer = (GeoMarketplaceItemOptionDef)mercernariesAvailable.GetRandomElement();

                        mercernariesAvailable.Remove(mercenaryToOffer);

                        int price = (int)(UnityEngine.Random.Range(mercenaryToOffer.MinPrice, mercenaryToOffer.MaxPrice) * priceModifier);

                        GeoEventChoice item = GenerateItemChoice(mercenaryToOffer.ItemDef, price);

                        geoMarketplace.MarketplaceChoices.Add(item);
                        TFTVLogger.Always($"should have added {mercenaryToOffer.name}");
                    }

                    return list;

                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }

            private static void GenerateResearchChoices(List<GeoMarketplaceOptionDef> availableOptions,
              int numberToGenerate, GeoMarketplace geoMarketplace, float priceModifier)
            {
                try
                {
                    List<GeoEventChoice> list = new List<GeoEventChoice>();

                    List<GeoMarketplaceOptionDef> researchOptions = availableOptions.Where(o => o is GeoMarketplaceResearchOptionDef).ToList();

                    if (_currentMarketPlaceSpecial != null && _currentMarketPlaceSpecial == _researchMarketPlaceSpecial)
                    {
                        TFTVLogger.Always($"research special!");
                        numberToGenerate = 8;
                        priceModifier *= 0.5f;

                    }

                    if (researchOptions.Count == 0)
                    {
                        return;

                    }


                    for (int x = 0; x < numberToGenerate; x++)
                    {
                        if (researchOptions.Count() == 0)
                        {
                            break;

                        }

                        GeoMarketplaceResearchOptionDef researchToOffer = (GeoMarketplaceResearchOptionDef)researchOptions.GetRandomElement();

                        researchOptions.Remove(researchToOffer);

                        int price = (int)(UnityEngine.Random.Range(researchToOffer.MinPrice, researchToOffer.MaxPrice) * priceModifier);

                        ResearchDef researchDef = researchToOffer.GetResearch();

                        if (researchDef == null)
                        {
                            break;
                        }


                        GeoEventChoice item = GenerateResearchChoice(researchDef, price);

                        geoMarketplace.MarketplaceChoices.Add(item);
                        TFTVLogger.Always($"should have added {researchDef.Id}");
                    }

                    _researchesAlreadyRolled.Clear();

                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }

            private static List<ResearchDef> _researchesAlreadyRolled = new List<ResearchDef>();

            [HarmonyPatch(typeof(GeoMarketplaceResearchOptionDef), "GetRandomResearch")]
            public static class GeoMarketplaceResearchOptionDef_GetRandomResearch_MarketPlace_patch
            {
                public static bool Prefix(ref ResearchDef __result)
                {
                    try
                    {
                        GeoLevelController level = GameUtl.CurrentLevel().GetComponent<GeoLevelController>();
                        List<ResearchElement> list = level.FactionsWithDiplomacy.ElementAt(UnityEngine.Random.Range(0, level.FactionsWithDiplomacy.Count())).Research.Completed.Where((ResearchElement x) => x.IsAvailableToFaction(level.PhoenixFaction)).ToList();
                        List<ResearchElement> phoenixFactionCompletedResearches = level.PhoenixFaction.Research.RevealedAndCompleted.ToList();
                        list.RemoveAll((ResearchElement research) => phoenixFactionCompletedResearches.Any((ResearchElement phoenixResearch) => research.ResearchID == phoenixResearch.ResearchID));

                        TFTVLogger.Always($"_researchesAlreadyRolled has any elements in it? {_researchesAlreadyRolled.Count > 0}");

                        if (_researchesAlreadyRolled.Count > 0)
                        {
                            list.RemoveAll(e => _researchesAlreadyRolled.Contains(e.ResearchDef));
                            TFTVLogger.Always($"removing already rolled researches from pool");
                        }

                        if (list.Count != 0)
                        {
                            __result = list.ElementAt(UnityEngine.Random.Range(0, list.Count)).ResearchDef;
                            _researchesAlreadyRolled.Add(__result);
                        }
                        else
                        {
                            __result = null;
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


            private static void CreateLogEntryAndRollSpecialsMarketplaceUpdated(GeoLevelController controller)
            {
                try
                {


                    string textToDisplay = $"{TFTVCommonMethods.ConvertKeyToString("KEY_MARKETPLACE_NEW_STOCK")} {TFTVCommonMethods.ConvertKeyToString(_currentMarketPlaceSpecial)} ";

                    GeoscapeLogEntry entry = new GeoscapeLogEntry
                    {
                        Text = new LocalizedTextBind(textToDisplay, true)
                    };
                    typeof(GeoscapeLog).GetMethod("AddEntry", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(controller.Log, new object[] { entry, null });
                    controller.View.SetGamePauseState(true);

                    _currentMarketPlaceSpecial = null;
                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }

            }



            [HarmonyPatch(typeof(GeoMarketplace), "AfterMissionComplete")]
            public static class GeoMarketplace_AfterMissionComplete_patch
            {
                public static bool Prefix(TimeUnit ____updateOptionsNextTime, GeoLevelController ____level)
                {
                    try
                    {
                        TFTVLogger.Always($"Canceling GeoMarketPlace AfterMissionComplete");
                        return false;

                    }
                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                        throw;
                    }
                }
            }


        }

        internal class TFTVMarketPlaceUI
        {
            

            private static void FakeResearchOptionToSetupCharacterSale(UIModuleTheMarketplace uIModuleTheMarketplace)
            {
                try
                {
                    // GeoLevelController controller = GameUtl.CurrentLevel().GetComponent<GeoLevelController>();
                    uIModuleTheMarketplace.ResearchRoot.SetActive(value: true);
                    uIModuleTheMarketplace.ItemsRoot.SetActive(value: false);
                    ResearchDef researchById = uIModuleTheMarketplace.Context.Level.GetResearchById("PX_Synedrion_ResearchDef");
                    ResearchElement researchElement = new ResearchElement(researchById);
                    researchElement.Init(uIModuleTheMarketplace.Context.ViewerFaction, researchById);
                    researchElement.State = ResearchState.Revealed;
                    uIModuleTheMarketplace.ResearchInfo.Init(researchElement);


                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }

            private static void ResearchInfoForCharacterKludge(GeoEventChoice choice, UIModuleTheMarketplace uIModuleTheMarketplace)
            {
                try
                {


                    if (choice != null && choice.Outcome != null && choice.Outcome.Units != null && choice.Outcome.Units.Count > 0
                        && choice.Outcome.Units[0] is TacCharacterDef tacCharacterDef && tacCharacterDef.Data.GameTags.Contains(MercenaryTag))
                    {
                        FakeResearchOptionToSetupCharacterSale(uIModuleTheMarketplace);

                        uIModuleTheMarketplace.ResearchRoot.SetActive(false);
                        uIModuleTheMarketplace.ResearchRoot.SetActive(true);

                        uIModuleTheMarketplace.ResearchInfo.Title.text = TFTVCommonMethods.ConvertKeyToString(tacCharacterDef.Data.ViewElementDef.DisplayName1.LocalizationKey);
                        /*  uIModuleTheMarketplace.ResearchInfo.Title.rectTransform.sizeDelta =
                              new Vector2(uIModuleTheMarketplace.ResearchInfo.Title.rectTransform.sizeDelta.x * 2, uIModuleTheMarketplace.ResearchInfo.Title.rectTransform.sizeDelta.y);
                          uIModuleTheMarketplace.ResearchInfo.Title.resizeTextMaxSize = 48;*/

                     /*   TFTVLogger.Always($"font size: {uIModuleTheMarketplace.ResearchInfo.Title.fontSize}; " +
                            $"size of rectransfrom {uIModuleTheMarketplace.ResearchInfo.Title.rectTransform.sizeDelta}; " +
                            $"resize text max size: {uIModuleTheMarketplace.ResearchInfo.Title.resizeTextMaxSize};" +
                            $"resize text min size:{uIModuleTheMarketplace.ResearchInfo.Title.resizeTextMinSize}" +
                            $"resize text for best fit: {uIModuleTheMarketplace.ResearchInfo.Title.resizeTextForBestFit}");*/

                        uIModuleTheMarketplace.ResearchInfo.Description.text = TFTVCommonMethods.ConvertKeyToString(tacCharacterDef.Data.ViewElementDef.Description.LocalizationKey);
                        uIModuleTheMarketplace.ResearchInfo.BenefitsContainer.SetActive(false);
                        uIModuleTheMarketplace.ResearchInfo.ResourceContainer.SetActive(false);
                        uIModuleTheMarketplace.ResearchInfo.RequirementsContainer.SetActive(false);
                        uIModuleTheMarketplace.ResearchInfo.Icon.sprite = tacCharacterDef.Data.ViewElementDef.InventoryIcon;

                    }

                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }



            [HarmonyPatch(typeof(UIModuleTheMarketplace), "SetupChoiceInfoBlock")]
            public static class UIModuleTheMarketplace_SetupChoiceInfoBlock_patch
            {
                public static void Postfix(UIModuleTheMarketplace __instance, GeoEventChoice choice)
                {
                    try
                    {

                        ResearchInfoForCharacterKludge(choice, __instance);

                    }
                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                        throw;
                    }
                }
            }


            [HarmonyPatch(typeof(UIModuleTheMarketplace), "UpdateVisuals")]
            public static class UIModuleTheMarketplace_UpdateVisuals_patch
            {

                public static void Postfix(UIModuleTheMarketplace __instance)
                {
                    try
                    {

                        //   TFTVLogger.Always($"Running UpdateVisuals");

                        CreateTestingButton();

                    }
                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                        throw;
                    }
                }
            }

            [HarmonyPatch(typeof(UIModuleTheMarketplace), "OnChoiceSelected")]
            public static class UIModuleTheMarketplace_OnChoiceSelected_patch
            {
               
                    public static void Postfix(UIModuleTheMarketplace __instance, GeoEventChoice choice)
                {
                    try
                    {

                        //  TFTVLogger.Always($"Running OnChoiceSelected");
                        if (MPGeoEventChoices != null && MPGeoEventChoices.Contains(choice))
                        {
                            //    TFTVLogger.Always($"Removing choice from internally saved list");

                            MPGeoEventChoices.Remove(choice);
                            
                        }

                        if (choice.Outcome.Units.Count > 0 && choice.Outcome.Units[0] is TacCharacterDef tacCharacterDef && tacCharacterDef.Data.GameTags.Contains(MercenaryTag))
                        {


                            //  TFTVLogger.Always($"got to this if here");

                            __instance.Loca_AllMissionsFinishedDesc.LocalizationKey = tacCharacterDef.Data.ViewElementDef.Category.LocalizationKey;
                            __instance.UpdateVisuals();
                        }

                        CheckSecretMPCounter();

                    }
                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                        throw;
                    }
                }
            }



            [HarmonyPatch(typeof(UIStateMarketplaceGeoscapeEvent), "ExitState")]
            public static class UIStateMarketplaceGeoscapeEvent_ExitState_patch
            {

                public static void Postfix()
                {
                    try
                    {

                        //   TFTVLogger.Always($"Running ExitState marketplace");
                        GeoMarketplace geoMarketplace = GameUtl.CurrentLevel().GetComponent<GeoLevelController>().Marketplace;
                        if (MPGeoEventChoices != null && MPGeoEventChoices.Count > 0)
                        {
                            PropertyInfo propertyInfo = typeof(GeoMarketplace).GetProperty("MarketplaceChoices", BindingFlags.Instance | BindingFlags.Public);

                            // TFTVLogger.Always($"before manually transferring the MarketChoices {propertyInfo.GetValue(geoMarketplace)}");                
                            propertyInfo.SetValue(geoMarketplace, new List<GeoEventChoice>(MPGeoEventChoices));
                            //  TFTVLogger.Always($"after manually transferring the MarketChoices {propertyInfo.GetValue(geoMarketplace)}");
                            MPGeoEventChoices = null;
                            //  TFTVLogger.Always($"after clearing the internal MarketChoices list {propertyInfo.GetValue(geoMarketplace)}");
                            UIModuleTheMarketplace marketplaceUI = GameUtl.CurrentLevel().GetComponent<GeoLevelController>().View.GeoscapeModules.TheMarketplaceModule;
                            marketplaceUI.transform.GetComponentsInChildren<Image>().FirstOrDefault(c => c.name.Equals("Picture")).sprite = Helper.CreateSpriteFromImageFile("UI_KaosMarket_Image_uinomipmaps.jpg");
                        }
                     

                    }
                    catch (Exception e)
                    {
                        TFTVLogger.Error(e);
                        throw;
                    }
                }
            }





            public static PhoenixGeneralButton MarketToggleButton = null;


            private static void CreateTestingButton()
            {
                try
                {
                    if (MarketToggleButton == null)
                    {

                        UIModuleTheMarketplace marketplaceUI = GameUtl.CurrentLevel().GetComponent<GeoLevelController>().View.GeoscapeModules.TheMarketplaceModule;
           
                        marketplaceUI.MissionRewardHeaderText.gameObject.SetActive(true);
                        marketplaceUI.MissionRewardDescriptionText.gameObject.SetActive(true);

                        marketplaceUI.MissionRewardHeaderText.text = "";
                        marketplaceUI.MissionRewardDescriptionText.text = "";

                        Resolution resolution = Screen.currentResolution;



                        // TFTVLogger.Always("Resolution is " + Screen.currentResolution.width);
                        float resolutionFactorWidth = (float)resolution.width / 1920f;
                        //   TFTVLogger.Always("ResolutionFactorWidth is " + resolutionFactorWidth);
                        float resolutionFactorHeight = (float)resolution.height / 1080f;
                        //   TFTVLogger.Always("ResolutionFactorHeight is " + resolutionFactorHeight);

                        //  marketplaceUI.MissionRewardHeaderText.gameObject.SetActive(true);
                        PhoenixGeneralButton allToggle = UnityEngine.Object.Instantiate(marketplaceUI.LocateMissionButton, marketplaceUI.MissionRewardDescriptionText.transform);
                        PhoenixGeneralButton vehicleToggle = UnityEngine.Object.Instantiate(marketplaceUI.LocateMissionButton, marketplaceUI.MissionRewardDescriptionText.transform);
                        PhoenixGeneralButton equipmentToggle = UnityEngine.Object.Instantiate(marketplaceUI.LocateMissionButton, marketplaceUI.MissionRewardDescriptionText.transform);
                        PhoenixGeneralButton otherToggle = UnityEngine.Object.Instantiate(marketplaceUI.LocateMissionButton, marketplaceUI.MissionRewardDescriptionText.transform);

                        allToggle.gameObject.AddComponent<UITooltipText>().TipText = "ALL";
                        allToggle.gameObject.SetActive(true);
                        allToggle.PointerClicked += () => ToggleButtonClicked(0);
                        allToggle.transform.GetComponentInChildren<Text>().text = "ALL";
                        //  allToggle.transform.localScale *= 0.6f;
                        allToggle.transform.position -= new Vector3(-150 * resolutionFactorWidth, 100 * resolutionFactorHeight, 0);



                        allToggle.GetComponent<RectTransform>().sizeDelta = new Vector2(allToggle.GetComponent<RectTransform>().sizeDelta.x * 0.65f, allToggle.GetComponent<RectTransform>().sizeDelta.y);


                        vehicleToggle.gameObject.AddComponent<UITooltipText>().TipText = "VEHICLES";
                        vehicleToggle.gameObject.SetActive(true);
                        vehicleToggle.PointerClicked += () => ToggleButtonClicked(1);
                        vehicleToggle.transform.GetComponentInChildren<Text>().text = "VEHICLES";
                        vehicleToggle.transform.GetComponentsInChildren<Image>().FirstOrDefault(c => c.name == "Icon").sprite = Helper.CreateSpriteFromImageFile("UI_Vehicle_FilterIcon.png");
                      
                        vehicleToggle.transform.position -= new Vector3(150 * resolutionFactorWidth, 0, 0); //new Vector3(150 * resolutionFactorWidth, 100 * resolutionFactorHeight, 0);
                        vehicleToggle.GetComponent<RectTransform>().sizeDelta = new Vector2(allToggle.GetComponent<RectTransform>().sizeDelta.x, allToggle.GetComponent<RectTransform>().sizeDelta.y);


                        equipmentToggle.gameObject.AddComponent<UITooltipText>().TipText = "EQUIPMENT";
                        equipmentToggle.gameObject.SetActive(true);
                        equipmentToggle.PointerClicked += () => ToggleButtonClicked(2);
                        equipmentToggle.transform.GetComponentInChildren<Text>().text = "EQUIPMENT";
                        //    equipmentToggle.transform.localScale *= 0.5f;
                        equipmentToggle.transform.position -= new Vector3(-150 * resolutionFactorWidth, 0, 0);
                        equipmentToggle.GetComponent<RectTransform>().sizeDelta = new Vector2(allToggle.GetComponent<RectTransform>().sizeDelta.x, allToggle.GetComponent<RectTransform>().sizeDelta.y);
                        equipmentToggle.transform.GetComponentsInChildren<Image>().FirstOrDefault(c => c.name == "Icon").sprite = Helper.CreateSpriteFromImageFile("MP_UI_Choices_Equipment.png");

                        otherToggle.gameObject.AddComponent<UITooltipText>().TipText = "OTHER";
                        otherToggle.gameObject.SetActive(true);
                        otherToggle.PointerClicked += () => ToggleButtonClicked(3);
                        otherToggle.transform.GetComponentInChildren<Text>().text = "OTHER";
                        //   otherToggle.transform.localScale *= 0.5f;
                        otherToggle.transform.position -= new Vector3(150 * resolutionFactorWidth, 100 * resolutionFactorHeight, 0);
                        otherToggle.GetComponent<RectTransform>().sizeDelta = new Vector2(allToggle.GetComponent<RectTransform>().sizeDelta.x, allToggle.GetComponent<RectTransform>().sizeDelta.y);
                        otherToggle.transform.GetComponentsInChildren<Image>().FirstOrDefault(c => c.name == "Icon").sprite = Helper.CreateSpriteFromImageFile("Geoscape_Icon_Research.png");

                    }

                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                }
            }

            private static List<GeoEventChoice> MPGeoEventChoices = null;


            private static bool CheckIfMarketChoiceVehicle(GeoEventChoice choice)
            {
                try
                {

                    if (choice.Outcome.Items.Count > 0 && choice.Outcome.Items[0].ItemDef.name.Contains("GroundVehicle")
                        || choice.Outcome.Units.Count > 0 && choice.Outcome.Units[0].name.Contains("KS_Kaos_Buggy")) //&& choice.Outcome.Units[0].name.Contains("KS_Kaos_Buggy")))
                    {

                        return true;

                    }
                    else return false;


                }


                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }

            }

            private static bool CheckIfMarketChoiceWeaponOrAmmo(GeoEventChoice choice)
            {
                try
                {
                    if (choice.Outcome != null && choice.Outcome.Items != null
                                        && choice.Outcome.Items.Count > 0 && choice.Outcome.Items[0].ItemDef != null
                                        && (choice.Outcome.Items[0].ItemDef.name.Contains("WeaponDef") || choice.Outcome.Items[0].ItemDef.name.Contains("AmmoClip"))
                                        && !choice.Outcome.Items[0].ItemDef.name.Contains("GroundVehicle"))
                    {

                        return true;

                    }
                    else return false;


                }


                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }

            }

            public static void FilterMarketPlaceOptions(GeoMarketplace geoMarketplace, int filter)
            {
                try
                {
                    PropertyInfo propertyInfoMarketPlaceChoicesGeoEventChoice = typeof(GeoMarketplace).GetProperty("MarketplaceChoices", BindingFlags.Instance | BindingFlags.Public);

                    if (MPGeoEventChoices == null)
                    {
                        // TFTVLogger.Always($"saving all Choices to internal list, count is {geoMarketplace.MarketplaceChoices.Count}");
                        MPGeoEventChoices = geoMarketplace.MarketplaceChoices;
                    }
                    else
                    {
                        // TFTVLogger.Always($"passing all Choices from internal list, count {MPGeoEventChoices.Count}, to proper list, count {geoMarketplace.MarketplaceChoices.Count}");
                        propertyInfoMarketPlaceChoicesGeoEventChoice?.SetValue(geoMarketplace, MPGeoEventChoices);

                    }

                    List<GeoEventChoice> choicesToShow = new List<GeoEventChoice>();

                    if (filter != 0)
                    {
                        if (filter == 1)
                        {
                            // TFTVLogger.Always($"There are {geoMarketplace.MarketplaceChoices.Count} choices");

                            for (int i = 0; i < geoMarketplace.MarketplaceChoices.Count; i++)
                            {
                                if (CheckIfMarketChoiceVehicle(geoMarketplace.MarketplaceChoices[i]))
                                {
                                    //TFTVLogger.Always($"the vehicle equipment choice number {i} is {geoMarketplace.MarketplaceChoices[i].Outcome.Items[0].ItemDef.name}");
                                    choicesToShow.Add(geoMarketplace.MarketplaceChoices[i]);
                                }
                            }

                            propertyInfoMarketPlaceChoicesGeoEventChoice.SetValue(geoMarketplace, choicesToShow);

                        }
                        else if (filter == 2)
                        {
                            //   TFTVLogger.Always($"There are {geoMarketplace.MarketplaceChoices.Count} choices");

                            for (int i = 0; i < geoMarketplace.MarketplaceChoices.Count; i++)
                            {
                                if (CheckIfMarketChoiceWeaponOrAmmo(geoMarketplace.MarketplaceChoices[i]))
                                {
                                    // TFTVLogger.Always($"the weapon or ammo choice number {i} is {geoMarketplace.MarketplaceChoices[i].Outcome.Items[0].ItemDef.name}");
                                    choicesToShow.Add(geoMarketplace.MarketplaceChoices[i]);
                                }
                            }

                        }
                        else if (filter == 3)
                        {
                            //   TFTVLogger.Always($"There are {geoMarketplace.MarketplaceChoices.Count} choices");

                            for (int i = 0; i < geoMarketplace.MarketplaceChoices.Count; i++)
                            {
                                if (!CheckIfMarketChoiceWeaponOrAmmo(geoMarketplace.MarketplaceChoices[i]) && !CheckIfMarketChoiceVehicle(geoMarketplace.MarketplaceChoices[i]))
                                {
                                    // TFTVLogger.Always($"the other choice number {i} is {geoMarketplace.MarketplaceChoices[i].Outcome.Items[0].ItemDef.name}");
                                    choicesToShow.Add(geoMarketplace.MarketplaceChoices[i]);
                                }
                            }
                        }

                        propertyInfoMarketPlaceChoicesGeoEventChoice.SetValue(geoMarketplace, choicesToShow);
                    }
                    //  TFTVLogger.Always($"Count of proper list (that will be shown) is {geoMarketplace.MarketplaceChoices.Count}");

                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                }
            }

            private static void ToggleButtonClicked(int filter)
            {
                try
                {
                    GeoMarketplace geoMarketplace = GameUtl.CurrentLevel().GetComponent<GeoLevelController>().Marketplace;
                    UIModuleTheMarketplace marketplaceUI = GameUtl.CurrentLevel().GetComponent<GeoLevelController>().View.GeoscapeModules.TheMarketplaceModule;
                    FieldInfo fieldInfoGeoEventGeoscapeEvent = typeof(UIModuleTheMarketplace).GetField("_geoEvent", BindingFlags.NonPublic | BindingFlags.Instance);
                    MethodInfo methodInfoUpdateList = typeof(UIModuleTheMarketplace).GetMethod("UpdateList", BindingFlags.NonPublic | BindingFlags.Instance);


                    switch (filter)
                    {
                        case 0:

                            marketplaceUI.transform.GetComponentsInChildren<Image>().FirstOrDefault(c => c.name.Equals("Picture")).sprite = Helper.CreateSpriteFromImageFile("MP_Choices_All.jpg");
                            break;

                        case 1:

                            marketplaceUI.transform.GetComponentsInChildren<Image>().FirstOrDefault(c => c.name.Equals("Picture")).sprite = Helper.CreateSpriteFromImageFile("Encounter_4_Kaos_Buggy_uinomipmaps.jpg");
                            break;

                        case 2:

                            marketplaceUI.transform.GetComponentsInChildren<Image>().FirstOrDefault(c => c.name.Equals("Picture")).sprite = Helper.CreateSpriteFromImageFile("MP_Choices_Equipment.jpg");
                            break;

                        case 3:

                            marketplaceUI.transform.GetComponentsInChildren<Image>().FirstOrDefault(c => c.name.Equals("Picture")).sprite = Helper.CreateSpriteFromImageFile("MP_Choices_Other.jpg");
                            break;

                    }

                    marketplaceUI.ListScrollRect.ScrollToElement(0);
                    FilterMarketPlaceOptions(geoMarketplace, filter);

                    methodInfoUpdateList.Invoke(marketplaceUI, new object[] { fieldInfoGeoEventGeoscapeEvent.GetValue(marketplaceUI) });


                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                }
            }

            public static int SecretMPCounter;
            private static void CheckSecretMPCounter()
            {
                try
                {
                    SecretMPCounter++;

                    UIModuleTheMarketplace marketplaceUI = GameUtl.CurrentLevel().GetComponent<GeoLevelController>().View.GeoscapeModules.TheMarketplaceModule;

                    if (SecretMPCounter >= 20 && SecretMPCounter <= 41)
                    {

                        TFTVLogger.Always("Should trigger MP EE");
                        // TFTVLogger.Always($"{marketplaceUI.MissionDescriptionText.text}");

                        marketplaceUI.Loca_AllMissionsFinishedDesc.LocalizationKey = "KEY_SECRET_MARKETPLACE_TEXT" + (SecretMPCounter - 20);

                        // marketplaceUI.MissionDescriptionText.text = TFTVCommonMethods.ConvertKeyToString("KEY_SECRET_MARKETPLACE_TEXT0");// + );


                    }

                    if (SecretMPCounter >= 43)
                    {
                        SecretMPCounter = 0;
                        marketplaceUI.Loca_AllMissionsFinishedDesc.LocalizationKey = "KEY_MARKETPLACE_DESCRIPTION_5";
                    }

                }


                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }

        }

       

        /// <summary>
        /// Ensures that rescue vehicle missions will not contain faction vehicles if they haven't been researched by the faction yet.
        /// </summary>

        [HarmonyPatch(typeof(GeoMissionGenerator), "GetRandomMission", new Type[] { typeof(IEnumerable<MissionTagDef>), typeof(ParticipantFilter), typeof(Func<TacMissionTypeDef, bool>) })]
        public static class GeoMissionGenerator_GetRandomMission_patch
        {

            public static void Prefix(GeoMissionGenerator __instance, IEnumerable<MissionTagDef> tags, out List<CustomMissionTypeDef> __state, GeoLevelController ____level)
            {
                try
                {
                    ClassTagDef aspida = DefCache.GetDef<ClassTagDef>("Aspida_ClassTagDef");
                    ClassTagDef armadillo = DefCache.GetDef<ClassTagDef>("Armadillo_ClassTagDef");

                    MissionTagDef requiresVehicle = DefCache.GetDef<MissionTagDef>("Contains_RescueVehicle_MissionTagDef");

                    __state = new List<CustomMissionTypeDef>();


                    if (tags.Contains(requiresVehicle) && ____level != null)
                    {
                        TFTVLogger.Always($"Generating rescue Vehicle scav; checking if factions have researched Aspida/Armadillo");
                        GeoLevelController controller = ____level;

                        if (controller.NewJerichoFaction.Research!=null && !controller.NewJerichoFaction.Research.HasCompleted("NJ_VehicleTech_ResearchDef"))
                        {

                            TFTVLogger.Always($"Armadillo not researched by New Jericho");

                            foreach (CustomMissionTypeDef customMissionTypeDef in Repo.GetAllDefs<CustomMissionTypeDef>().Where(m => m.Tags.Contains(requiresVehicle)))
                            {
                                if (customMissionTypeDef.ParticipantsData[1].ActorDeployParams[0].Limit.ActorTag == armadillo)
                                {

                                    __state.Add(customMissionTypeDef);

                                }

                            }

                        }
                        if (controller.SynedrionFaction.Research != null && !controller.SynedrionFaction.Research.HasCompleted("SYN_Rover_ResearchDef"))   
                            {
                            TFTVLogger.Always($"Aspida not researched by Synedrion");

                            foreach (CustomMissionTypeDef customMissionTypeDef in Repo.GetAllDefs<CustomMissionTypeDef>().Where(m => m.Tags.Contains(requiresVehicle)))
                            {
                                if (customMissionTypeDef.ParticipantsData[1].ActorDeployParams[0].Limit.ActorTag == aspida)
                                {

                                    __state.Add(customMissionTypeDef);

                                }
                            }
                        }

                        if (__state.Count > 0)
                        {
                            TFTVLogger.Always($"Removing rescue vehicle missions with not researched vehicles from generation pool");

                            foreach (CustomMissionTypeDef mission in __state)
                            {
                                mission.Tags.Remove(requiresVehicle);



                            }
                        }
                    }
                }

                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }


            public static void Postfix(GeoMissionGenerator __instance, IEnumerable<MissionTagDef> tags, in List<CustomMissionTypeDef> __state)
            {
                try
                {
                    ClassTagDef aspida = DefCache.GetDef<ClassTagDef>("Aspida_ClassTagDef");
                    ClassTagDef armadillo = DefCache.GetDef<ClassTagDef>("Armadillo_ClassTagDef");


                    MissionTagDef requiresVehicle = DefCache.GetDef<MissionTagDef>("Contains_RescueVehicle_MissionTagDef");

                    if (tags.Contains(DefCache.GetDef<MissionTagDef>("Contains_RescueVehicle_MissionTagDef")) && __state.Count > 0)
                    {
                        TFTVLogger.Always($"Adding back missions that were removed from the pool");

                        foreach (CustomMissionTypeDef mission in __state)
                        {

                            if (!mission.Tags.Contains(requiresVehicle))
                            {
                                mission.Tags.Add(requiresVehicle);

                            }

                        }

                    }

                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                    throw;
                }
            }
        }

    
        public static void ForceMarketPlaceUpdate()
        {

            try
            {
                GeoLevelController controller = GameUtl.CurrentLevel()?.GetComponent<GeoLevelController>();

                GeoMarketplace geoMarketplace = controller.Marketplace;
                MethodInfo updateOptionsWithRespectToTimeMethod = typeof(GeoMarketplace).GetMethod("UpdateOptionsWithRespectToTime", BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo updateOptionsNextTimeField = typeof(GeoMarketplace).GetField("_updateOptionsNextTime", BindingFlags.NonPublic | BindingFlags.Instance);

                updateOptionsNextTimeField.SetValue(geoMarketplace, controller.Timing.Now);
                TFTVLogger.Always($"Forced Marketplace options update; changing next update time to now, {controller.Timing.Now.DateTime}");

                updateOptionsWithRespectToTimeMethod.Invoke(geoMarketplace, null);


            }
            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }
        }

      

        [HarmonyPatch(typeof(GeoMarketplace), "OnSiteVisited")]
        public static class GeoMarketplace_OnSiteVisited_MarketPlace_patch
        {
            public static void Prefix(GeoMarketplace __instance, GeoLevelController ____level, TheMarketplaceSettingsDef ____settings)
            {
                try
                {
                    if (____level.EventSystem.GetVariable(____settings.NumberOfDLC5MissionsCompletedVariable) == 0)
                    {
                        TFTVLogger.Always($"Marketplace visited for the first time");

                        ____level.EventSystem.SetVariable(____settings.NumberOfDLC5MissionsCompletedVariable, 4);
                        ____level.EventSystem.SetVariable(____settings.DLC5IntroCompletedVariable, 1);
                        ____level.EventSystem.SetVariable(____settings.DLC5FinalMovieCompletedVariable, 1);
                        ForceMarketPlaceUpdate();
                    }

                }
                catch (Exception e)
                {
                    TFTVLogger.Error(e);
                }
            }
        }



        /// <summary>
        /// Easter egg conversation in the Marketplace after player makes a lot of purchases
        /// </summary>


      

    }
}

