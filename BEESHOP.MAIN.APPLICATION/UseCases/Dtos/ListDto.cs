namespace BEESHOP.MAIN.APPLICATION.UseCases.Dtos;

public class ListDto<T>
{
    public int Count { get; set; }

    public required List<T> Entities { get; set; }
}
