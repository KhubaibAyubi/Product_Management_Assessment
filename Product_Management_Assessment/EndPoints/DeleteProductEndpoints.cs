using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Product_Management_Assessment.DTO;
using System.Net;

namespace Product_Management_Assessment.EndPoints
{
    public class DeleteProductEndpoints : Endpoint<ProductDeleteRequest, EmptyResponse>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteProductEndpoints> _logger;
        public DeleteProductEndpoints(AppDbContext context, ILogger<DeleteProductEndpoints> logger)
        {
            _context = context;
            _logger = logger;
        }

        public override void Configure()
        {
            Delete("/DeleteProduct/{id:int}");
            AllowAnonymous();
           
        }

        public override async Task HandleAsync(ProductDeleteRequest req, CancellationToken ct)
        {
            try
            {
                // Find the product by ID
                var product = await _context.Products.FindAsync(req.Id);
                if (product is null)
                {
                    _logger.LogWarning($"Product with ID {req.Id} not found.");
                    await SendNotFoundAsync();
                    return;
                }

                // Remove the product from the database
                _context.Products.Remove(product);
                await _context.SaveChangesAsync(ct);

                // Send a success response
                await SendOkAsync();
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
