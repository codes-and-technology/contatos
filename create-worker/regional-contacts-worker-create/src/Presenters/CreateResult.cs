namespace Presenters
{
    public class CreateResult<T> where T : class
    {
        public CreateResult()
        {
                
        }

        public CreateResult(T data)
        {
            this.Data = data;
        }

        public List<string> Errors { get; set; } = new List<string>();
        public T Data { get; set; }
        public bool Success { get { return !Errors.Any(); } }
    }
}
