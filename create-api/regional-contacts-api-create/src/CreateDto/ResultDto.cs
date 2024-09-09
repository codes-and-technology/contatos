namespace CreateDto
{
    public class ResultDto<T> where T : class
    {
        public List<string> Errors { get; set; } = new List<string>();
        public T Data { get; set; }
        public bool Success { get { return !Errors.Any(); } }
    }
}
