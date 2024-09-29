namespace Presenters
{
    public class UpdateResult<T> where T : class
    {
        public UpdateResult()
        {
                
        }

        public UpdateResult(T data)
        {
            this.Data = data;
        }

        public List<string> Errors { get; set; } = new List<string>();
        public T Data { get; set; }
        public bool Success { get { return !Errors.Any(); } }
    }
}
