namespace ClinicSystem.Dtos
{
    public class ApiResponse<T>
    {
        public T data { get; set; }
        public int dataCount { get; set; }
        public string ? errorMsg { get; set; }

        public ApiResponse() { }

        public ApiResponse(T data,int dataCount)
        {
            this.data = data;
            this.dataCount = dataCount;
        }

        public ApiResponse(string errorMsg)
        {
            this.errorMsg = errorMsg;
        }

        public ApiResponse(T data)
        {
            this.data = data;
        } 

    }
}
