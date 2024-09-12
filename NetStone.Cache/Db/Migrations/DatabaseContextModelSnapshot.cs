﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetStone.Cache.Db;
using NetStone.Common.Enums;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetStone.Cache.Db.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "class_job", new[] { "gladiator", "pugilist", "marauder", "lancer", "archer", "conjurer", "thaumaturge", "carpenter", "blacksmith", "armorer", "goldsmith", "leatherworker", "weaver", "alchemist", "culinarian", "miner", "botanist", "fisher", "paladin", "monk", "warrior", "dragoon", "bard", "white_mage", "black_mage", "arcanist", "summoner", "scholar", "rogue", "ninja", "machinist", "dark_knight", "astrologian", "samurai", "red_mage", "blue_mage", "gunbreaker", "dancer", "reaper", "sage", "viper", "pictomancer" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "gear_slot", new[] { "main_hand", "off_hand", "head", "body", "hands", "legs", "feet", "earrings", "necklace", "bracelets", "ring1", "ring2", "soul_crystal" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "gender", new[] { "male", "female" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "grand_company", new[] { "no_affiliation", "maelstrom", "order_of_the_twin_adder", "immortal_flames" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "race", new[] { "hyur", "elezen", "lalafell", "miqote", "roegadyn", "au_ra", "hrothgar", "viera" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "tribe", new[] { "midlander", "highlander", "wildwood", "duskwight", "plainsfolk", "dunesfolk", "seeker_of_the_sun", "keeper_of_the_moon", "sea_wolf", "hellsguard", "raen", "xaela", "helions", "the_lost", "rava", "veena" });
            NpgsqlModelBuilderExtensions.UseIdentityAlwaysColumns(modelBuilder);

            modelBuilder.Entity("NetStone.Cache.Db.Models.Character", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<ClassJob>("ActiveClassJob")
                        .HasColumnType("class_job")
                        .HasColumnName("active_class_job");

                    b.Property<string>("ActiveClassJobIcon")
                        .IsRequired()
                        .HasMaxLength(127)
                        .HasColumnType("character varying(127)")
                        .HasColumnName("active_class_job_icon");

                    b.Property<short>("ActiveClassJobLevel")
                        .HasColumnType("smallint")
                        .HasColumnName("active_class_job_level");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("avatar");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasMaxLength(3000)
                        .HasColumnType("character varying(3000)")
                        .HasColumnName("bio");

                    b.Property<DateTime?>("CharacterClassJobsUpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("character_class_jobs_updated_at");

                    b.Property<DateTime?>("CharacterMinionsUpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("character_minions_updated_at");

                    b.Property<DateTime?>("CharacterMountsUpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("character_mounts_updated_at");

                    b.Property<DateTime>("CharacterUpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("character_updated_at");

                    b.Property<int?>("FullFreeCompanyId")
                        .HasColumnType("integer")
                        .HasColumnName("full_free_company_id");

                    b.Property<Gender>("Gender")
                        .HasColumnType("gender")
                        .HasColumnName("gender");

                    b.Property<GrandCompany>("GrandCompany")
                        .HasColumnType("grand_company")
                        .HasColumnName("grand_company");

                    b.Property<string>("GrandCompanyRank")
                        .HasMaxLength(63)
                        .HasColumnType("character varying(63)")
                        .HasColumnName("grand_company_rank");

                    b.Property<string>("GuardianDeityIcon")
                        .IsRequired()
                        .HasMaxLength(127)
                        .HasColumnType("character varying(127)")
                        .HasColumnName("guardian_deity_icon");

                    b.Property<string>("GuardianDeityName")
                        .IsRequired()
                        .HasMaxLength(63)
                        .HasColumnType("character varying(63)")
                        .HasColumnName("guardian_deity_name");

                    b.Property<string>("LodestoneId")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("lodestone_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("character varying(21)")
                        .HasColumnName("name");

                    b.Property<string>("Nameday")
                        .IsRequired()
                        .HasMaxLength(63)
                        .HasColumnType("character varying(63)")
                        .HasColumnName("nameday");

                    b.Property<string>("Portrait")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("portrait");

                    b.Property<string>("PvpTeam")
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("pvp_team");

                    b.Property<Race>("Race")
                        .HasColumnType("race")
                        .HasColumnName("race");

                    b.Property<string>("Server")
                        .IsRequired()
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("server");

                    b.Property<string>("Title")
                        .HasMaxLength(63)
                        .HasColumnType("character varying(63)")
                        .HasColumnName("title");

                    b.Property<string>("TownIcon")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("town_icon");

                    b.Property<string>("TownName")
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("town_name");

                    b.Property<Tribe>("Tribe")
                        .HasColumnType("tribe")
                        .HasColumnName("tribe");

                    b.HasKey("Id")
                        .HasName("pk_characters");

                    b.HasIndex("FullFreeCompanyId")
                        .HasDatabaseName("ix_characters_full_free_company_id");

                    b.HasIndex("LodestoneId")
                        .IsUnique()
                        .HasDatabaseName("ix_characters_lodestone_id");

                    b.ToTable("characters", (string)null);
                });

            modelBuilder.Entity("NetStone.Cache.Db.Models.CharacterAttributes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int>("AttackMagicPotency")
                        .HasColumnType("integer")
                        .HasColumnName("attack_magic_potency");

                    b.Property<int>("AttackPower")
                        .HasColumnType("integer")
                        .HasColumnName("attack_power");

                    b.Property<int>("CharacterId")
                        .HasColumnType("integer")
                        .HasColumnName("character_id");

                    b.Property<int>("CriticalHitRate")
                        .HasColumnType("integer")
                        .HasColumnName("critical_hit_rate");

                    b.Property<int>("Defense")
                        .HasColumnType("integer")
                        .HasColumnName("defense");

                    b.Property<int>("Determination")
                        .HasColumnType("integer")
                        .HasColumnName("determination");

                    b.Property<int>("Dexterity")
                        .HasColumnType("integer")
                        .HasColumnName("dexterity");

                    b.Property<int>("DirectHitRate")
                        .HasColumnType("integer")
                        .HasColumnName("direct_hit_rate");

                    b.Property<int>("HealingMagicPotency")
                        .HasColumnType("integer")
                        .HasColumnName("healing_magic_potency");

                    b.Property<int>("Hp")
                        .HasColumnType("integer")
                        .HasColumnName("hp");

                    b.Property<int>("Intelligence")
                        .HasColumnType("integer")
                        .HasColumnName("intelligence");

                    b.Property<int>("MagicDefense")
                        .HasColumnType("integer")
                        .HasColumnName("magic_defense");

                    b.Property<int>("Mind")
                        .HasColumnType("integer")
                        .HasColumnName("mind");

                    b.Property<int>("MpGpCp")
                        .HasColumnType("integer")
                        .HasColumnName("mp_gp_cp");

                    b.Property<string>("MpGpCpParameterName")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("character varying(2)")
                        .HasColumnName("mp_gp_cp_parameter_name");

                    b.Property<int?>("Piety")
                        .HasColumnType("integer")
                        .HasColumnName("piety");

                    b.Property<int>("SkillSpeed")
                        .HasColumnType("integer")
                        .HasColumnName("skill_speed");

                    b.Property<int?>("SpellSpeed")
                        .HasColumnType("integer")
                        .HasColumnName("spell_speed");

                    b.Property<int>("Strength")
                        .HasColumnType("integer")
                        .HasColumnName("strength");

                    b.Property<int?>("Tenacity")
                        .HasColumnType("integer")
                        .HasColumnName("tenacity");

                    b.Property<int>("Vitality")
                        .HasColumnType("integer")
                        .HasColumnName("vitality");

                    b.HasKey("Id")
                        .HasName("pk_character_attributes");

                    b.HasIndex("CharacterId")
                        .IsUnique()
                        .HasDatabaseName("ix_character_attributes_character_id");

                    b.ToTable("character_attributes", (string)null);
                });

            modelBuilder.Entity("NetStone.Cache.Db.Models.CharacterClassJob", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int?>("CharacterId")
                        .HasColumnType("integer")
                        .HasColumnName("character_id");

                    b.Property<string>("CharacterLodestoneId")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("character_lodestone_id");

                    b.Property<ClassJob>("ClassJob")
                        .HasColumnType("class_job")
                        .HasColumnName("class_job");

                    b.Property<int>("ExpCurrent")
                        .HasColumnType("integer")
                        .HasColumnName("exp_current");

                    b.Property<int>("ExpMax")
                        .HasColumnType("integer")
                        .HasColumnName("exp_max");

                    b.Property<int>("ExpToGo")
                        .HasColumnType("integer")
                        .HasColumnName("exp_to_go");

                    b.Property<bool>("IsJobUnlocked")
                        .HasColumnType("boolean")
                        .HasColumnName("is_job_unlocked");

                    b.Property<bool>("IsSpecialized")
                        .HasColumnType("boolean")
                        .HasColumnName("is_specialized");

                    b.Property<short>("Level")
                        .HasColumnType("smallint")
                        .HasColumnName("level");

                    b.HasKey("Id")
                        .HasName("pk_character_class_jobs");

                    b.HasIndex("CharacterId")
                        .HasDatabaseName("ix_character_class_jobs_character_id");

                    b.HasIndex("CharacterLodestoneId", "ClassJob")
                        .IsUnique()
                        .HasDatabaseName("ix_character_class_jobs_character_lodestone_id_class_job");

                    b.ToTable("character_class_jobs", (string)null);
                });

            modelBuilder.Entity("NetStone.Cache.Db.Models.CharacterFreeCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<string>("BottomLayer")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("bottom_layer");

                    b.Property<int>("CharacterId")
                        .HasColumnType("integer")
                        .HasColumnName("character_id");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("link");

                    b.Property<string>("LodestoneId")
                        .IsRequired()
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("lodestone_id");

                    b.Property<string>("MiddleLayer")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("middle_layer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("name");

                    b.Property<string>("TopLayer")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("top_layer");

                    b.HasKey("Id")
                        .HasName("pk_character_free_companies");

                    b.HasIndex("CharacterId")
                        .IsUnique()
                        .HasDatabaseName("ix_character_free_companies_character_id");

                    b.ToTable("character_free_companies", (string)null);
                });

            modelBuilder.Entity("NetStone.Cache.Db.Models.CharacterGear", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int>("CharacterId")
                        .HasColumnType("integer")
                        .HasColumnName("character_id");

                    b.Property<string>("CreatorName")
                        .HasMaxLength(21)
                        .HasColumnType("character varying(21)")
                        .HasColumnName("creator_name");

                    b.Property<string>("GlamourDatabaseLink")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("glamour_database_link");

                    b.Property<string>("GlamourName")
                        .HasMaxLength(63)
                        .HasColumnType("character varying(63)")
                        .HasColumnName("glamour_name");

                    b.Property<bool?>("IsHq")
                        .HasColumnType("boolean")
                        .HasColumnName("is_hq");

                    b.Property<string>("ItemDatabaseLink")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("item_database_link");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasMaxLength(63)
                        .HasColumnType("character varying(63)")
                        .HasColumnName("item_name");

                    b.Property<string>("Materia1")
                        .HasMaxLength(63)
                        .HasColumnType("character varying(63)")
                        .HasColumnName("materia1");

                    b.Property<string>("Materia2")
                        .HasMaxLength(63)
                        .HasColumnType("character varying(63)")
                        .HasColumnName("materia2");

                    b.Property<string>("Materia3")
                        .HasMaxLength(63)
                        .HasColumnType("character varying(63)")
                        .HasColumnName("materia3");

                    b.Property<string>("Materia4")
                        .HasMaxLength(63)
                        .HasColumnType("character varying(63)")
                        .HasColumnName("materia4");

                    b.Property<string>("Materia5")
                        .HasMaxLength(63)
                        .HasColumnType("character varying(63)")
                        .HasColumnName("materia5");

                    b.Property<GearSlot>("Slot")
                        .HasColumnType("gear_slot")
                        .HasColumnName("slot");

                    b.Property<string>("StrippedItemName")
                        .HasMaxLength(63)
                        .HasColumnType("character varying(63)")
                        .HasColumnName("stripped_item_name");

                    b.HasKey("Id")
                        .HasName("pk_character_gears");

                    b.HasIndex("CharacterId", "Slot")
                        .IsUnique()
                        .HasDatabaseName("ix_character_gears_character_id_slot");

                    b.ToTable("character_gears", (string)null);
                });

            modelBuilder.Entity("NetStone.Cache.Db.Models.CharacterMinion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int?>("CharacterId")
                        .HasColumnType("integer")
                        .HasColumnName("character_id");

                    b.Property<string>("CharacterLodestoneId")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("character_lodestone_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(63)
                        .HasColumnType("character varying(63)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_character_minions");

                    b.HasIndex("CharacterId")
                        .HasDatabaseName("ix_character_minions_character_id");

                    b.HasIndex("CharacterLodestoneId", "Name")
                        .IsUnique()
                        .HasDatabaseName("ix_character_minions_character_lodestone_id_name");

                    b.ToTable("character_minions", (string)null);
                });

            modelBuilder.Entity("NetStone.Cache.Db.Models.CharacterMount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int?>("CharacterId")
                        .HasColumnType("integer")
                        .HasColumnName("character_id");

                    b.Property<string>("CharacterLodestoneId")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("character_lodestone_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(63)
                        .HasColumnType("character varying(63)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_character_mounts");

                    b.HasIndex("CharacterId")
                        .HasDatabaseName("ix_character_mounts_character_id");

                    b.HasIndex("CharacterLodestoneId", "Name")
                        .IsUnique()
                        .HasDatabaseName("ix_character_mounts_character_lodestone_id_name");

                    b.ToTable("character_mounts", (string)null);
                });

            modelBuilder.Entity("NetStone.Cache.Db.Models.FreeCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<short>("ActiveMemberCount")
                        .HasColumnType("smallint")
                        .HasColumnName("active_member_count");

                    b.Property<string>("ActiveState")
                        .IsRequired()
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("active_state");

                    b.Property<string>("CrestBottom")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("crest_bottom");

                    b.Property<string>("CrestMiddle")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("crest_middle");

                    b.Property<string>("CrestTop")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("crest_top");

                    b.Property<string>("EstateGreeting")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("estate_greeting");

                    b.Property<string>("EstateName")
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("estate_name");

                    b.Property<string>("EstatePlot")
                        .HasMaxLength(63)
                        .HasColumnType("character varying(63)")
                        .HasColumnName("estate_plot");

                    b.Property<int>("Focus")
                        .HasColumnType("integer")
                        .HasColumnName("focus");

                    b.Property<DateTime>("Formed")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("formed");

                    b.Property<DateTime?>("FreeCompanyMembersUpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("free_company_members_updated_at");

                    b.Property<DateTime>("FreeCompanyUpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("free_company_updated_at");

                    b.Property<GrandCompany>("GrandCompany")
                        .HasColumnType("grand_company")
                        .HasColumnName("grand_company");

                    b.Property<short>("ImmortalFlamesProgress")
                        .HasColumnType("smallint")
                        .HasColumnName("immortal_flames_progress");

                    b.Property<string>("ImmortalFlamesRank")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)")
                        .HasColumnName("immortal_flames_rank");

                    b.Property<string>("LodestoneId")
                        .IsRequired()
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("lodestone_id");

                    b.Property<short>("MaelstromProgress")
                        .HasColumnType("smallint")
                        .HasColumnName("maelstrom_progress");

                    b.Property<string>("MaelstromRank")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)")
                        .HasColumnName("maelstrom_rank");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("name");

                    b.Property<short>("Rank")
                        .HasColumnType("smallint")
                        .HasColumnName("rank");

                    b.Property<short?>("RankingMonthly")
                        .HasColumnType("smallint")
                        .HasColumnName("ranking_monthly");

                    b.Property<short?>("RankingWeekly")
                        .HasColumnType("smallint")
                        .HasColumnName("ranking_weekly");

                    b.Property<string>("Recruitment")
                        .IsRequired()
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("recruitment");

                    b.Property<string>("Slogan")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("slogan");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)")
                        .HasColumnName("tag");

                    b.Property<short>("TwinAdderProgress")
                        .HasColumnType("smallint")
                        .HasColumnName("twin_adder_progress");

                    b.Property<string>("TwinAdderRank")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)")
                        .HasColumnName("twin_adder_rank");

                    b.Property<string>("World")
                        .IsRequired()
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("world");

                    b.HasKey("Id")
                        .HasName("pk_free_companies");

                    b.HasIndex("LodestoneId")
                        .IsUnique()
                        .HasDatabaseName("ix_free_companies_lodestone_id");

                    b.ToTable("free_companies", (string)null);
                });

            modelBuilder.Entity("NetStone.Cache.Db.Models.FreeCompanyMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("avatar");

                    b.Property<string>("CharacterLodestoneId")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("character_lodestone_id");

                    b.Property<string>("DataCenter")
                        .IsRequired()
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("data_center");

                    b.Property<int?>("FreeCompanyId")
                        .HasColumnType("integer")
                        .HasColumnName("free_company_id");

                    b.Property<string>("FreeCompanyLodestoneId")
                        .IsRequired()
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("free_company_lodestone_id");

                    b.Property<int?>("FullCharacterId")
                        .HasColumnType("integer")
                        .HasColumnName("full_character_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("character varying(21)")
                        .HasColumnName("name");

                    b.Property<string>("Rank")
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("rank");

                    b.Property<string>("RankIcon")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("rank_icon");

                    b.Property<string>("Server")
                        .IsRequired()
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("server");

                    b.HasKey("Id")
                        .HasName("pk_free_company_members");

                    b.HasIndex("FreeCompanyId")
                        .HasDatabaseName("ix_free_company_members_free_company_id");

                    b.HasIndex("FullCharacterId")
                        .IsUnique()
                        .HasDatabaseName("ix_free_company_members_full_character_id");

                    b.HasIndex("CharacterLodestoneId", "FreeCompanyLodestoneId")
                        .IsUnique()
                        .HasDatabaseName("ix_free_company_members_character_lodestone_id_free_company_lo");

                    b.ToTable("free_company_members", (string)null);
                });

            modelBuilder.Entity("NetStone.Cache.Db.Models.Character", b =>
                {
                    b.HasOne("NetStone.Cache.Db.Models.FreeCompany", "FullFreeCompany")
                        .WithMany("MembersCachedCharacters")
                        .HasForeignKey("FullFreeCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_characters_free_companies_full_free_company_id");

                    b.Navigation("FullFreeCompany");
                });

            modelBuilder.Entity("NetStone.Cache.Db.Models.CharacterAttributes", b =>
                {
                    b.HasOne("NetStone.Cache.Db.Models.Character", "Character")
                        .WithOne("Attributes")
                        .HasForeignKey("NetStone.Cache.Db.Models.CharacterAttributes", "CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_character_attributes_characters_character_id");

                    b.Navigation("Character");
                });

            modelBuilder.Entity("NetStone.Cache.Db.Models.CharacterClassJob", b =>
                {
                    b.HasOne("NetStone.Cache.Db.Models.Character", "Character")
                        .WithMany("CharacterClassJobs")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_character_class_jobs_characters_character_id");

                    b.Navigation("Character");
                });

            modelBuilder.Entity("NetStone.Cache.Db.Models.CharacterFreeCompany", b =>
                {
                    b.HasOne("NetStone.Cache.Db.Models.Character", "Character")
                        .WithOne("FreeCompany")
                        .HasForeignKey("NetStone.Cache.Db.Models.CharacterFreeCompany", "CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_character_free_companies_characters_character_id");

                    b.Navigation("Character");
                });

            modelBuilder.Entity("NetStone.Cache.Db.Models.CharacterGear", b =>
                {
                    b.HasOne("NetStone.Cache.Db.Models.Character", "Character")
                        .WithMany("Gear")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_character_gears_characters_character_id");

                    b.Navigation("Character");
                });

            modelBuilder.Entity("NetStone.Cache.Db.Models.CharacterMinion", b =>
                {
                    b.HasOne("NetStone.Cache.Db.Models.Character", "Character")
                        .WithMany("Minions")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_character_minions_characters_character_id");

                    b.Navigation("Character");
                });

            modelBuilder.Entity("NetStone.Cache.Db.Models.CharacterMount", b =>
                {
                    b.HasOne("NetStone.Cache.Db.Models.Character", "Character")
                        .WithMany("Mounts")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_character_mounts_characters_character_id");

                    b.Navigation("Character");
                });

            modelBuilder.Entity("NetStone.Cache.Db.Models.FreeCompanyMember", b =>
                {
                    b.HasOne("NetStone.Cache.Db.Models.FreeCompany", "FreeCompany")
                        .WithMany("Members")
                        .HasForeignKey("FreeCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_free_company_members_free_companies_free_company_id");

                    b.HasOne("NetStone.Cache.Db.Models.Character", "FullCharacter")
                        .WithOne("FreeCompanyMembership")
                        .HasForeignKey("NetStone.Cache.Db.Models.FreeCompanyMember", "FullCharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_free_company_members_characters_full_character_id");

                    b.Navigation("FreeCompany");

                    b.Navigation("FullCharacter");
                });

            modelBuilder.Entity("NetStone.Cache.Db.Models.Character", b =>
                {
                    b.Navigation("Attributes")
                        .IsRequired();

                    b.Navigation("CharacterClassJobs");

                    b.Navigation("FreeCompany");

                    b.Navigation("FreeCompanyMembership");

                    b.Navigation("Gear");

                    b.Navigation("Minions");

                    b.Navigation("Mounts");
                });

            modelBuilder.Entity("NetStone.Cache.Db.Models.FreeCompany", b =>
                {
                    b.Navigation("Members");

                    b.Navigation("MembersCachedCharacters");
                });
#pragma warning restore 612, 618
        }
    }
}
