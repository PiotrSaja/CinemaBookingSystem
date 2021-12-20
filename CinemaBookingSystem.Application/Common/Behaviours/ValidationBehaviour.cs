using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace CinemaBookingSystem.Application.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var failures = _validators.Select(x => x.Validate(context)).SelectMany(result => result.Errors)
                    .Where(f => f != null).ToList();


                if (failures.Count != 0)
                {
                    var failuresMessage = "";

                    foreach (var item in failures)
                    {
                        failuresMessage += $"{item.ToString()};";
                    }

                    throw new ValidationException(failures);
                }
            }

            return await next();
        }
    }
}
