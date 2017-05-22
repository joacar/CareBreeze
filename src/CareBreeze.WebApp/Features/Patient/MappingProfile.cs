using AutoMapper;
using CareBreeze.Data;
using CareBreeze.Data.Domain;
using Feature = CareBreeze.WebApp.Features.Patient;

namespace CareBreeze.WebApp.Features
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Feature.Create.Command, Data.Domain.Patient>()
                .ForMember(target => target.Condition, e => e.Ignore())
                .AfterMap((source, target) =>
                {
                    target.ConditionId = Enumeration.FromName<Condition>(source.Condition).Value;
                });

            CreateMap<Data.Domain.Patient, Feature.Index.Patient>();

        }

    }
}
