
namespace Steam_Authenticator.Model
{
    public class SelectItem
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is SelectItem other)
            {
                return Equals(other);
            }

            return false;
        }

        public bool Equals(SelectItem other)
        {
            return (Value ?? "").Equals(other?.Value ?? "") && (Name ?? "").Equals(other?.Name ?? "");
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Value);
        }
    }
}
