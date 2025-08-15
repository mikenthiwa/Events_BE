using AutoMapper;
using Domain.Entities;

namespace Application.Features.Events.Command.CreateEvent;

public class EventDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Venue { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Event, EventDto>();
        }
        
    }
}
