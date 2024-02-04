using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MiniProjektAPI.Data;
using MiniProjektAPI.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using System.Net;

namespace MiniProjektAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("ApplicationContext");
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
           
            var app = builder.Build();


            app.MapGet("/", () => "Hello world!");

            //h�mta alla
            app.MapGet("/people", (ApplicationContext context) =>
            {
                var people = context.Person
                    .Include(p => p.Interests)
                    .Include(p => p.InterestLinks)
                    .ToList();

                var result = people.GroupBy(
                    p => new
                    {
                        p.PersonId,
                        p.FirstName,
                        p.LastName,
                        p.PhoneNumber
                    },
                    p => new
                    {
                        Interest = p.Interests.Select(i => new { i.InterestId, i.Title, i.Descriptions }),
                        InterestLinks = p.InterestLinks.Select(il => new { il.InterestLinkId, il.Url })
                    })
                    .Select(group => new
                    {
                        group.Key.PersonId,
                        group.Key.FirstName,
                        group.Key.LastName,
                        group.Key.PhoneNumber,
                        Interests = group.SelectMany(g => g.Interest).ToList(),
                        InterestLinks = group.SelectMany(g => g.InterestLinks).ToList()
                    })
                    .ToList();

                return Results.Json(result);
            });

            //h�mta personens intresse
            app.MapGet("/person/{id}/interests", (ApplicationContext context, int id) =>
            {
                var person = context.Person.Include(p => p.Interests).FirstOrDefault(p => p.PersonId == id);
                if (person != null)
                {
                    return Results.Json(person.Interests);
                }
                else
                {
                    return Results.NotFound();
                }
            });

            //h�mta personens l�nkar
            app.MapGet("/person/{id}/links", (ApplicationContext context, int id) =>
            {
                var person = context.Person
                    .Include(p => p.InterestLinks)
                    .FirstOrDefault(p => p.PersonId == id);

                if (person != null)
                {
                    var simplifiedLinks = person.InterestLinks
                        .Select(link => new
                        {
                            PersonName = $"{person.FirstName} {person.LastName}",
                            LinkUrl = link.Url
                        });

                    var jsonOptions = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve,
                        // eventuella andra alternativ eller anpassningar h�r
                    };

                    var json = JsonSerializer.Serialize(simplifiedLinks, jsonOptions);

                    return Results.Text(json, "application/json");
                }
                else
                {
                    return Results.NotFound();
                }
            });

            // h�mta en ny person genom post till api
            app.MapPost("/new/person", (ApplicationContext context, Person person) =>
            {
                if (person != null)
                {
                    // L�gg till intresse om det finns
                    if (person.Interests != null && person.Interests.Any())
                    {
                        foreach (var interest in person.Interests)
                        {
                            context.Interests.Add(interest);

                            // L�gg till l�nkar om det finns
                            if (interest.InterestLinks != null && interest.InterestLinks.Any())
                            {
                                foreach (var link in interest.InterestLinks)
                                {
                                    link.Person = person;
                                    link.Interest = interest;
                                    context.InterestLinks.Add(link);
                                }
                            }
                        }
                    }

                    // L�gg till person i databasen efter att intressen och l�nkar �r tillagda
                    context.Person.Add(person);
                    context.SaveChanges();

                    return Results.StatusCode((int)HttpStatusCode.Created);
                }
                else
                {
                    return Results.BadRequest();
                }
            });

            //l�gga till ett nytt intresse till en befinting person
            app.MapPost("/person/{personId}/interest", (ApplicationContext context, int personId, Interest interest) =>
            {
                var person = context.Person.Find(personId);

                if (person != null && interest != null)
                {
                    person.Interests ??= new List<Interest>();

                    person.Interests.Add(interest);
                    context.SaveChanges();
                }
                else
                {
                    return Results.NotFound();
                }
                return Results.StatusCode((int)HttpStatusCode.Created);
            });

            //l�gg till en ny l�nk till en perosn
            app.MapPost("/person/{personId}/interest/{interestId}/link", async (HttpContext httpContext, ApplicationContext context, int personId, int interestId) =>
            {
                var person = await context.Person.FindAsync(personId);
                var interest = await context.Interests.FindAsync(interestId);

                if (person != null && interest != null)
                {
                    try
                    {
                        using (JsonDocument document = await JsonDocument.ParseAsync(httpContext.Request.Body))
                        {
                            var linkUrl = document.RootElement.GetProperty("Url").GetString();

                            if (!string.IsNullOrEmpty(linkUrl))
                            {
                                context.InterestLinks.Add(new InterestLink
                                {
                                    PersonId = personId,
                                    InterestId = interestId,
                                    Url = linkUrl
                                });

                                await context.SaveChangesAsync();
                            }
                            else
                            {
                                return Results.BadRequest("Invalid data");
                            }
                        }
                    }
                    catch (JsonException)
                    {
                        return Results.BadRequest("Not Correct JSON format.");
                    }
                }
                else
                {
                    return Results.NotFound();
                }
                return Results.StatusCode((int)HttpStatusCode.Created);
            });

            app.Run();
        }
    }
}
