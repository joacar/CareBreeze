using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CareBreeze.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Condition",
                columns: table => new
                {
                    Value = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condition", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "Doctor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Value = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentMachineCapability",
                columns: table => new
                {
                    Value = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentMachineCapability", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConditionId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Registered = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patient_Condition_ConditionId",
                        column: x => x.ConditionId,
                        principalTable: "Condition",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorRole",
                columns: table => new
                {
                    DoctorId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorRole", x => new { x.DoctorId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_DoctorRole_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentMachine",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    CapabilityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentMachine", x => x.Name);
                    table.ForeignKey(
                        name: "FK_TreatmentMachine_TreatmentMachineCapability_CapabilityId",
                        column: x => x.CapabilityId,
                        principalTable: "TreatmentMachineCapability",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentRoom",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    TreatmentMachineId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentRoom", x => x.Name);
                    table.ForeignKey(
                        name: "FK_TreatmentRoom_TreatmentMachine_TreatmentMachineId",
                        column: x => x.TreatmentMachineId,
                        principalTable: "TreatmentMachine",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Consultation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConsultationDate = table.Column<DateTime>(type: "date", nullable: false),
                    DoctorId = table.Column<int>(nullable: false),
                    PatientId = table.Column<int>(nullable: false),
                    RegistrationDate = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    TreatmentRoomId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultation", x => x.Id);
                    table.UniqueConstraint("AK_Consultation_DoctorId_ConsultationDate", x => new { x.DoctorId, x.ConsultationDate });
                    table.UniqueConstraint("AK_Consultation_PatientId_ConsultationDate", x => new { x.PatientId, x.ConsultationDate });
                    table.UniqueConstraint("AK_Consultation_TreatmentRoomId_ConsultationDate", x => new { x.TreatmentRoomId, x.ConsultationDate });
                    table.ForeignKey(
                        name: "FK_Consultation_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Consultation_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Consultation_TreatmentRoom_TreatmentRoomId",
                        column: x => x.TreatmentRoomId,
                        principalTable: "TreatmentRoom",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorRole_RoleId",
                table: "DoctorRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_ConditionId",
                table: "Patient",
                column: "ConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentMachine_CapabilityId",
                table: "TreatmentMachine",
                column: "CapabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentRoom_TreatmentMachineId",
                table: "TreatmentRoom",
                column: "TreatmentMachineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Consultation");

            migrationBuilder.DropTable(
                name: "DoctorRole");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "TreatmentRoom");

            migrationBuilder.DropTable(
                name: "Doctor");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Condition");

            migrationBuilder.DropTable(
                name: "TreatmentMachine");

            migrationBuilder.DropTable(
                name: "TreatmentMachineCapability");
        }
    }
}
