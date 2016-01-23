namespace Mac65
{
    public struct Label
    {
        private readonly ushort value;
        private readonly string name;

        public Label(string name, int value)
        {
            this.name = name;
            this.value = (ushort)(value & 0xFFFF);
        }

        public string Name
        {
            get { return name; }
        }

        public int Value
        {
            get { return value; }
        }

        public bool Equals(Label other)
        {
            return string.Equals(name, other.name) && value == other.value;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((name != null ? name.GetHashCode() : 0)*397) ^ value;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Label && Equals((Label) obj);
        }
    }
}