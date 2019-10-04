using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3MapLoader.Components
{
    public class GameConstants
    {
        // Player
        public const int PLAYER_LIMIT_T = 8;
        public const int MAX_HEROES_PER_PLAYER = 8;
        public const int AVAILABLE_HEROES_PER_PLAYER = 2;
        public const int ALL_PLAYERS = 255; //bitfield
        public const int F_NUMBER = 9;

        // Heroes
        public const int HEROES_QUANTITY = 156;
        public const int HERO_GOLD_COST = 2500;
        public const int HEROES_PER_TYPE = 8; //amount of heroes of each type
        public const int BASE_MOVEMENT_COST = 100;
        public const int HERO_PORTRAIT_SHIFT = 30;// 2 special frames + some extra portraits

        public const int SPELLBOOK_GOLD_COST = 500;
        public const int SKILL_GOLD_COST = 2000;

        // Creatures
        public const int CREATURES_PER_TOWN = 7; //without upgrades
        public const int CREATURES_COUNT = 197;

        // Artifacts
        public const int ARTIFACTS_QUANTITY = 171;


        // Spells
        public const int SPELLS_QUANTITY = 70;
        public const int SPELL_LEVELS = 5;
        public const int SPELL_SCHOOL_LEVELS = 4;


        // Skills
        public const int SKILL_QUANTITY = 28;
        public const int PRIMARY_SKILLS = 4;
        public const int SKILL_PER_HERO = 8;

        // Battle Field
        public const int BATTLE_PENALTY_DISTANCE = 10; //if the distance is > than this, then shooting stack has distance penalty
        public const int ARMY_SIZE = 7;

        public int[] POSSIBLE_TURNTIME = new int[] { 1, 2, 4, 6, 8, 10, 15, 20, 25, 30, 0 };

        //Puzzle
        public const int PUZZLE_MAP_PIECES = 48;

    }




    public enum EVictoryConditionType
    {
        ARTIFACT, GATHERTROOP, GATHERRESOURCE, BUILDCITY, BUILDGRAIL, BEATHERO,
        CAPTURECITY, BEATMONSTER, TAKEDWELLINGS, TAKEMINES, TRANSPORTITEM, WINSTANDARD = 255
    };

    public enum ELossConditionType
    {
        LOSSCASTLE, LOSSHERO, TIMEEXPIRES, LOSSSTANDARD = 255
    };

    public enum EPlayerStatus
    {
        WRONG = -1,
        INGAME,
        LOSER,
        WINNER
    };

    public enum EPlayerRelations
    {
        ENEMIES,
        ALLIES,
        SAME_PLAYER
    };

    public enum ESecondarySkillLevel
    {
        NONE,
        BASIC,
        ADVANCED,
        EXPERT,
        LEVELS_SIZE
    };

    public enum EDateType
    {
        DAY = 0,
        DAY_OF_WEEK = 1,
        WEEK = 2,
        MONTH = 3,
        DAY_OF_MONTH
    };

    public enum EObjectType
    {
        NO_OBJ = -1,
        ALTAR_OF_SACRIFICE = 2,
        ANCHOR_POINT = 3,
        ARENA = 4,
        ARTIFACT = 5,
        PANDORAS_BOX = 6,
        BLACK_MARKET = 7,
        BOAT = 8,
        BORDERGUARD = 9,
        KEYMASTER = 10,
        BUOY = 11,
        CAMPFIRE = 12,
        CARTOGRAPHER = 13,
        SWAN_POND = 14,
        COVER_OF_DARKNESS = 15,
        CREATURE_BANK = 16,
        CREATURE_GENERATOR1 = 17,
        CREATURE_GENERATOR2 = 18,
        CREATURE_GENERATOR3 = 19,
        CREATURE_GENERATOR4 = 20,
        CURSED_GROUND1 = 21,
        CORPSE = 22,
        MARLETTO_TOWER = 23,
        DERELICT_SHIP = 24,
        DRAGON_UTOPIA = 25,
        EVENT = 26,
        EYE_OF_MAGI = 27,
        FAERIE_RING = 28,
        FLOTSAM = 29,
        FOUNTAIN_OF_FORTUNE = 30,
        FOUNTAIN_OF_YOUTH = 31,
        GARDEN_OF_REVELATION = 32,
        GARRISON = 33,
        HERO = 34,
        HILL_FORT = 35,
        GRAIL = 36,
        HUT_OF_MAGI = 37,
        IDOL_OF_FORTUNE = 38,
        LEAN_TO = 39,
        LIBRARY_OF_ENLIGHTENMENT = 41,
        LIGHTHOUSE = 42,
        MONOLITH_ONE_WAY_ENTRANCE = 43,
        MONOLITH_ONE_WAY_EXIT = 44,
        MONOLITH_TWO_WAY = 45,
        MAGIC_PLAINS1 = 46,
        SCHOOL_OF_MAGIC = 47,
        MAGIC_SPRING = 48,
        MAGIC_WELL = 49,
        MERCENARY_CAMP = 51,
        MERMAID = 52,
        MINE = 53,
        MONSTER = 54,
        MYSTICAL_GARDEN = 55,
        OASIS = 56,
        OBELISK = 57,
        REDWOOD_OBSERVATORY = 58,
        OCEAN_BOTTLE = 59,
        PILLAR_OF_FIRE = 60,
        STAR_AXIS = 61,
        PRISON = 62,
        PYRAMID = 63,//subtype 0
        WOG_OBJECT = 63,//subtype > 0
        RALLY_FLAG = 64,
        RANDOM_ART = 65,
        RANDOM_TREASURE_ART = 66,
        RANDOM_MINOR_ART = 67,
        RANDOM_MAJOR_ART = 68,
        RANDOM_RELIC_ART = 69,
        RANDOM_HERO = 70,
        RANDOM_MONSTER = 71,
        RANDOM_MONSTER_L1 = 72,
        RANDOM_MONSTER_L2 = 73,
        RANDOM_MONSTER_L3 = 74,
        RANDOM_MONSTER_L4 = 75,
        RANDOM_RESOURCE = 76,
        RANDOM_TOWN = 77,
        REFUGEE_CAMP = 78,
        RESOURCE = 79,
        SANCTUARY = 80,
        SCHOLAR = 81,
        SEA_CHEST = 82,
        SEER_HUT = 83,
        CRYPT = 84,
        SHIPWRECK = 85,
        SHIPWRECK_SURVIVOR = 86,
        SHIPYARD = 87,
        SHRINE_OF_MAGIC_INCANTATION = 88,
        SHRINE_OF_MAGIC_GESTURE = 89,
        SHRINE_OF_MAGIC_THOUGHT = 90,
        SIGN = 91,
        SIRENS = 92,
        SPELL_SCROLL = 93,
        STABLES = 94,
        TAVERN = 95,
        TEMPLE = 96,
        DEN_OF_THIEVES = 97,
        TOWN = 98,
        TRADING_POST = 99,
        LEARNING_STONE = 100,
        TREASURE_CHEST = 101,
        TREE_OF_KNOWLEDGE = 102,
        SUBTERRANEAN_GATE = 103,
        UNIVERSITY = 104,
        WAGON = 105,
        WAR_MACHINE_FACTORY = 106,
        SCHOOL_OF_WAR = 107,
        WARRIORS_TOMB = 108,
        WATER_WHEEL = 109,
        WATERING_HOLE = 110,
        WHIRLPOOL = 111,
        WINDMILL = 112,
        WITCH_HUT = 113,
        HOLE = 124,
        RANDOM_MONSTER_L5 = 162,
        RANDOM_MONSTER_L6 = 163,
        RANDOM_MONSTER_L7 = 164,
        BORDER_GATE = 212,
        FREELANCERS_GUILD = 213,
        HERO_PLACEHOLDER = 214,
        QUEST_GUARD = 215,
        RANDOM_DWELLING = 216,
        RANDOM_DWELLING_LVL = 217, //subtype = creature level
        RANDOM_DWELLING_FACTION = 218, //subtype = faction
        GARRISON2 = 219,
        ABANDONED_MINE = 220,
        TRADING_POST_SNOW = 221,
        CLOVER_FIELD = 222,
        CURSED_GROUND2 = 223,
        EVIL_FOG = 224,
        FAVORABLE_WINDS = 225,
        FIERY_FIELDS = 226,
        HOLY_GROUNDS = 227,
        LUCID_POOLS = 228,
        MAGIC_CLOUDS = 229,
        MAGIC_PLAINS2 = 230,
        ROCKLANDS = 231,
    };

    public enum EActionType
    {
        CANCEL = -3,
	    END_TACTIC_PHASE = -2,
	    INVALID = -1,
	    NO_ACTION = 0,
	    HERO_SPELL,
	    WALK,
        DEFEND,
	    RETREAT,
	    SURRENDER,
	    WALK_AND_ATTACK,
	    SHOOT,
	    WAIT,
	    CATAPULT,
	    MONSTER_SPELL,
	    BAD_MORALE,         // This design is not matching my new design, just put it here for reference
	    STACK_HEAL,
	    DAEMON_SUMMONING
    };

    public enum ETerrainType
    {
        WRONG = -2, BORDER = -1, DIRT, SAND, GRASS, SNOW, SWAMP,
        ROUGH, SUBTERRANEAN, LAVA, WATER, ROCK
    };

    public enum ERoadType
    {
        NO_ROAD, DIRT_ROAD, GRAVEL_ROAD, COBBLESTONE_ROAD
    };

    public enum ERiverType
    {
        NO_RIVER, CLEAR_RIVER, ICY_RIVER, MUDDY_RIVER, LAVA_RIVER
    };

    public enum ETileType
    {
        FREE,
        POSSIBLE,
        BLOCKED,
        USED
    };



    public enum EDiggingStatus
    {
        UNKNOWN = -1,
        CAN_DIG = 0,
        LACK_OF_MOVEMENT,
        WRONG_TERRAIN,
        TILE_OCCUPIED
    };

    public enum EPathFindingLayer
    {
        LAND = 0, SAIL = 1, WATER, AIR, NUM_LAYERS, WRONG, AUTO
    };

    public enum EBattleFieldType
    {
        NONE = -1, NONE2, SAND_SHORE, SAND_MESAS, DIRT_BIRCHES, DIRT_HILLS, DIRT_PINES, GRASS_HILLS,
        GRASS_PINES, LAVA, MAGIC_PLAINS, SNOW_MOUNTAINS, SNOW_TREES, SUBTERRANEAN, SWAMP_TREES, FIERY_FIELDS,
        ROCKLANDS, MAGIC_CLOUDS, LUCID_POOLS, HOLY_GROUND, CLOVER_FIELD, EVIL_FOG, FAVORABLE_WINDS, CURSED_GROUND,
        ROUGH, SHIP_TO_SHIP, SHIP
    };

    public enum EBattleFieldImage
    {
        NONE = -1,
        COASTAL,
        CURSED_GROUND,
        MAGIC_PLAINS,
        HOLY_GROUND,
        EVIL_FOG,
        CLOVER_FIELD,
        LUCID_POOLS,
        FIERY_FIELDS,
        ROCKLANDS,
        MAGIC_CLOUDS
    };

    public enum ESiegeHex
    {
        DESTRUCTIBLE_WALL_1 = 29,
        DESTRUCTIBLE_WALL_2 = 78,
        DESTRUCTIBLE_WALL_3 = 130,
        DESTRUCTIBLE_WALL_4 = 182,
        GATE_BRIDGE = 94,
        GATE_OUTER = 95,
        GATE_INNER = 96
    };

    public enum EGateState
    {
        NONE,
	    CLOSED,
	    BLOCKED, //dead or alive stack blocking from outside
	    OPENED,
	    DESTROYED
    };

    public enum EWallState
    {
        NONE = -1, //no wall
        DESTROYED,
        DAMAGED,
        INTACT
    };

    public enum EWallPart
    {
        INDESTRUCTIBLE_PART_OF_GATE = -3,
        INDESTRUCTIBLE_PART = -2,
        INVALID = -1,
        KEEP = 0,
        BOTTOM_TOWER,
        BOTTOM_WALL,
        BELOW_GATE,
        OVER_GATE,
        UPPER_WALL,
        UPPER_TOWER,
        GATE,
        PARTS_COUNT /* This constant SHOULD always stay as the last item in the enum. */
    };

    public enum EMarketMode
    {
        RESOURCE_RESOURCE, RESOURCE_PLAYER, CREATURE_RESOURCE, RESOURCE_ARTIFACT,
        ARTIFACT_RESOURCE, ARTIFACT_EXP, CREATURE_EXP, CREATURE_UNDEAD, RESOURCE_SKILL,
        MARTKET_AFTER_LAST_PLACEHOLDER
    };

    public enum ESpellCastProblem
    {
        OK, NO_HERO_TO_CAST_SPELL, CASTS_PER_TURN_LIMIT, NO_SPELLBOOK,
        HERO_DOESNT_KNOW_SPELL, NOT_ENOUGH_MANA, ADVMAP_SPELL_INSTEAD_OF_BATTLE_SPELL,
        SPELL_LEVEL_LIMIT_EXCEEDED, NO_SPELLS_TO_DISPEL,
        NO_APPROPRIATE_TARGET, STACK_IMMUNE_TO_SPELL, WRONG_SPELL_TARGET, ONGOING_TACTIC_PHASE,
        MAGIC_IS_BLOCKED, //For Orb of Inhibition and similar - no casting at all
        INVALID
    };

    public enum EBuildingState
    {
        HAVE_CAPITAL, NO_WATER, FORBIDDEN, ADD_MAGES_GUILD, ALREADY_PRESENT, CANT_BUILD_TODAY,
        NO_RESOURCES, ALLOWED, PREREQUIRES, MISSING_BASE, BUILDING_ERROR, TOWN_NOT_OWNED
    };

    public enum EAiTactic
    {
        NONE = -1,
        RANDOM,
        WARRIOR,
        BUILDER,
        EXPLORER
    };

    public enum EArtifactPosition
    {
        FIRST_AVAILABLE = -2,
        PRE_FIRST = -1, //sometimes used as error, sometimes as first free in backpack
        HEAD, SHOULDERS, NECK, RIGHT_HAND, LEFT_HAND, TORSO, //5
        RIGHT_RING, LEFT_RING, FEET, //8
        MISC1, MISC2, MISC3, MISC4, //12
        MACH1, MACH2, MACH3, MACH4, //16
        SPELLBOOK, MISC5, //18
        AFTER_LAST,
        //cres
        CREATURE_SLOT = 0,
        COMMANDER1 = 0, COMMANDER2, COMMANDER3, COMMANDER4, COMMANDER5, COMMANDER6, COMMANDER_AFTER_LAST    // Only for WoG
    };

    public enum EArtifactID
    {
        NONE = -1,
        SPELLBOOK = 0,
        SPELL_SCROLL = 1,
        GRAIL = 2,
        CATAPULT = 3,
        BALLISTA = 4,
        AMMO_CART = 5,
        FIRST_AID_TENT = 6,
        //CENTAUR_AXE = 7,
        //BLACKSHARD_OF_THE_DEAD_KNIGHT = 8,
        ARMAGEDDONS_BLADE = 128,
        TITANS_THUNDER = 135,
        //CORNUCOPIA = 140,
        //FIXME: the following is only true if WoG is enabled. Otherwise other mod artifacts will take these slots.
        ART_SELECTION = 144,
        ART_LOCK = 145, // FIXME: We must get rid of this one since it's conflict with artifact from mods. See issue 2455
        AXE_OF_SMASHING = 146,
        MITHRIL_MAIL = 147,
        SWORD_OF_SHARPNESS = 148,
        HELM_OF_IMMORTALITY = 149,
        PENDANT_OF_SORCERY = 150,
        BOOTS_OF_HASTE = 151,
        BOW_OF_SEEKING = 152,
        DRAGON_EYE_RING = 153
        //HARDENED_SHIELD = 154,
        //SLAVAS_RING_OF_POWER = 155
    };

    public enum ECreatureID
    {
        NONE = -1,
        CAVALIER = 10,
        CHAMPION = 11,
        STONE_GOLEM = 32,
        IRON_GOLEM = 33,
        IMP = 42,
        SKELETON = 56,
        WALKING_DEAD = 58,
        WIGHTS = 60,
        LICHES = 64,
        BONE_DRAGON = 68,
        TROGLODYTES = 70,
        HYDRA = 110,
        CHAOS_HYDRA = 111,
        AIR_ELEMENTAL = 112,
        EARTH_ELEMENTAL = 113,
        FIRE_ELEMENTAL = 114,
        WATER_ELEMENTAL = 115,
        GOLD_GOLEM = 116,
        DIAMOND_GOLEM = 117,
        PSYCHIC_ELEMENTAL = 120,
        CATAPULT = 145,
        BALLISTA = 146,
        FIRST_AID_TENT = 147,
        AMMO_CART = 148,
        ARROW_TOWERS = 149
    };

    public enum EBuildingID
    {
        DEFAULT = -50,
        NONE = -1,
        MAGES_GUILD_1 = 0, MAGES_GUILD_2, MAGES_GUILD_3, MAGES_GUILD_4, MAGES_GUILD_5,
        TAVERN, SHIPYARD, FORT, CITADEL, CASTLE,
        VILLAGE_HALL, TOWN_HALL, CITY_HALL, CAPITOL, MARKETPLACE,
        RESOURCE_SILO, BLACKSMITH, SPECIAL_1, HORDE_1, HORDE_1_UPGR,
        SHIP, SPECIAL_2, SPECIAL_3, SPECIAL_4, HORDE_2,
        HORDE_2_UPGR, GRAIL, EXTRA_TOWN_HALL, EXTRA_CITY_HALL, EXTRA_CAPITOL,
        DWELL_FIRST = 30, DWELL_LVL_2, DWELL_LVL_3, DWELL_LVL_4, DWELL_LVL_5, DWELL_LVL_6, DWELL_LAST = 36,
        DWELL_UP_FIRST = 37, DWELL_LVL_2_UP, DWELL_LVL_3_UP, DWELL_LVL_4_UP, DWELL_LVL_5_UP,
        DWELL_LVL_6_UP, DWELL_UP_LAST = 43,

        DWELL_LVL_1 = DWELL_FIRST,
        DWELL_LVL_7 = DWELL_LAST,
        DWELL_LVL_1_UP = DWELL_UP_FIRST,
        DWELL_LVL_7_UP = DWELL_UP_LAST,

        //Special buildings for towns.
        LIGHTHOUSE = SPECIAL_1,
        STABLES = SPECIAL_2, //Castle
        BROTHERHOOD = SPECIAL_3,

        MYSTIC_POND = SPECIAL_1,
        FOUNTAIN_OF_FORTUNE = SPECIAL_2, //Rampart
        TREASURY = SPECIAL_3,

        ARTIFACT_MERCHANT = SPECIAL_1,
        LOOKOUT_TOWER = SPECIAL_2, //Tower
        LIBRARY = SPECIAL_3,
        WALL_OF_KNOWLEDGE = SPECIAL_4,

        STORMCLOUDS = SPECIAL_2,
        CASTLE_GATE = SPECIAL_3, //Inferno
        ORDER_OF_FIRE = SPECIAL_4,

        COVER_OF_DARKNESS = SPECIAL_1,
        NECROMANCY_AMPLIFIER = SPECIAL_2, //Necropolis
        SKELETON_TRANSFORMER = SPECIAL_3,

        //ARTIFACT_MERCHANT - same ID as in tower
        MANA_VORTEX = SPECIAL_2,
        PORTAL_OF_SUMMON = SPECIAL_3, //Dungeon
        BATTLE_ACADEMY = SPECIAL_4,

        ESCAPE_TUNNEL = SPECIAL_1,
        FREELANCERS_GUILD = SPECIAL_2, //Stronghold
        BALLISTA_YARD = SPECIAL_3,
        HALL_OF_VALHALLA = SPECIAL_4,

        CAGE_OF_WARLORDS = SPECIAL_1,
        GLYPHS_OF_FEAR = SPECIAL_2, // Fortress
        BLOOD_OBELISK = SPECIAL_3,

        //ARTIFACT_MERCHANT - same ID as in tower
        MAGIC_UNIVERSITY = SPECIAL_2, // Conflux
    };

    public enum ETownType
    {
        ANY = -1,
        CASTLE, RAMPART, TOWER, INFERNO, NECROPOLIS, DUNGEON, STRONGHOLD, FORTRESS, CONFLUX, NEUTRAL
    };

    public enum EAlignment
    {
        GOOD,
        EVIL,
        NEUTRAL
    };

    public enum ESpellID
    {
        PRESET = -2,
        NONE = -1,

        // Adventure Spell
        SUMMON_BOAT = 0, SCUTTLE_BOAT = 1, VISIONS = 2, VIEW_EARTH = 3, DISGUISE = 4, VIEW_AIR = 5,
        FLY = 6, WATER_WALK = 7, DIMENSION_DOOR = 8, TOWN_PORTAL = 9,

        // Battle Spell
        QUICKSAND = 10, LAND_MINE = 11, FORCE_FIELD = 12, FIRE_WALL = 13, EARTHQUAKE = 14,
        MAGIC_ARROW = 15, ICE_BOLT = 16, LIGHTNING_BOLT = 17, IMPLOSION = 18,
        CHAIN_LIGHTNING = 19, FROST_RING = 20, FIREBALL = 21, INFERNO = 22,
        METEOR_SHOWER = 23, DEATH_RIPPLE = 24, DESTROY_UNDEAD = 25, ARMAGEDDON = 26,
        SHIELD = 27, AIR_SHIELD = 28, FIRE_SHIELD = 29, PROTECTION_FROM_AIR = 30,
        PROTECTION_FROM_FIRE = 31, PROTECTION_FROM_WATER = 32,
        PROTECTION_FROM_EARTH = 33, ANTI_MAGIC = 34, DISPEL = 35, MAGIC_MIRROR = 36,
        CURE = 37, RESURRECTION = 38, ANIMATE_DEAD = 39, SACRIFICE = 40, BLESS = 41,
        CURSE = 42, BLOODLUST = 43, PRECISION = 44, WEAKNESS = 45, STONE_SKIN = 46,
        DISRUPTING_RAY = 47, PRAYER = 48, MIRTH = 49, SORROW = 50, FORTUNE = 51,
        MISFORTUNE = 52, HASTE = 53, SLOW = 54, SLAYER = 55, FRENZY = 56,
        TITANS_LIGHTNING_BOLT = 57, COUNTERSTRIKE = 58, BERSERK = 59, HYPNOTIZE = 60,
        FORGETFULNESS = 61, BLIND = 62, TELEPORT = 63, REMOVE_OBSTACLE = 64, CLONE = 65,
        SUMMON_FIRE_ELEMENTAL = 66, SUMMON_EARTH_ELEMENTAL = 67, SUMMON_WATER_ELEMENTAL = 68, SUMMON_AIR_ELEMENTAL = 69,

        // Creature Skills
        STONE_GAZE = 70, POISON = 71, BIND = 72, DISEASE = 73, PARALYZE = 74, AGE = 75, DEATH_CLOUD = 76, THUNDERBOLT = 77,
        DISPEL_HELPFUL_SPELLS = 78, DEATH_STARE = 79, ACID_BREATH_DEFENSE = 80, ACID_BREATH_DAMAGE = 81,

        FIRST_NON_SPELL = 70, AFTER_LAST = 82
    };

    public enum ESpellSchool
    {
        AIR 	= 0,
	    FIRE 	= 1,
	    WATER 	= 2,
	    EARTH 	= 3
    };

    public enum EMetaclass
    {
        INVALID = 0,
	    ARTIFACT,
	    CREATURE,
	    FACTION,
	    EXPERIENCE,
	    HERO,
	    HEROCLASS,
	    LUCK,
	    MANA,
	    MORALE,
	    MOVEMENT,
	    OBJECT,
	    PRIMARY_SKILL,
	    SECONDARY_SKILL,
	    SPELL,
	    RESOURCE
    };

    public enum EHealLevel
    {
        HEAL,
	    RESURRECT,
	    OVERHEAL
    };

    public enum EHealPower
    {
        ONE_BATTLE,
	    PERMANENT
    };

    public enum ESecondarySkill
    {
        WRONG = -2,
        DEFAULT = -1,
        PATHFINDING = 0, ARCHERY, LOGISTICS, SCOUTING, DIPLOMACY, NAVIGATION, LEADERSHIP, WISDOM, MYSTICISM,
        LUCK, BALLISTICS, EAGLE_EYE, NECROMANCY, ESTATES, FIRE_MAGIC, AIR_MAGIC, WATER_MAGIC, EARTH_MAGIC,
        SCHOLAR, TACTICS, ARTILLERY, LEARNING, OFFENCE, ARMORER, INTELLIGENCE, SORCERY, RESISTANCE,
        FIRST_AID, SKILL_SIZE
    };

    public enum PrimarySkill
    {
        ATTACK, DEFENSE, SPELL_POWER, KNOWLEDGE,
        EXPERIENCE = 4      //for some reason changePrimSkill uses it
    };

}
