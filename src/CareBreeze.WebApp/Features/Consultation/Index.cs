using AutoMapper.QueryableExtensions;
using CareBreeze.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CareBreeze.WebApp.Features.Consultation
{
    public static class Index
    {
        public class Query : IRequest<Result>
        {

        }

        public class Result : BaseResult
        {
            public IList<Consultation> Consultations { get; set; }
        }

        public class Consultation
        {
            public string Patient { get; set; }

            public string Doctor { get; set; }

            public string Room { get; set; }

            public string Conditation => Enumeration.FromValue<Data.Domain.Condition>(ConditionId).Name;

            [JsonIgnore]
            public int ConditionId { get; set; }

            public DateTime ConsultationDate { get; set; }

            public DateTime RegistrationDate { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, Result>
        {
            private readonly CareBreezeDbContext _context;

            public QueryHandler(CareBreezeDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(Query message)
            {
                var result = new Result();
                try
                {
                    var consultations = await _context.Set<Data.Domain.Consultation>()
                                     .ProjectTo<Consultation>()
                                     .ToListAsync();
                    result.Consultations = consultations;
                }
                catch (Exception ex)
                {
                    result.AddError(new Error
                    {
                        Message = ex.Message
                    });
                }
                return result;
            }
        }
    }
}
