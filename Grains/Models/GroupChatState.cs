namespace Grains.Models;

[GenerateSerializer]
public class GroupChatState 
{
    [Id(0)]
    public IList<PersonState> Persons { get; set; } = new List<PersonState>();
    
    [Id(1)]
    public IList<Message> Messages { get; set; } = new List<Message>();
}