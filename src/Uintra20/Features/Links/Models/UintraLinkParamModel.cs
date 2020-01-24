namespace Uintra20.Features.Links.Models
{
    public class UintraLinkParamModel
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Name}={Value}";
        }
    }
}