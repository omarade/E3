using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    //to do
                    AddPlatform(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMsg)
        {
            Console.WriteLine("----> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMsg);

            switch(eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("----> Platform Published Event Detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("----> Could not Determine Event Type");
                    return EventType.Undetermined;

            }
        }

        private void AddPlatform(string platformPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

                var platformReceiveDto = JsonSerializer.Deserialize<PlatformReceiveDto>(platformPublishedMessage);
                
                try
                {
                    var plat = _mapper.Map<Platform>(platformReceiveDto);
                    if (!repo.ExternalPlatformExists(plat.ExternalId))
                    {
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                        Console.WriteLine("----> Platform Added ");
                    }
                    else
                    {
                        Console.WriteLine("----> Platform already exisits ");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"----> Could not add Platform to DB: {ex.Message}");
                }
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}