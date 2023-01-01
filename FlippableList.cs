using Modding;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace PaperKnight
{
    public class FlippableList
    {
        internal static string[] sceneFlippables = new string[] {

            "Room_temple/Quirrel",                                      // Quirrel
            "Room_Slug_Shrine/Quirrel Slug Shrine",
            "Fungus2_01/Quirrel Station NPC",
            "Fungus2_14/Quirrel Mantis NPC",
            "Ruins1_02/RestBench/Quirrel Bench",
            "Deepnest_30/Quirrel Spa",
            "Mines_13/Quirrel Mines",
            "Fungus3_47/Quirrel Archive Ext",
            "Fungus3_archive_02_boss/Mega Jellyfish/Quirrel Slash",
            "Fungus3_archive_02_boss/Quirrel Land",
            "Fungus3_archive_02/Dreamer Monomon/Quirrel",
            "Crossroads_50/Quirrel Lakeside",
            
            "Fungus2_18/Card & Mushroom/Mr Mushroom NPC",               // Mr. Mushroom
            "Deepnest_East_01/Mr Mushroom NPC",
            "Deepnest_40/Mr Mushroom NPC",
            "Room_Nailmaster/Mr Mushroom NPC",
            "Abyss_21/Mr Mushroom NPC",
            "Fungus3_44/Mr Mushroom NPC",
            "Tutorial_01/Mr Mushroom NPC (1)",
            
            "Crossroads_33/Cornifer",                                   // Cornifer
            "Fungus1_06/Cornifer",
            "Fungus3_25/Cornifer",
            "Fungus2_18/_NPCs/Cornifer",
            "Deepnest_01b/Cornifer Deepnest",
            "Fungus2_25/DEEPNEST", // Is this right?
            "Deepnest_East_03/Cornifer",
            "Abyss_04/Cornifer",
            "Waterways_09/Cornifer",
            "Ruins1_31/Cornifer",
            "Mines_30/Cornifer",
            "Fungus1_24/Cornifer",
            "Cliffs_01/Cornifer",
            
            "Room_Town_Stag_Station/Stag",                              // Stag
            "Crossroads_47/Stag",
            "Fungus1_16_alt/Stag",
            "Fungus2_02/Stag",
            "Ruins1_29/Stag",
            "Ruins2_08/Stag",
            "Deepnest_09/Stag",
            "Abyss_22/Stag",
            "RestingGrounds_09/Stag",
            "Fungus3_40/Stag",
            "Cliffs_03/Stag",

            "Room_Nailsmith/Nailsmith",                                 // Nailsmith
            "Ruins1_04/Nailsmith Cliff NPC",
            "GG_Waterways/Nailsmith_Corpse",
            "Room_Nailmaster_02/NM Parent/Painting/Nailsmith Painted NPC",
            "Room_Nailmaster_02/NM Parent/Modelling/Nailsmith Modelling NPC",

            "Room_Shop/Basement Closed/Sly Shop",                       // Sly
            "Room_Sly_Storeroom/Sly Basement NPC",
            "Room_ruinhouse/Sly Dazed",

            "Room_Nailmaster/NM Mato NPC",                              // Nailmaster Mato
            "Room_Nailmaster_03/NM Oro NPC",                            // Nailmaster Oro
            "Room_Nailmaster_02/NM Parent/Painting/NM Sheo NPC",        // Nailmaster Sheo
            "Room_Nailmaster_02/NM Parent/Modelling/NM Sheo NPC Modeller",

            "Fungus2_09/Cloth NPC 1",                                   // Cloth
            "Deepnest_14/Cloth NPC 2",
            "Abyss_17/Cloth NPC Tramway",
            "Fungus3_34/Cloth NPC QG Entrance",
            "Fungus3_23/Cloth Corpse",
            "Fungus3_23_boss/Battle Scene/Cloth Entry/Cloth Fighter",
            "Town/Cloth NPC Town",

            "Ruins1_27/Hornet Fountain Encounter/Hornet",               // Hornet
            "Abyss_06_Core/Hornet Abyss NPC",
            "Deepnest_Spider_Town/Hornet Beast Den NPC",
            "Room_temple/Hornet Black Egg NPC",

            "Town/_NPCs/Tiso Town NPC",                                 // Tiso
            "Crossroads_47/_NPCs/Tiso Bench NPC",
            "Crossroads_50/Tiso Lake NPC",
            "Room_Colossem_02/Tiso Col NPC",
            "Deepnest_East_07/tiso_corpse",
            "GG_Brooding_Mawlek_V/Battle Scene/Tiso Boss",

            "Fungus1_20_v02/Zote Death",                                // Zote the Mighty
            "Fungus1_20_v02/Zote Buzzer Convo(Clone)",
            "Town/Zote Town",
            "Town/Zote Final Scene/Zote Final",
            "Ruins1_06/Zote Ruins",
            "Deepnest_33/Zote Deepnest",
            "Deepnest_33/Zote Deepnest/Faller/NPC",
            "Room_Colosseum_02/Zote Colosseum",
            "Room_Colosseum_Bronze/Corpse Zote Boss(Clone)",

            "Fungus2_23/Bretta Dazed",                                  // Bretta
            "Town/Bretta Bench",
            "Town/Zote Final Scene/Bretta Zote Fan",
            "Room_Bretta/Bretta Sleeping",

            "GG_Wyrm/Godseeker Young NPC",                              // The Godseeker
            "GG_Engine_Root/Godseeker Young NPC",
            "GG_Unn/Godseeker Young NPC",
            "GG_Engine/Godseeker EngineRoom NPC",
            "GG_Engine_Prime/Godseeker EngineRoom NPC",
            "GG_Waterways/Godseeker Waterways/Godseeker Fall",
            "GG_Waterways/Godseeker Waterways/Godseeker Dazed",
            "GG_Waterways/Godseeker Waterways/Godseeker Awake",

            "GG_Spa/spa_pieces/atrium_NPC_Hornhead_sit (1)",            // Mini Godseekers
            "GG_Spa/spa_pieces/atrium_NPC_trihead_sit",
            "GG_Atrium/atrium_NPC_Hornhead_sit",
            "GG_Atrium/atrium_NPC_trihead_sit (0-1)",
            "GG_Atrium/atrium_NPC_round_stand (0-2)",
            "GG_Atrium/atrium_NPC_trihead_stand (0-1)",
            "GG_Atrium/atrium_NPC_Hornhead_stand (0-2)",
            "GG_Atrium/atrium_NPC_round_FG (0-1)",

            "Ruins2_Watcher_Room/Dreamer Lurien",                       // Lurien
            "Dream_Guardian_Lurien/Dreamer NPC",

            "Fungus3_archive_02/Dreamer Monomon",                       // Monomon
            "Dream_Guardian_Monomon/Dreamer NPC",

            "Deepnest_Spider_Town/Dreamer Hegemol",                     // Herrah
            "Dream_Guardian_Hegemol/Dreamer NPC",

            "Deepnest_Spider_Town/Spider Royal Tall (0-3)",             // Distant Villagers
            "Deepnest_Spider_Town/Spider Royal Short (0-3)",
            "Deepnest_Spider_Town/Spider Royal Remains/Spider Royal Head Tall (0-2)",
            "Deepnest_Spider_Town/Spider Royal Remains/Spider Royal Head Short (0-3)",
            "Deepnest_Spider_Town/Spider Royal Remains/_0000_s_royal_02_robe (0-3)",
            "Deepnest_Spider_Town/Spider Royal Remains/_0001_s_royal_01_robe (0-2)",

            "Grimm_Main_Tent/Grimm Holder/Grimm Scene/Grimm NPC",       // Troupe Master Grimm
            "Grimm_Main_Tent/Grimm_sleep_heartbeat0002",

            "Grimm_Main_Tent/Brum NPC",                                 // Brumm
            "Room_Spider_Small/Brumm Torch NPC",
            "Cliffs_06/Brumm Lantern NPV",

            "Town/Grimmsteed BG",                                       // Grimmsteed
            "Town/Grimmsteed FG",

            "Fungus3_35/Banker",                                        // Millibelle the Banker
            "Ruins_Bathhouse/Banker Spa NPC",

            "Ruins1_05b/Relic Dealer",                                  // Relic Seeker Lemm
            "Ruins1_27/Antique Dealer Outside",

            "Waterways_03/Alive_Tuk/Tuk NPC",                           // Tuk
            "Waterways_03/Dead_Tuk/Tuk_dead",

            "Crossroads_38/Fat Grub King",                              // Grubfather
            "Crossroads_38/Grub King",

            "Fungus1_08/Hunter Entry/Hunter",                           // The Hunter
            "Fungus1_08/Hunter Hangout NPC",

            "Deepnest_East_04/Big Caterpillar",                         // Bardoon
            "Deepnest_East_04/Big Caterpillar Tail",

            "Waterways_05/Dung Defender NPC",                           // Dung Defender
            "Waterways_15/Dung Defender_Sleep",
            "Waterways_15/Dung Defender_Awake",

            "Town/_NPCs/Elderbug",                                      // Elderbug
            "Town/_NPCs/Elderbug Grimm",

            "Fungus3_39/Moss Cultist",                                  // Moss Prophet
            "Fungus3_39/corpse set/corpse0000",

            "Crossroads_45/Miner",                                      // Myla
            "Crossroads_45/Myla Crazy NPC",

            "Crossroads_ShamanTemple/_Props/Shaman Meeting",            // Snail Shaman
            "Crossroads_ShamanTemple/_Props/Shaman Killed Blocker",

            "Fungus2_15_boss/Mantis Battle/Mantis Lord Throne 1",       // Mantis Lords
            "Fungus2_15_boss/Mantis Battle/Mantis Lord Throne 2",
            "Fungus2_15_boss/Mantis Battle/Mantis Lord Throne 3",

            "Fungus2_15_boss_defeated/Mantis Lord Throne 1",            // Mantis Lords (after defeat)
            "Fungus2_15_boss_defeated/Mantis Lord Throne 2",           
            "Fungus2_15_boss_defeated/Mantis Lord Throne 3",      

            "Room_Queen/Queen",                                         // White Lady
            "Room_Queen/queen_0023_a",
            "Room_Queen/queen_0022_a",

            "Fungus1_20_v02/_Enemies/Giant Buzzer",                     // Vengefly King
            "Crossroads_10/Mace Head Bug",                              // Mace Bug
            "Room_Charm_Shop/Charm Slug",                               // Charm Lover Salubra
            "Room_Ouiji/Jiji NPC",                                      // Confessor Jiji
            "Grimm_Divine/Divine NPC",                                  // Divine
            "Room_Mapper/Iselda",                                       // Iselda
            "Fungus2_26/Leg Eater",                                     // Leg Eater
            "Room_Colloseum_01/Little Fool NPC",                        // Little Fool
            "Room_Jinn/Jinn NPC",                                       // Steel Soul Jinn
            "Room_Mansion/Xun NPC",                                     // Grey Mourner
            "RestingGrounds_07/Dream Moth",                             // Seer
            "Ruins_House_03/Emilitia NPC",                              // Eternal Emilitia
            "Room_GG_Shortcut/Fluke Hermit",                            // Fluke Hermit
            "Room_Mask_Maker/Maskmaker NPC",                            // Mask Maker
            "Deepnest_41/Happy Spider NPC",                             // Midwife
            "Town/Nymm NPC",                                            // Nymm
            "Dream_Backer_Shrine/moth_ghost_idle0002",                  // Unnamed Moth
            "Fungus2_34/Giraffe NPC",                                   // Willoh
            "White_Palace_09/White King Corpse/Sprite",                 // Pale King is dead lmao

            // Spirits
            "Town/_NPCs/Gravedigger NPC",                               // Gravedigger
            "Cliffs_05/Ghost Activator/Ghost NPC Joni",                 // Joni
            "Fungus3_49/Mantis Grave/Mantis Ghost",                     // Traitor Child
            "Ruins_Bathhouse/Ghost NPC/Character Sprite",               // Marissa
            "Ruins_Elevator/Ghost NPC",                                 // Poggy Thorax
            "Fungus1_24/Ghost NPC",                                     // Caelif & Fera Orthop
            "Hive_05/Battle Scene/Vespa NPC",                           // Vespa
            "Fungus3_23/Cloth Ghost NPC",                               // Cloth, but dead

            // Spirits Glade
            "RestingGrounds_08/Ghost kcin",
            "RestingGrounds_08/Ghost atra",
            "RestingGrounds_08/Ghost wyatt",
            "RestingGrounds_08/Ghost boss",
            "RestingGrounds_08/Ghost chagax",
            "RestingGrounds_08/Ghost hex",
            "RestingGrounds_08/Ghost garro",
            "RestingGrounds_08/Ghost perpetos",
            "RestingGrounds_08/Ghost molten",
            "RestingGrounds_08/Ghost revek",
            "RestingGrounds_08/Ghost NPC 100 nail",
            "RestingGrounds_08/Ghost caspian",
            "RestingGrounds_08/Ghost waldie",
            "RestingGrounds_08/Ghost milly",
            "RestingGrounds_08/Ghost magnus",
            "RestingGrounds_08/Ghost grohac",
            "RestingGrounds_08/Ghost wayner",
            "RestingGrounds_08/Ghost thistlewind",

            // Flippable scene Objects (zote skull?, Others???)
        };



        internal static string[] objectFlippables = new string[] { // Persisting Objects and Corpses that activate at death
            // Charm Minions
            "Weaverling",
            "Grimmchild",


            // Corpses
            "Mace Head Bug",
            //"Zote Buzzer Encounter",
            "Zote Buzzer Convo",
        };

        internal static string[] preventFlippables = new string[] {
            "False Knight New/Rage Begin",
            "False Knight Dream/Rage Begin"
        };

        internal static string[] enableFlippables = new string[] {
            "False Knight New/Stun Start",
            "False Knight New/Rage End",
            "False Knight New/JA Antic 2", // Final time when rage ends.
            "False Knight Dream/Stun Start",
            "False Knight Dream/Rage End",
            "False Knight Dream/JA Antic 2", // Final time when rage ends.
        };

        internal static string[] forbiddenTags = {
            // Charm Minions
            "Weaverling",
            "Grimmchild",
            "Knight Hatchling"
        };

        internal static List<string> sceneFlipList = new List<string>(sceneFlippables);
        internal static List<string> objectFlipList = new List<string>(objectFlippables);
        internal static List<string> forbiddenTagList = new List<string>(forbiddenTags);
        internal static List<string> preventFlipList = new List<string>(preventFlippables);
        internal static List<string> enableFlipList = new List<string>(enableFlippables);
        internal static GameObject[] nailAttacks = null;

        public static GameObject[] buildNailAttacks()
        {
            GameObject attacks = GameObject.Find("Attacks");
            if (!attacks)
                return null;

            nailAttacks = new GameObject[9]
            {
                attacks,
                attacks.transform.Find("Slash") ? attacks.transform.Find("Slash").gameObject : null,
                attacks.transform.Find("AltSlash") ? attacks.transform.Find("AltSlash").gameObject : null,
                attacks.transform.Find("DownSlash") ? attacks.transform.Find("DownSlash").gameObject : null,
                attacks.transform.Find("UpSlash") ? attacks.transform.Find("UpSlash").gameObject : null,
                attacks.transform.Find("Cyclone Slash") ? attacks.transform.Find("Cyclone Slash").gameObject : null,
                attacks.transform.Find("Wall Slash") ? attacks.transform.Find("Wall Slash").gameObject : null,
                attacks.transform.Find("Great Slash") ? attacks.transform.Find("Great Slash").gameObject : null,
                attacks.transform.Find("Dash Slash") ? attacks.transform.Find("Dash Slash").gameObject : null,
            };

            return nailAttacks;
        }

        internal static Dictionary<string, string[]> flippableChildren = buildFlippableChildren(); 
        
        internal static Dictionary<string, string[]> buildFlippableChildren()
        {
            Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
            dict.Add("Divine NPC", new string[6] { "Body 1", "Body 2", "Body 3", "Body 4", "Body 5", "Body 6" });
            dict.Add("Absolute Radiance", new string[2] { "Legs", "Halo" });
            dict.Add("Nymm NPC", new string[1] { "Accordion" });
            //dict.Add("Ghost NPC", new string[1] { "Character Sprite" });
            dict.Add("Hunter Hangout NPC", new string[1] { "Bod" });
            dict.Add("Grimmsteed BG", new string[1] { "Bod" });
            dict.Add("Grimmsteed FG", new string[1] { "Bod" });
            dict.Add("Mawlek Body", new string[4] {"Mawlek Head", "Mawlek Arm L", "Mawlek Arm R", "Dummy"});
            dict.Add("False Knight New", new string[2] { "Head", "Hitter" });
            dict.Add("False Knight Dream", new string[2] { "Head", "Hitter" });


            return dict;
        }

        internal static Dictionary<string, float> SpriteCenters = buildSpriteCenters();

        private static Dictionary<string, float> buildSpriteCenters()
        {
            Dictionary<string, float> dict = new Dictionary<string, float>();
            dict.Add("White King Corpse/Sprite", 60.45F);
            dict.Add("Divine NPC", 20.00F);
            dict.Add("Queen", 70.38F);
            dict.Add("queen_0022_a", 70.38F);
            dict.Add("queen_0023_a", 70.38F);
            dict.Add("Leg Eater", 46.377F);
            dict.Add("Grimmsteed BG", 88.8174F);
            dict.Add("Grimmsteed FG", 77.71F);

            return dict;
        }

        internal static float rotationCenter(string name, string parent)
        {
            string builtName = parent != null ? parent + "/" + name : name;
            if (!SpriteCenters.ContainsKey(builtName))
                return 0;
            return SpriteCenters[builtName];
        }
    }
}



/*
 

 
Charm Lover Salubra
Confessor Jiji
Divine
Iselda
Leg Eater
Little Fool
Millibelle the Banker
Nailsmith
Relic Seeker Lemm
Sly
Steel Soul Jinn
The Last Stag
Tuk 
Nailmaster Mato	
Nailmaster Oro	
Nailmaster Sheo
Cloth	
Cornifer	
Hornet	
Mister Mushroom
Quirrel	
Tiso	
Zote the Mighty
Bretta	
Brumm	
Dreamers	
Grey Mourner
Grubfather	
Seer	
The Godseeker	
The Hunter
Troupe Master Grimm	
Bardoon	
Distant Villagers	
Dung Defender	
Elderbug
Eternal Emilitia	
Fluke Hermit	
Grimmsteed	
Mask Maker
Midwife	
Moss Prophet	
Myla	
Nymm
Snail Shaman	
Unnamed Moth	
White Lady	
Willoh
Hot Spring bugs
Distant Villagers (beasts den)
Grimmsteed (Grimm tent bugs)
Mace Bug



*/