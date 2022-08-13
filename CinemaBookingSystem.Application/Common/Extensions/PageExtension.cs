using CinemaBookingSystem.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Common.Extensions
{
    public static class PageExtension
    {
        public static async Task<PagedModel<TModel>> PaginateAsync<TModel>(
            this IQueryable<TModel> query,
            int page,
            int limit,
            CancellationToken cancellationToken)
            where TModel : class
        {

            var paged = new PagedModel<TModel>();

            page = (page < 0) ? 1 : page;

            paged.CurrentPage = page;
            paged.PageSize = limit;

            var startRow = (page - 1) * limit;
            paged.Items = await query
                .Skip(startRow)
                .Take(limit)
                .ToListAsync(cancellationToken);

            var totalItemsCountTask = query.CountAsync(cancellationToken);

            paged.TotalItems = await totalItemsCountTask;
            paged.TotalPages = (int)Math.Ceiling(paged.TotalItems / (double)limit);

            return paged;
        }
    }
}
