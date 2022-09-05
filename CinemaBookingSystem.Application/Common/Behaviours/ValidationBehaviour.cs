using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using FluentValidation;
using MediatR;

namespace CinemaBookingSystem.Application.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        #region ValidationBehaviour()
        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        #endregion

        #region Handle()
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

                    throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, failuresMessage);
                }
            }

            return await next();
        }
        #endregion
    }
}
