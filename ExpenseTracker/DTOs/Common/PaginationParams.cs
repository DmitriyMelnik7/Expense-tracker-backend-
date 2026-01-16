using FluentValidation;

namespace ExpenseTracker.Api.DTOs.Common
{
    public sealed class PaginationParams
    {
        public const int MaxPageSize = 100;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }

    public sealed class PaginationParamsValidator : AbstractValidator<PaginationParams>
    {
        public PaginationParamsValidator()
        {
            RuleFor(p => p.PageNumber).
                GreaterThan(0);

            RuleFor(p => p.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(PaginationParams.MaxPageSize);
        }
    }
}
