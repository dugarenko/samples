namespace AppTest
{
	internal class Country
	{
        private static int _id = 1;

        internal Country()
        {
            Id = _id++;
        }

        public int Id
        { get; private set; }

        public string DisplayName
		{ get; set; }

		public string NativeName
		{ get; set; }

		public string ThreeLetterISOLanguageName
		{ get; set; }
	}
}
