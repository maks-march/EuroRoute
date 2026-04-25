namespace Domain.Models.Abstract;

public interface IManyFiles<TFile>
{
    public IList<TFile> Photos { get; set; }
}