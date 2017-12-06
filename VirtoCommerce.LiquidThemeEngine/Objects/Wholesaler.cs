using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class Wholesaler : Drop
    {
        public string Url { get; set; }
        public string Logo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
    }
}
