/*
 * File name            : AppDbContext.cs
 * Author               : Munirul Islam
 * Date                 : 16 October 2026
 * Version              : 1.0
 *
 * Description          : database mapping information
 *
 * Modification history :
 * Name                         Date                            Desc
 * 
 * 
 * Copyright (c) 2026: THIS IS WRITTEN BY MUNIRUL ISLAM  FOR VEXCASH TEST PROJECT
 */

using MessageBird.Entities;
using MessageBird.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBird.DatabaseContext
{

    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApiResponse> ApiResponse =>
           Set<ApiResponse>();

        public DbSet<customerInfo> Customer_info =>
            Set<customerInfo>();

        public DbSet<MessageTemplate> messageTemplate =>
            Set<MessageTemplate>();

        public DbSet<MessageResponse> SmsMessage =>
            Set<MessageResponse>();

        public DbSet<MessageRecipient> SmsRecipient =>
            Set<MessageRecipient>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          
  

            modelBuilder.Entity<ApiResponse>(entity =>
            {
                entity.ToTable("apiResponse");
                entity.Property(x => x.Id);
                entity.Property(x=>x.EntryDate)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

                // Existing customer table
                modelBuilder.Entity<customerInfo>(entity =>
            {
                entity.ToTable("Customer_info");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(14);

                entity.HasIndex(x => x.PhoneNumber)
                    .IsUnique();

                entity.Property(x => x.CustName)
                    .HasColumnName("cust_name")
                    .HasMaxLength(60);

                entity.Property(x => x.CreditNumber)
                    .HasColumnName("credit_number")
                    .HasMaxLength(20);

                entity.Property(x => x.Amount)
                    .HasColumnName("amount")
                    .HasPrecision(8, 2);

                entity.Property(x => x.DueDate)
                    .HasColumnName("due_date")
                    .HasColumnType("TEXT");

                entity.Property(x => x.IsSend)
                    .HasColumnName("isSend")
                    .HasMaxLength(1);
            });

            // Existing message template table
            modelBuilder.Entity<MessageTemplate>(entity =>
            {
                entity.ToTable("message_template");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.MsgType)
                    .HasColumnName("msgType")
                    .HasMaxLength(15);

                entity.Property(x => x.TmsgText)
                    .HasColumnName("tmsgText")
                    .HasMaxLength(500);
            });

            // Bird message response table
            modelBuilder.Entity<MessageResponse>(entity =>
            {
                entity.ToTable("SmsMessage");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(x => x.TypeDetailsJson)
                    .HasColumnName("type_details_json")
                    .HasColumnType("TEXT");

                entity.Property(x => x.TotalCount)
                    .HasColumnName("total_count");

                entity.Property(x => x.TotalSentCount)
                    .HasColumnName("total_sent_count");

                entity.Property(x => x.TotalDeliveredCount)
                    .HasColumnName("total_delivered_count");

                entity.Property(x => x.TotalDeliveryFailedCount)
                    .HasColumnName("total_delivery_failed_count");

                entity.HasMany(x => x.Recipients)
                    .WithOne(x => x.Message)
                    .HasForeignKey(x => x.MessageId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MessageRecipient>(entity =>
            {
                entity.ToTable("SmsRecipient");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.MessageId)
                    .HasColumnName("message_id");

                entity.Property(x => x.Recipient)
                    .HasColumnName("recipient")
                    .HasMaxLength(20);

                entity.Property(x => x.Status)
                    .HasColumnName("status");

                entity.Property(x => x.StatusDatetime)
                    .HasColumnName("status_datetime");
            });
        }
    }
}