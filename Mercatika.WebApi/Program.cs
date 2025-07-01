using Mercatika.Business;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Servicios registrados
builder.Services.AddScoped<OrderDetailBusiness>(
    sp => new OrderDetailBusiness(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<OrderBusiness>(
    sp => new OrderBusiness(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<ProductBusiness>();

builder.Services.AddScoped<ClientBusiness>(
    sp => new ClientBusiness(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<CompanyBusiness>(
    sp => new CompanyBusiness(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<PaymentBusiness>(
    sp => new PaymentBusiness(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<PaymentMethodBusiness>(
    sp => new PaymentMethodBusiness(builder.Configuration.GetConnectionString("DefaultConnection"))
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
