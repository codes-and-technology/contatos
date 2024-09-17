namespace Presenters
{
    public class DeleteResult<T> where T : class
    {
        public DeleteResult()
        {
                
        }

        public DeleteResult(T data)
        {
            this.Data = data;
        }

        public List<string> Errors { get; set; } = new List<string>();
        public T Data { get; set; }
        public bool Success { get { return !Errors.Any(); } }
    }
}
