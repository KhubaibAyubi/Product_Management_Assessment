using FastEndpoints;
using Product_Management_Assessment.DTO;
using System.Net;

namespace Product_Management_Assessment.EndPoints
{
    public class CreateNewProductEndpoint : Endpoint<ProductRequest, ProductResponse>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteProductEndpoints> _logger;
        public CreateNewProductEndpoint(AppDbContext context, ILogger<DeleteProductEndpoints> logger)
        {
            _context = context;
            _logger = logger;
        }
        public override void Configure()
        {
            Post("/Postproducts");
            AllowAnonymous();
          
        }
        public override async Task HandleAsync(ProductRequest req, CancellationToken ct)
        {

            try
            {
                var product = new Product
                {
                    Name = req.Name,
                    Description = req.Description,
                    Price = req.Price
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync(ct);

                await SendAsync(new ProductResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CreatedDate = DateTime.Now
                });
            }
            catch (Exception ex) {
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