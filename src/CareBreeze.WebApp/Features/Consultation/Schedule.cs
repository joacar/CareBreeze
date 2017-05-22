using CareBreeze.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CareBreeze.WebApp.Features.Consultation
{
    public static class Schedule
    {
        public class PatientRegistered : INotification
        {
            public int Id { get; private set; }

            public PatientRegistered(int id)
            {
                Id = id;
            }
        }

        public class PatientRegisteredHandler : IAsyncNotificationHandler<PatientRegistered>
        {
            private readonly CareBreezeDbContext _context;

            public PatientRegisteredHandler(CareBreezeDbContext context)
            {
                _context = context;
            }

            public async Task Handle(PatientRegistered notification)
            {
                var patient = await _context.Set<Data.Domain.Patient>()
                    .Include(p => p.Condition)
                    .FirstAsync(p => p.Id == notification.Id);
                // Find those doctors that has 
                // the correct roles to treat the patient
                var doctorResources = new List<Data.Domain.Doctor>();
                var doctors = await _context.Set<Data.Domain.Doctor>()
                    .Include(d => d.Roles)
                        .ThenInclude(r => r.Role)
                    .ToListAsync();
                foreach (var doctor in doctors)
                {
                    if (doctor.CanTreat(patient))
                    {
                        doctorResources.Add(doctor);
                    }
                }
                // The doctor resources that can treat the patient's condition are known
                var treatmentRooms = await _context.Set<Data.Domain.TreatmentRoom>()
                    .Include(r => r.TreatmentMachine)
                        .ThenInclude(m => m.Capability)
                    .ToListAsync();

                var treatmentRoomsResources = new Dictionary<string, Data.Domain.TreatmentRoom>();

                foreach (var room in treatmentRooms)
                {
                    foreach (var doctor in doctorResources)
                    {
                        if (doctor.CanBook(patient, room) && !treatmentRoomsResources.ContainsKey(room.Name))
                        {
                            treatmentRoomsResources.Add(room.Name, room);
                        }
                    }
                }
                // So we have doctor resources and treatment room resources that conforms
                // to the rules for treating the patient. 
                // Check time and resource constraint
                // for doctor and rooms to create consultation record as soon as possible
                // but no earlier than the patient was registered
                var startDate = patient.Registered;
                var consultationCreated = false;
                do
                {
                    startDate = startDate.AddDays(1);
                    foreach (var doctor in doctorResources)
                    {
                        // Check if doctor is already booked for this day then we can skip check
                        var doctorBusy = await _context.Set<Data.Domain.Consultation>()
                            .AnyAsync(c => c.DoctorId == doctor.Id && c.ConsultationDate == startDate);
                        if (doctorBusy)
                        {
                            continue;
                        }
                        foreach (var room in treatmentRoomsResources.Values)
                        {
                            var roomBusy = await _context.Set<Data.Domain.Consultation>()
                                .AnyAsync(c => c.TreatmentRoomId == room.TreatmentMachineId
                                && c.ConsultationDate == startDate);
                            if (roomBusy)
                            {
                                continue;
                            }
                            var consultation = new Data.Domain.Consultation
                            {
                                Patient = patient,
                                Doctor = doctor,
                                TreatmentRoom = room,
                                ConsultationDate = startDate
                            };
                            try
                            {
                                _context.Add(consultation);
                                await _context.SaveChangesAsync();
                                consultationCreated = true;
                                break;
                            }
                            catch(DbUpdateException)
                            {
                                // Consultation could not be created due to
                                // unique constraint violation. Either
                                // 1. DoctorId and ConsultationDate
                                // 2. TreatmentRoomId and ConsultationDate
                                // 3. PatientId and ConsultationDate
                                _context.Entry(consultation).State = EntityState.Detached;
                            }
                            catch (Exception)
                            {
                                // Consultation could not be created
                                _context.Entry(consultation).State = EntityState.Detached;
                            }
                        }
                        if (consultationCreated)
                        {
                            break;
                        }
                    }
                } while (!consultationCreated);
            }
        }
    }
}
