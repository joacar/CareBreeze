using CareBreeze.Data;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using PatientDomain = CareBreeze.Data.Domain.Patient;

namespace CareBreeze.WebApp.Features.Patient
{
    public static class Create
    {
        public class Command : IRequest<Result>
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public string Condition { get; set; }
        }

        public class Result : BaseResult
        {
            public int? PatientId { get; }

            public Result(int patientId)
            {
                PatientId = patientId;
            }

            private Result()
            {

            }

            public static Result InvalidCondition(string parameter, string value, IList<string> values)
            {
                var error = new EnumerationError
                {
                    Parameter = parameter,
                    Message = $"Value '{value}' is not a valid condition",
                    Values = values
                };
                var result = new Result();
                result.AddError(error);
                return result;
            }

            internal static Result Exception()
            {
                var result = new Result();
                result.AddError(Error.Generic);
                return result;
            }
        }

        public class Handler : IAsyncRequestHandler<Command, Result>
        {
            private readonly CareBreezeDbContext _context;
            private readonly IMediator _mediator;

            public Handler(CareBreezeDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }

            public async Task<Result> Handle(Command message)
            {
                PatientDomain patient;
                try
                {
                    patient = AutoMapper.Mapper.Map<PatientDomain>(message);
                }
                catch (Enumeration.InvalidValueException ive)
                {
                    return Result.InvalidCondition(nameof(Command.Condition), ive.Value, ive.Values);
                }
                catch
                {
                    return Result.Exception();
                }

                try
                {
                    _context.Add(patient);
                    await _context.SaveChangesAsync();
                    // This should be put on MQ for safe processing
                    await _mediator.Publish(new Consultation.Schedule.PatientRegistered(patient.Id));
                    return new Result(patient.Id);
                }
                catch
                {
                    return Result.Exception();
                }
            }
        }
    }
}
