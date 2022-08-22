using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingSystem.Domain.Enums;
using MediatR;

namespace CinemaBookingSystem.Application.Recommendations.Queries.WhichRecommendation
{
    public class WhichRecommendationQuery : IRequest<RecommendationType>
    {
    }
}
