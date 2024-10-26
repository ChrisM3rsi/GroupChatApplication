namespace Grains.Models;

[GenerateSerializer]
public class GroupChatState 
{
    [Id(0)]
    public IList<PersonState> Persons { get; set; } = new List<PersonState>();
    
    [Id(1)]
    public IList<string> Messages { get; set; } = new List<string>();
}