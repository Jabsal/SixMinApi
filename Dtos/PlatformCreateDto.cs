namespace SixMinApi.Dtos
{
    using System.ComponentModel.DataAnnotations;
    public class PlatformCreateDto
    {
        [Required] public string? PlatformName { get; set; }
    }
}
