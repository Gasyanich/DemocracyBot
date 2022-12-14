// <auto-generated />
using System;
using DemocracyBot.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DemocracyBot.DataAccess.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221024190053_UpdatePollQuestion")]
    partial class UpdatePollQuestion
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BotUserChat", b =>
                {
                    b.Property<long>("ChatsId")
                        .HasColumnType("bigint");

                    b.Property<long>("UsersId")
                        .HasColumnType("bigint");

                    b.HasKey("ChatsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("BotUserChat");
                });

            modelBuilder.Entity("BotUserMeet", b =>
                {
                    b.Property<long>("MeetsId")
                        .HasColumnType("bigint");

                    b.Property<long>("UsersId")
                        .HasColumnType("bigint");

                    b.HasKey("MeetsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("BotUserMeet");
                });

            modelBuilder.Entity("DemocracyBot.DataAccess.Entities.BotUser", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DemocracyBot.DataAccess.Entities.Chat", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsNotificationsActivated")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("DemocracyBot.DataAccess.Entities.Meet", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Place")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.ToTable("Meets");
                });

            modelBuilder.Entity("DemocracyBot.DataAccess.Entities.Poll", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Question")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Polls");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Poll");
                });

            modelBuilder.Entity("DemocracyBot.DataAccess.Entities.PollAnswer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<byte>("Number")
                        .HasColumnType("tinyint");

                    b.Property<string>("PollId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VoteCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PollId");

                    b.ToTable("PollAnswers");
                });

            modelBuilder.Entity("DemocracyBot.DataAccess.Entities.RestrictPoll", b =>
                {
                    b.HasBaseType("DemocracyBot.DataAccess.Entities.Poll");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasIndex("ChatId");

                    b.HasIndex("UserId");

                    b.HasDiscriminator().HasValue("RestrictPoll");
                });

            modelBuilder.Entity("BotUserChat", b =>
                {
                    b.HasOne("DemocracyBot.DataAccess.Entities.Chat", null)
                        .WithMany()
                        .HasForeignKey("ChatsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DemocracyBot.DataAccess.Entities.BotUser", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BotUserMeet", b =>
                {
                    b.HasOne("DemocracyBot.DataAccess.Entities.Meet", null)
                        .WithMany()
                        .HasForeignKey("MeetsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DemocracyBot.DataAccess.Entities.BotUser", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DemocracyBot.DataAccess.Entities.Meet", b =>
                {
                    b.HasOne("DemocracyBot.DataAccess.Entities.Chat", "Chat")
                        .WithMany("Meets")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");
                });

            modelBuilder.Entity("DemocracyBot.DataAccess.Entities.PollAnswer", b =>
                {
                    b.HasOne("DemocracyBot.DataAccess.Entities.Poll", "Poll")
                        .WithMany("Answers")
                        .HasForeignKey("PollId");

                    b.Navigation("Poll");
                });

            modelBuilder.Entity("DemocracyBot.DataAccess.Entities.RestrictPoll", b =>
                {
                    b.HasOne("DemocracyBot.DataAccess.Entities.Chat", "Chat")
                        .WithMany("Polls")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DemocracyBot.DataAccess.Entities.BotUser", "User")
                        .WithMany("RestrictPolls")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DemocracyBot.DataAccess.Entities.BotUser", b =>
                {
                    b.Navigation("RestrictPolls");
                });

            modelBuilder.Entity("DemocracyBot.DataAccess.Entities.Chat", b =>
                {
                    b.Navigation("Meets");

                    b.Navigation("Polls");
                });

            modelBuilder.Entity("DemocracyBot.DataAccess.Entities.Poll", b =>
                {
                    b.Navigation("Answers");
                });
#pragma warning restore 612, 618
        }
    }
}
