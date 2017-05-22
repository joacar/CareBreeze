using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CareBreeze.Data;

namespace CareBreeze.Data.Migrations
{
    [DbContext(typeof(CareBreezeDbContext))]
    [Migration("20170521233932_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CareBreeze.Data.Domain.Condition", b =>
                {
                    b.Property<int>("Value");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Value");

                    b.ToTable("Condition");

                    b.HasDiscriminator<string>("Type").HasValue("Condition");
                });

            modelBuilder.Entity("CareBreeze.Data.Domain.Consultation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ConsultationDate")
                        .HasColumnType("date");

                    b.Property<int>("DoctorId");

                    b.Property<int>("PatientId");

                    b.Property<DateTime>("RegistrationDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("TreatmentRoomId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAlternateKey("DoctorId", "ConsultationDate");


                    b.HasAlternateKey("PatientId", "ConsultationDate");


                    b.HasAlternateKey("TreatmentRoomId", "ConsultationDate");

                    b.ToTable("Consultation");
                });

            modelBuilder.Entity("CareBreeze.Data.Domain.Doctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Doctor");
                });

            modelBuilder.Entity("CareBreeze.Data.Domain.DoctorRole", b =>
                {
                    b.Property<int>("DoctorId");

                    b.Property<int>("RoleId");

                    b.HasKey("DoctorId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("DoctorRole");
                });

            modelBuilder.Entity("CareBreeze.Data.Domain.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ConditionId");

                    b.Property<string>("Name");

                    b.Property<DateTime>("Registered")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.HasKey("Id");

                    b.HasIndex("ConditionId");

                    b.ToTable("Patient");
                });

            modelBuilder.Entity("CareBreeze.Data.Domain.Role", b =>
                {
                    b.Property<int>("Value");

                    b.Property<string>("Name");

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Value");

                    b.ToTable("Role");

                    b.HasDiscriminator<string>("Type").HasValue("Role");
                });

            modelBuilder.Entity("CareBreeze.Data.Domain.TreatmentMachine", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CapabilityId");

                    b.HasKey("Name");

                    b.HasIndex("CapabilityId");

                    b.ToTable("TreatmentMachine");
                });

            modelBuilder.Entity("CareBreeze.Data.Domain.TreatmentMachineCapability", b =>
                {
                    b.Property<int>("Value");

                    b.Property<string>("Name");

                    b.HasKey("Value");

                    b.ToTable("TreatmentMachineCapability");
                });

            modelBuilder.Entity("CareBreeze.Data.Domain.TreatmentRoom", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("TreatmentMachineId");

                    b.HasKey("Name");

                    b.HasIndex("TreatmentMachineId");

                    b.ToTable("TreatmentRoom");
                });

            modelBuilder.Entity("CareBreeze.Data.Domain.Cancer", b =>
                {
                    b.HasBaseType("CareBreeze.Data.Domain.Condition");


                    b.ToTable("Cancer");

                    b.HasDiscriminator().HasValue("Cancer");
                });

            modelBuilder.Entity("CareBreeze.Data.Domain.Flu", b =>
                {
                    b.HasBaseType("CareBreeze.Data.Domain.Condition");


                    b.ToTable("Flu");

                    b.HasDiscriminator().HasValue("Flu");
                });

            modelBuilder.Entity("CareBreeze.Data.Domain.GeneralPractitioner", b =>
                {
                    b.HasBaseType("CareBreeze.Data.Domain.Role");


                    b.ToTable("GeneralPractitioner");

                    b.HasDiscriminator().HasValue("GeneralPractitioner");
                });

            modelBuilder.Entity("CareBreeze.Data.Domain.Oncologist", b =>
                {
                    b.HasBaseType("CareBreeze.Data.Domain.Role");


                    b.ToTable("Oncologist");

                    b.HasDiscriminator().HasValue("Oncologist");
                });

            modelBuilder.Entity("CareBreeze.Data.Domain.Consultation", b =>
                {
                    b.HasOne("CareBreeze.Data.Domain.Doctor", "Doctor")
                        .WithMany()
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CareBreeze.Data.Domain.Patient", "Patient")
                        .WithMany("Consultations")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CareBreeze.Data.Domain.TreatmentRoom", "TreatmentRoom")
                        .WithMany()
                        .HasForeignKey("TreatmentRoomId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CareBreeze.Data.Domain.DoctorRole", b =>
                {
                    b.HasOne("CareBreeze.Data.Domain.Doctor", "Doctor")
                        .WithMany("Roles")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CareBreeze.Data.Domain.Role", "Role")
                        .WithMany("Doctors")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CareBreeze.Data.Domain.Patient", b =>
                {
                    b.HasOne("CareBreeze.Data.Domain.Condition", "Condition")
                        .WithMany()
                        .HasForeignKey("ConditionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CareBreeze.Data.Domain.TreatmentMachine", b =>
                {
                    b.HasOne("CareBreeze.Data.Domain.TreatmentMachineCapability", "Capability")
                        .WithMany()
                        .HasForeignKey("CapabilityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CareBreeze.Data.Domain.TreatmentRoom", b =>
                {
                    b.HasOne("CareBreeze.Data.Domain.TreatmentMachine", "TreatmentMachine")
                        .WithMany("Rooms")
                        .HasForeignKey("TreatmentMachineId");
                });
        }
    }
}
