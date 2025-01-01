using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Product_Management_Assessment.DTO;
using System.Net;

namespace Product_Management_Assessment.EndPoints
{
    public class GetProductsEndpoint : EndpointWithoutRequest<List<ProductResponse>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetProductsEndpoint> _logger;
        public GetProductsEndpoint(AppDbContext context, ILogger<GetProductsEndpoint> logger)
        {
            _context = context;
            _logger = logger;
        }

        public override void Configure()
        {
            Get("/products");
            AllowAnonymous();
            //Options(o => o.WithTags("Products"));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var products = await _context.Products
               .Select(p => new ProductResponse
               {
                   Id = p.Id,
                   Name = p.Name,
                   Description = p.Description,
                   Price = p.Price,
                   CreatedDate = DateTime.Now
               })
               .ToListAsync(ct);

                await SendAsync(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting the product: {ex.Message}");
                var customerrorResponse = new CustomErrorResponse
                {
                    Error = "An error occurred while processing your request.",
                    Exception = ex.Message
                };
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await HttpContext.Response.WriteAsync(
                    $"{{\"error\":\"An error occurred while processing your request.\",\"exception\":\"{ex.Message}\"}}",
                    ct
                    );
            }


        }
    }
}