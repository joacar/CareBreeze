using AutoMapper.QueryableExtensions;
using CareBreeze.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareBreeze.WebApp.Features.Patient
{
    public static class Index
    {
        public class Query : IRequest<Result>
        {
            public int? Id { get; set; }
        }

        public class Result : BaseResult
        {
            public IList<Patient> Patients { get; set; }
        }

        public class Patient
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public DateTime Registered { get; set; }

            public string Condition => Enumeration.FromValue<Data.Domain.Condition>(ConditionId).Name;

            [JsonIgnore]
            public int ConditionId { get; set; }
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
                var query = _context.Set<Data.Domain.Patient>().AsQueryable();
                if (message.Id.HasValue)
                {
                    query = query.Where(p => p.Id == message.Id.Value);
                }
                var patients = await query.ProjectTo<Patient>().ToListAsync();
                var result = new Result
                {
                    Patients = patients
                };
                return result;
            }
        }
    }
}
