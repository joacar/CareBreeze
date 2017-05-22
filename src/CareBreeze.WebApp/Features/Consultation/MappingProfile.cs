using AutoMapper;

namespace CareBreeze.WebApp.Features.Consultation
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Data.Domain.Consultation, Index.Consultation>()
                .ForMember(s => s.Room, e => e.MapFrom(t => t.TreatmentRoom.Name))
                .ForMember(s => s.Doctor, e => e.MapFrom(t => t.Doctor.Name))
                .ForMember(s => s.Patient, e => e.MapFrom(t => t.Patient.Name))
                .ForMember(s => s.ConditionId, e => e.MapFrom(t => t.Patient.ConditionId));

        }
    }
}
