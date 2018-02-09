namespace VersionChecker
{
    class CsprojVersion
    {
        public string Name { get; set; }
        public Version FileVersion { get; set; }
        public Version AssemblyVersion { get; set; }


    }

    class Version
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
        public int Revision { get; set; }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Build}.{Revision}";
        }
    }
}
