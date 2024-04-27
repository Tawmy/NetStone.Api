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
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "class_job", new[] { "gladiator", "pugilist", "marauder", "lancer", "archer", "conjurer", "thaumaturge", "carpenter", "blacksmith", "armorer", "goldsmith", "leatherworker", "weaver", "alchemist", "culinarian", "miner", "botanist", "fisher", "paladin", "monk", "warrior", "dragoon", "bard", "white_mage", "black_mage", "arcanist", "summoner", "scholar", "rogue", "ninja", "machinist", "dark_knight", "astrologian", "samurai", "red_mage", "blue_mage", "gunbreaker", "dancer", "reaper", "sage" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "grand_company", new[] { "no_affiliation", "maelstrom", "order_of_the_twin_adder", "immortal_flames" });
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

                    b.Property<DateTime>("CharacterUpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("character_updated_at");

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

                    b.Property<string>("RaceClanGender")
                        .IsRequired()
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("race_clan_gender");

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

                    b.HasKey("Id")
                        .HasName("pk_characters");

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
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("materia1");

                    b.Property<string>("Materia2")
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("materia2");

                    b.Property<string>("Materia3")
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("materia3");

                    b.Property<string>("Materia4")
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("materia4");

                    b.Property<string>("Materia5")
                        .HasMaxLength(31)
                        .HasColumnType("character varying(31)")
                        .HasColumnName("materia5");

                    b.Property<int>("Slot")
                        .HasColumnType("integer")
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

            modelBuilder.Entity("NetStone.Cache.Db.Models.Character", b =>
                {
                    b.Navigation("Attributes")
                        .IsRequired();

                    b.Navigation("CharacterClassJobs");

                    b.Navigation("FreeCompany");

                    b.Navigation("Gear");
                });
#pragma warning restore 612, 618
        }
    }
}
