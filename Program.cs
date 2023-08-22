using CarBuilder.Models;


#region Lists
//  Paint Colors
List<PaintColor> paintColors = new List<PaintColor>()
{
    new PaintColor()
    {
        Id = 1,
        Price = 500.50M,
        Color = "Silver"
    },
    new PaintColor()
    {
        Id = 2,
        Price = 750.85M,
        Color = "Midnight Blue"
    },
    new PaintColor()
    {
        Id = 3,
        Price = 800.90M,
        Color = "Firebrick Red"
    },
    new PaintColor()
    {
        Id = 4,
        Price = 650.00M,
        Color = "Spring Green"
    }
};

//  Interiors
List<Interior> interiors = new List<Interior>()
{
    new Interior()
    {
        Id = 1,
        Price = 630.50M,
        Material = "Beige Fabric"
    },
    new Interior()
    {
        Id = 2,
        Price = 650.00M,
        Material = "Charcoal Fabric"
    },
    new Interior()
    {
        Id = 3,
        Price = 780.80M,
        Material = "White Leather"
    },
    new Interior()
    {
        Id = 4,
        Price = 740.25M,
        Material = "Black Leather"
    }
};

// Technology
List<Technology> technologies = new List<Technology>()
{
    new Technology()
    {
        Id = 1,
        Price = 500.00M,
        Package = "Basic Package"
    },
    new Technology()
    {
        Id = 2,
        Price = 660.50M,
        Package = "Navigation Package"
    },
    new Technology()
    {
        Id = 3,
        Price = 750.95M,
        Package = "Visibility Package"
    },
    new Technology()
    {
        Id = 4,
        Price = 1100.00M,
        Package = "Ultra Package"
    }
};

// Wheels
List<Wheels> wheels = new List<Wheels>()
{
    new Wheels()
    {
        Id = 1,
        Price = 400.74M,
        Style = "17-inch Pair Radial"
    },
    new Wheels()
    {
        Id = 2,
        Price = 430.25M,
        Style = "17-inch Pair Radial Black"
    },
    new Wheels()
    {
        Id = 3,
        Price = 520.55M,
        Style = "18-inch Pair Spoke Silver"
    },
    new Wheels()
    {
        Id = 4,
        Price = 530.95M,
        Style = "18-inch Pair Spoke Black"
    },
};

// Orders
List<Order> orders = new List<Order>()
{
    new Order()
    {
        Id = 1,
        Timestamp = new DateTime(2023, 08, 22),
        WheelId = 4,
        TechnologyId = 2,
        PaintId = 1,
        InteriorId = 3
    },
    new Order()
    {
        Id = 2,
        Timestamp = new DateTime(2023, 07, 14),
        WheelId = 3,
        TechnologyId = 1,
        PaintId = 4,
        InteriorId = 2
    },
    new Order()
    {
        Id = 3,
        Timestamp = new DateTime(2023, 02, 09),
        WheelId = 2,
        TechnologyId = 4,
        PaintId = 3,
        InteriorId = 1
    },
    new Order()
    {
        Id = 4,
        Timestamp = new DateTime(2023, 04, 29),
        WheelId = 1,
        TechnologyId = 3,
        PaintId = 2,
        InteriorId = 4
    }
};
#endregion


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region Endpoints
    //  Paint Colors
    app.MapGet("/paintcolors", () => 
    {
        return paintColors;
    });

    //  Interiors
    app.MapGet("/interiors", () => 
    {
        return interiors;
    });

    //  Technology
    app.MapGet("/technologies", () =>
    {
        return technologies;
    });

    //  Wheels
    app.MapGet("/wheels", () =>
    {
        return wheels;
    });

    #region Orders
        //  All Orders
        app.MapGet("/orders", () =>
        {
            foreach (Order order in orders)
            {
                order.Wheels = wheels.FirstOrDefault(w => w.Id == order.WheelId);
                order.Technology = technologies.FirstOrDefault(t => t.Id == order.TechnologyId);
                order.PaintColor = paintColors.FirstOrDefault(p => p.Id == order.PaintId);
                order.Interior = interiors.FirstOrDefault(i => i.Id == order.InteriorId);
            }
            return orders;
        });

        //  Order by ID
        app.MapGet("/orders/{id}", (int id) =>
        {
            Order order = orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return Results.NotFound();
            }
            order.Wheels = wheels.FirstOrDefault(w => w.Id == order.WheelId);
            order.Technology = technologies.FirstOrDefault(t => t.Id == order.TechnologyId);
            order.PaintColor = paintColors.FirstOrDefault(p => p.Id == order.PaintId);
            order.Interior = interiors.FirstOrDefault(i => i.Id == order.InteriorId);
            return Results.Ok(order);
        });

        //  Create Order
        app.MapPost("/orders", (Order order) =>
        {
            order.Id = orders.Count > 0 ? orders.Max(o => o.Id) + 1 : 1;
            order.Timestamp = DateTime.Now;
            orders.Add(order);
            return order;
        });

    #endregion

#endregion

app.Run();