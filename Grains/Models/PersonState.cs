namespace Grains.Models;

[GenerateSerializer]
public class PersonState
{
    [Id(0)]
    public string Name { get; set; }
    
    [Id(1)]
    public string? GroupName { get; set; }

    [Id(2)] 
    public int MessagesSent { get; set; } = 0;

}