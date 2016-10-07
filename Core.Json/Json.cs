
namespace Core.Json
{
    public class Json
    {
        private JsonData _data;
        private int _error = 0;

        public Json(string val, string __type)
        {
            this._data = new JsonData(val, __type);
        }

        public JsonData data
        {
            get
            {
                return this._data;
            }
        }

        public int error
        {
            get
            {
                return this._error;
            }
            set
            {
                this._error = value;
            }
        }
    }
}
