namespace TCMount
{
    class Automounter
    {
        private string _file = null;
        private string _key = null;
        private string _letter = null;


        public Automounter(string file, string letter, string key = null)
        {
            _file = file;
            _letter = letter;
            _key = key;
        }

        public string Dismount()
        {
            TruecryptWrapper wrapper = new TruecryptWrapper();
            return wrapper.Dismount(_letter);
        }

        public string Mount(string password)
        {
            string result = "";
            TruecryptWrapper wrapper = new TruecryptWrapper();
            result += wrapper.Mount(_file, _letter, password, _key);
            return result;
        }
    }
}
