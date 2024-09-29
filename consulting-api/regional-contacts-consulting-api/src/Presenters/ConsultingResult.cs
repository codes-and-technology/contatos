namespace Presenters
{
    public class ConsultingResult<T> where T : class
    {
        public ConsultingResult()
        {
                
        }

        public ConsultingResult(T data)
        {
            this.Data = data;
        }

        public List<string> Errors { get; set; } = new List<string>();
        public T Data { get; set; }
        public bool Success { get { return !Errors.Any(); } }
    }
}
